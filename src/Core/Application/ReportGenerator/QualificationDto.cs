namespace AACSB.WebApi.Application.ReportGenerator;

public class QualificationDto : IDto
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public string? EnglishDescription { get; set; }
    public string? Abbreviation { get; set; }
}