using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("VisitorCard")]
public class VisitorCard
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string CardNumber { get; set; }

    public bool IsAssigned { get; set; }
}
