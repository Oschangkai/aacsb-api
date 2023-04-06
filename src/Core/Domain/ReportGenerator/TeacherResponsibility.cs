using System.ComponentModel.DataAnnotations.Schema;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("TeacherResponsibilities", Schema="ReportGenerator")]
public class TeacherResponsibility : BaseEntity
{
    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; }

    public Guid ResponsibilityId { get; set; }
    public Responsibility Responsibility { get; set; }
}