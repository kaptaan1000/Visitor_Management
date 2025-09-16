using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

namespace Visitor_Management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add MVC services
            builder.Services.AddControllersWithViews();

            // 👇 Add this line before building the app
            QuestPDF.Settings.License = LicenseType.Community;

            // Register DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            // 👇 This makes Visitors/Index the default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Visitors}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
