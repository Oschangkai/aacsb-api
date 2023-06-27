namespace AACSB.WebApi.Application.ReportGenerator.Teachers;

public class ATeacherDto
{
    public string Teacher { get; set; }
    public string? TeacherEnglishName { get; set; }
    public string? Degree { get; set; }
    public decimal? DegreeYear { get; set; }
    public string? Responsibilities { get; set; }
    public string? Qualification { get; set; }
}