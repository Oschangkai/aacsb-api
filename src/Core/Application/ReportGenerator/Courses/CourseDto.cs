namespace AACSB.WebApi.Application.ReportGenerator.Courses;

public class CourseDto : IDto
{
    public Guid Id { get; set; }
    public decimal Semester { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid DisciplineId { get; set; }
    public Guid ImportSignatureId { get; set; }
}