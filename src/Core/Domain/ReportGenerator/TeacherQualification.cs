using System.ComponentModel.DataAnnotations.Schema;
using MassTransit;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("TeacherQualifications", Schema="ReportGenerator")]
public class TeacherQualification : BaseEntity
{
    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; }

    public Guid QualificationId { get; set; }
    public Qualification Qualification { get; set; }
}