using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("Responsibilities", Schema="ReportGenerator")]
public class Responsibility : AuditableEntity, IAggregateRoot
{
    [MaxLength(80)]
    public string? Description { get; set; }

    [MaxLength(80)]
    public string? EnglishDescription { get; set; }

    [MaxLength(10)]
    public string? Abbreviation { get; set; }
}