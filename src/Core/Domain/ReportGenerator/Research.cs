using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("Research", Schema="ReportGenerator")]
public class Research : AuditableEntity, IAggregateRoot
{
    public Teacher Teacher { get; set; }
    public Guid TeacherId { get; set; }

    [MaxLength(50)]
    [Comment("Journal 1, Journal 2, Presentation, Proceeding")]
    public string Type { get; set; }

    public string Title { get; set; }

    public string? OtherAuthors { get; set; }

    [Comment("第一作者/.../第四(以上)作者")]
    public string? OrderAuthors { get; set; }

    public bool? AddressAuthors { get; set; }

    public bool? Publication { get; set; }

    public string? Seminar { get; set; }

    public short? YearStart { get; set; }

    public byte? MonthStart { get; set; }

    public byte? DayStart { get; set; }

    public short? YearEnd { get; set; }

    public byte? MonthEnd { get; set; }

    public byte? DayEnd { get; set; }

    public short? Year { get; set; }

    public byte? Month { get; set; }

    [MaxLength(256)]
    public string? Country { get; set; }

    [MaxLength(256)]
    public string? City { get; set; }

    public short? PageStart { get; set; }

    public short? PageEnd { get; set; }

    [MaxLength(512)]
    public string? Project { get; set; }

    public bool? FullText { get; set; }

    public string? JournalsName { get; set; }

    [MaxLength(10)]
    [Comment("電子期刊/紙本")]
    public string? JournalsType { get; set; }

    [MaxLength(128)]
    public string? JournalsClass { get; set; }

    [MaxLength(10)]
    [Comment("已接受/已發表")]
    public string? JournalsStatus { get; set; }

    public short? Volume { get; set; }

    public short Issue { get; set; }
}