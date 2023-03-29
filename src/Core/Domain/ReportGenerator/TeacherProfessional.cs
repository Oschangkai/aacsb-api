using System.ComponentModel.DataAnnotations.Schema;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("TeacherProfessionals", Schema="ReportGenerator")]
public class TeacherProfessional : AuditableEntity, IAggregateRoot
{
    public string? Description { get; set; }

    public string? EnglishDescription { get; set; }

    public Guid? TeacherId { get; set; }
}