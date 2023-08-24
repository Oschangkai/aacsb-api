namespace AACSB.WebApi.Application.ReportGenerator.Teachers;

public class TeacherDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Title { get; set; }
    public string? DepartmentId { get; set; }
    public Guid? QualificationId { get; set; }
    public string? Degree { get; set; }
    public short? DegreeYear { get; set; }
    public string? WorkTypeAbbr { get; set; }
}