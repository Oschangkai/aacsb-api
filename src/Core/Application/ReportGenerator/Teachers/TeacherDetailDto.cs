namespace AACSB.WebApi.Application.ReportGenerator.Teachers;

public class TeacherDetailDto : TeacherDto
{
    public string? EnglishName { get; set; }
    public string Email { get; set; }
    public DateTime? ResignDate { get; set; }
}