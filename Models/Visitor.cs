using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("VISITOR")]
public class Visitor
{
    [Key]
    public int Id { get; set; }   // Primary Key (not part of old VB6 schema but needed for EF)

    [Column("COMP")]
    [StringLength(4)]
    public string? Comp { get; set; }

    [Column("UNIT")]
    [StringLength(6)]
    public string? Unit { get; set; }

    [Column("DATE")]
    public DateTime? Date { get; set; }

    [Column("VSNM")]
    [StringLength(250)]
    public string? VisitorName { get; set; }

    [Column("VSCNM")]
    [StringLength(250)]
    public string? CompanyName { get; set; }

    [Column("VHCL")]
    [StringLength(50)]
    public string? VehicleNo { get; set; }

    [Column("TOMEET")]
    [StringLength(50)]
    public string? ToMeet { get; set; }

    [Column("INTM")]
    public DateTime? InTime { get; set; }

    [Column("OTTM")]
    public DateTime? OutTime { get; set; }

    [Column("PURPOSE")]
    [StringLength(50)]
    public string? Purpose { get; set; }

    [Column("REMARK")]
    [StringLength(50)]
    public string? Remark { get; set; }

    [Column("VSIMG")]
    public byte[]? VsImg { get; set; }

    [Column("EXTRA1")]
    [StringLength(50)]
    public string? Extra1 { get; set; }

    [Column("EXTRA2")]
    [StringLength(50)]
    public string? Extra2 { get; set; }

    [Column("EXTRA3")]
    [StringLength(50)]
    public string? Extra3 { get; set; }

    [Column("EXTRA4")]
    [StringLength(50)]
    public string? Extra4 { get; set; }

    [Column("EXTRA5")]
    [StringLength(50)]
    public string? Extra5 { get; set; }

    [Column("VID")]
    [StringLength(18)]
    public string? VID { get; set; }

    [Column("MOB")]
    [StringLength(12)]
    public string? Mobile { get; set; }

    [Column("VBNO")]
    [StringLength(10)]
    public string? VBNo { get; set; }

    // New relation with VisitorCard
    public int? VisitorCardId { get; set; }
    public VisitorCard? VisitorCard { get; set; }
}
