using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("TeacherResearch", Schema="ReportGenerator")]
public class TeacherResearch : AuditableEntity, IAggregateRoot
{
    public string? Description { get; set; }

    public string? EnglishDescription { get; set; }

    public Guid? TeacherId { get; set; }
}