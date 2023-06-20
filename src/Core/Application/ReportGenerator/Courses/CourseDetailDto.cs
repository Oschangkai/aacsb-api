namespace AACSB.WebApi.Application.ReportGenerator.Courses;

public class CourseDetailDto : CourseDto
{
    public string EnglishName { get; set; }
    public decimal Credit { get; set; }
    public bool? Required { get; set; }
    public bool? Year { get; set; }
    public string? Time { get; set; }
    public string? ClassRoomNo { get; set; }
    public string? Contents { get; set; }
}