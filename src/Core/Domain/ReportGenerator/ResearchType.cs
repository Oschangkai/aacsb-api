using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("ResearchType", Schema="ReportGenerator")]
public class ResearchType : AuditableEntity, IAggregateRoot
{
    public string? Description { get; set; }

    public string? EnglishDescription { get; set; }

    public string? Title { get; set; }

    public string? Certification { get; set; }

    [MaxLength(80)]
    public string? Code { get; set; }

    public ICollection<ResearchResearchType>? Researches { get; set; }
}