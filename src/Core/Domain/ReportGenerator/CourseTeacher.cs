namespace AACSB.WebApi.Domain.ReportGenerator;

public class CourseTeacher : BaseEntity
{
    public Guid CourseId { get; set; }
    public Course Course { get; set; }

    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; }
}