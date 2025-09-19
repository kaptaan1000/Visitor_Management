using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

public class VisitorViewModel
{
    public int? Id { get; set; }

    // Auto-filled, required, but user cannot edit
    [Required]
    public DateTime Date { get; set; } = DateTime.Now;

    // Required
    [Required(ErrorMessage = "Visitor name is required")]
    [Display(Name = "Visitor Name")]   // 👈 this controls the label text
    [StringLength(250)]
    public string VisitorName { get; set; }

    [Display(Name = "Company Name")]   // 👈 this controls the label text
    [StringLength(250)]
    public string? CompanyName { get; set; }

    [Display(Name = "Vehicle No.")]   // 👈 this controls the label text
    [StringLength(50)]
    public string? VehicleNo { get; set; }

    // Required
    [Required(ErrorMessage = "Please enter whom to meet")]
    [StringLength(50)]
    public string ToMeet { get; set; }

    // Auto-filled, required, but user cannot edit
    [Required]
    public DateTime InTime { get; set; } = DateTime.Now;

    // Optional
    public DateTime? OutTime { get; set; }

    [StringLength(100)]
    public string? Purpose { get; set; }

    [StringLength(250)]
    public string? Remark { get; set; }

    // Optional
    public IFormFile? Photo { get; set; }

    // Required
    // 👇 Mobile Validation
    [Required(ErrorMessage = "Mobile number is required")]
    [RegularExpression(@"^[1-9][0-9]{9}$", ErrorMessage = "Enter valid 10 digit mobile number")]
    public string Mobile { get; set; }

    public string? Comp { get; set; }
    public string? Unit { get; set; }

    [Required(ErrorMessage = "Please select a Visitor Pass ID")]
    public int VisitorCardId { get; set; }

}
