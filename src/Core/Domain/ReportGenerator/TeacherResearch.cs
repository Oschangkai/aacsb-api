using System.ComponentModel.DataAnnotations.Schema;
using MassTransit;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("TeacherResearch", Schema="ReportGenerator")]
public class TeacherResearch : BaseEntity
{
    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; }

    public Guid ResearchId { get; set; }
    public Research Research { get; set; }
}