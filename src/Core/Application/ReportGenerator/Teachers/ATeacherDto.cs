namespace AACSB.WebApi.Application.ReportGenerator.Teachers;

public class ATeacherDto
{
    public string Teacher { get; set; }
    public string? TeacherEnglishName { get; set; }
    public string? Degree { get; set; }
    public short? DegreeYear { get; set; }
    public string? Qualification { get; set; }
    public string? WorkType { get; set; }
    public Guid TeacherId { get; set; }
}