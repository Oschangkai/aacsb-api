using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace AACSB.WebApi.Domain.ReportGenerator.Function;

[Table("TableA81", Schema="ReportGenerator")]
public class TableA81
{
    // Course Information
    [Precision(4, 2)]
    public decimal? DisciplineTotal { get; set; }

    [Precision(4, 1)]
    public decimal? CreditTotal { get; set; }

    [Precision(2, 0)]
    public decimal Discipline { get; set; }


    // Teacher Information
    [MaxLength(200)]
    public string? Teacher { get; set; }

    public Guid? TeacherId { get; set; }

    [Comment("P=Full Time, S=Part Time, C=Contractual")]
    [MaxLength(10)]
    public string? WorkType { get; set; }

    // Research Type Count
    public int? Journal1 { get; set; }
    public int? Journal2 { get; set; }
    public int? Others { get; set; }


    // Research Portfolio Count
    public int? Basic { get; set; }
    public int? Applied { get; set; }
    public int? Teaching { get; set; }
}