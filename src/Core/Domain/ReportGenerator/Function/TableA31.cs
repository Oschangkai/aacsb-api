using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AACSB.WebApi.Domain.ReportGenerator.Function;

[Table("TableA31", Schema="ReportGenerator")]
public class TableA31
{
    [Precision(4, 2)]
    public decimal? DisciplineTotal { get; set; }

    [Precision(4, 1)]
    public decimal? CreditTotal { get; set; }

    public Guid? TeacherId { get; set; }

    [MaxLength(80)]
    public string? Teacher { get; set; }

    [MaxLength(200)]
    public string? TeacherEnglishName { get; set; }

    [MaxLength(80)]
    public string? TeacherDepartment { get; set; }

    [MaxLength(10)]
    public string? Degree { get; set; }

    public short? DegreeYear { get; set; }

    [Comment("MT/RES/... 可以為多值")]
    [MaxLength(100)]
    public string? Responsibilities { get; set; }

    [MaxLength(80)]
    public string? Qualification { get; set; }

    [Comment("P=Full Time, S=Part Time, C=Contractual")]
    [MaxLength(10)]
    public string? WorkType { get; set; }

    [Precision(2, 0)]
    public decimal Discipline { get; set; }
}