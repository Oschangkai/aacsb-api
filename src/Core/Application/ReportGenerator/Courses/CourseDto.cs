namespace AACSB.WebApi.Application.ReportGenerator.Courses;

public class CourseDto : IDto
{
    public Guid Id { get; set; }
    public decimal Semester { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Credit { get; set; }
    public string Time { get; set; }
}