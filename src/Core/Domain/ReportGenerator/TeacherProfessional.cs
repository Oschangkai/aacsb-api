using System.ComponentModel.DataAnnotations.Schema;
using MassTransit;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("TeacherProfessionals", Schema="ReportGenerator")]
public class TeacherProfessional : BaseEntity
{
    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; }

    public Guid ProfessionalId { get; set; }
    public Professional Professional { get; set; }
}