using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("Research", Schema="ReportGenerator")]
public class Research : AuditableEntity, IAggregateRoot
{
    public string? Description { get; set; }

    public string? EnglishDescription { get; set; }
}