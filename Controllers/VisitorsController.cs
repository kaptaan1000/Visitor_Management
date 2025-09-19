using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class VisitorsController : Controller
{
    private readonly ApplicationDbContext _db;
    public VisitorsController(ApplicationDbContext db) { _db = db; }

    public async Task<IActionResult> Index(DateTime? date, string status, string q, int page = 1)
    {
        // default date is today
        var filterDate = date ?? DateTime.Today;

        var query = _db.Visitors
            .Include(v => v.VisitorCard) // include card details
            .Where(v => v.Date.HasValue && v.Date.Value.Date == filterDate.Date);

        // ✅ search filter
        if (!string.IsNullOrEmpty(q))
        {
            query = query.Where(v =>
                v.VisitorName.Contains(q) ||
                v.CompanyName.Contains(q) ||
                (v.VisitorCard != null && v.VisitorCard.CardNumber.Contains(q)) ||
                v.Mobile.Contains(q));
        }

        // ✅ status filter
        if (!string.IsNullOrEmpty(status))
        {
            if (status == "inside")
                query = query.Where(v => v.OutTime == null);
            else if (status == "exited")
                query = query.Where(v => v.OutTime != null);
        }

        var list = await query
            .OrderByDescending(v => v.InTime)
            .ToListAsync();

        // pass filters back to view
        ViewBag.SelectedDate = filterDate.ToString("yyyy-MM-dd");
        ViewBag.SelectedStatus = status;
        ViewBag.Query = q;

        return View(list);
    }

    public IActionResult Create()
    {
        var availableCards = _db.VisitorCards
            .Where(c => !c.IsAssigned)
            .ToList();

        ViewBag.VisitorCards = new SelectList(availableCards, "Id", "CardNumber");

        return View(new VisitorViewModel { Date = DateTime.Now });
    }

    [HttpGet]
    public async Task<IActionResult> GetVisitorByMobile(string mobile)
    {
        if (string.IsNullOrWhiteSpace(mobile))
            return Json(null);

        var visitor = await _db.Visitors
            .Where(v => v.Mobile == mobile)
            .OrderByDescending(v => v.Date)   // get most recent record
            .FirstOrDefaultAsync();

        if (visitor == null)
            return Json(null);

        return Json(new
        {
            visitor.VisitorName,
            visitor.CompanyName,
            visitor.VehicleNo
        });
    }

    [HttpPost]
    public async Task<IActionResult> MarkOut(int id)
    {
        var visitor = await _db.Visitors.Include(v => v.VisitorCard).FirstOrDefaultAsync(v => v.Id == id);
        if (visitor == null) return NotFound();

        // Only update if OutTime is still null
        if (!visitor.OutTime.HasValue)
        {
            visitor.OutTime = DateTime.Now;

            if (visitor.VisitorCard != null)
                visitor.VisitorCard.IsAssigned = false; // make card reusable

            await _db.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VisitorViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var availableCards = _db.VisitorCards.Where(c => !c.IsAssigned).ToList();
            ViewBag.VisitorCards = new SelectList(availableCards, "Id", "CardNumber");
            return View(vm);
        }

        // ✅ Check if same mobile already has an active visitor
        bool alreadyInside = await _db.Visitors
            .AnyAsync(v => v.Mobile == vm.Mobile && v.OutTime == null);

        if (alreadyInside)
        {
            ModelState.AddModelError("Mobile", $"Mobile {vm.Mobile} is already inside and not checked out.");
            var availableCards = _db.VisitorCards.Where(c => !c.IsAssigned).ToList();
            ViewBag.VisitorCards = new SelectList(availableCards, "Id", "CardNumber");
            return View(vm);
        }

        var visitor = new Visitor
        {
            Date = DateTime.Now,
            InTime = DateTime.Now,
            VisitorName = vm.VisitorName,
            CompanyName = vm.CompanyName,
            VehicleNo = vm.VehicleNo,
            ToMeet = vm.ToMeet,
            Purpose = vm.Purpose,
            Remark = vm.Remark,
            Mobile = vm.Mobile,
            VisitorCardId = vm.VisitorCardId
        };

        if (vm.Photo != null && vm.Photo.Length > 0)
        {
            using var ms = new MemoryStream();
            await vm.Photo.CopyToAsync(ms);
            visitor.VsImg = ms.ToArray();
        }

        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            _db.Visitors.Add(visitor);

            var card = await _db.VisitorCards.FindAsync(vm.VisitorCardId);
            if (card == null || card.IsAssigned)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("VisitorCardId", "This card is already assigned.");
                return View(vm);
            }

            card.IsAssigned = true;

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            TempData["LastVisitorId"] = visitor.Id;
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    private byte[] GenerateVisitorPdf(Visitor visitor)
    {
        // Load logo
        var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "logo.png");
        var logoData = System.IO.File.Exists(logoPath) ? System.IO.File.ReadAllBytes(logoPath) : null;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A6);
                page.Margin(20);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Content()
                    .Column(col =>
                    {
                        col.Spacing(8);

                        // Header row: logo (left) + title (center)
                        col.Item().Row(row =>
                        {
                            if (logoData != null)
                            {
                                row.ConstantItem(60).Height(40).Image(logoData).FitArea(); // 👈 safe scaling
                            }

                            row.RelativeItem().AlignCenter().Text("Visitor Pass")
                                .FontSize(16).SemiBold();
                        });

                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

                        // Show Visitor Pass Card No.
                        col.Item().Text($"Pass ID: {(visitor.VisitorCard != null ? visitor.VisitorCard.CardNumber : "-")}");

                        col.Item().Text($"Date: {visitor.Date:dd/MM/yyyy}");
                        col.Item().Text($"In Time: {visitor.InTime:HH:mm}");
                        col.Item().Text($"Visitor: {visitor.VisitorName}");
                        col.Item().Text($"Company: {visitor.CompanyName}");
                        col.Item().Text($"Mobile: {visitor.Mobile}");
                        col.Item().Text($"To Meet: {visitor.ToMeet}");
                        col.Item().Text($"Vehicle: {visitor.VehicleNo}");
                        col.Item().Text($"Purpose: {visitor.Purpose}");
                        //col.Item().Text($"Remark: {visitor.Remark}");

                        col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

                        col.Item().AlignCenter().Text("Thank you").Italic();
                    });
            });
        }).GeneratePdf();
    }

    public async Task<IActionResult> Details(int id)
    {
        var v = await _db.Visitors.FindAsync(id);
        if (v == null) return NotFound();
        return View(v);
    }

    public IActionResult Print(int id)
    {
        var visitor = _db.Visitors
            .Include(v => v.VisitorCard)  // 👈 ensures Pass ID is loaded
            .FirstOrDefault(v => v.Id == id);
        
        if (visitor == null) return NotFound();

        var pdfBytes = GenerateVisitorPdf(visitor);

        //return File(pdfBytes, "application/pdf", $"VisitorSlip_{visitor.Id}.pdf");


        // 👇 This tells browser to preview instead of download
        return File(pdfBytes, "application/pdf");
    }

}
