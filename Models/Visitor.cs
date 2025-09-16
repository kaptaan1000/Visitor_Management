using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("VISITOR")]
public class Visitor
{
    [Key]
    public int Id { get; set; }   // 👈 new identity column you added

    [Column("COMP")] public string? Comp { get; set; }   // char(4), nullable

    [Column("UNIT")] public string? Unit { get; set; }   // char(6), nullable

    [Column("DATE")] public DateTime? Date { get; set; }   // datetime, nullable

    [Column("VSNM")] public string? VisitorName { get; set; }

    [Column("VSCNM")] public string? CompanyName { get; set; }

    [Column("VHCL")] public string? VehicleNo { get; set; }

    [Column("TOMEET")] public string? ToMeet { get; set; }

    [Column("INTM")] public DateTime? InTime { get; set; }

    [Column("OTTM")] public DateTime? OutTime { get; set; }

    [Column("PURPOSE")] public string? Purpose { get; set; }

    [Column("REMARK")] public string? Remark { get; set; }

    [Column("VSIMG")] public byte[]? VsImg { get; set; }

    [Column("EXTRA1")] public string? Extra1 { get; set; }

    [Column("EXTRA2")] public string? Extra2 { get; set; }

    [Column("EXTRA3")] public string? Extra3 { get; set; }

    [Column("EXTRA4")] public string? Extra4 { get; set; }

    [Column("EXTRA5")] public string? Extra5 { get; set; }

    [Column("VID")] public string? VID { get; set; }

    [Column("MOB")] public string? Mobile { get; set; }

    [Column("VBNO")] public string? VBNo { get; set; }
}
