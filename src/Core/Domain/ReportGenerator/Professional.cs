using System.ComponentModel.DataAnnotations.Schema;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("Professionals", Schema="ReportGenerator")]
public class Professional : AuditableEntity, IAggregateRoot
{
    public string? Description { get; set; }

    public string? EnglishDescription { get; set; }
}