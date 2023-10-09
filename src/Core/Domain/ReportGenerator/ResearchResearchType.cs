using System.ComponentModel.DataAnnotations.Schema;

namespace AACSB.WebApi.Domain.ReportGenerator;

[Table("ResearchResearchType", Schema="ReportGenerator")]
public class ResearchResearchType : BaseEntity
{
    public Guid ResearchId { get; set; }
    public Research Research { get; set; }

    public Guid ResearchTypeId { get; set; }
    public ResearchType ResearchType { get; set; }
}