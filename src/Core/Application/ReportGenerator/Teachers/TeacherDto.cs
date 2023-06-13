namespace AACSB.WebApi.Application.ReportGenerator.Teachers;

public class TeacherDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string DepartmentId { get; set; }
    public string Responsibilities { get; set; }
    public string WorkTypeAbbr { get; set; }
}