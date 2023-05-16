using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AACSB.WebApi.Domain.ReportGenerator.Function;

public class TableA31
{
    [Precision(38, 15)]
    public decimal? DisciplineTotal { get; set; }

    public Guid? TeacherId { get; set; }

    [MaxLength(80)]
    public string? Teacher { get; set; }

    [MaxLength(42)]
    public string? Degree { get; set; }

    [Comment("MT/RES/... 可以為多值")]
    [MaxLength(100)]
    public string? Responsibilities { get; set; }

    [MaxLength(80)]
    public string? Qualification { get; set; }

    [Comment("P=Full Time, F=Part Time, C=Contractual")]
    [MaxLength(10)]
    public string? WorkType { get; set; }

    [Precision(2, 0)]
    public decimal Discipline { get; set; }
}