using System.ComponentModel;

namespace AACSB.WebApi.Application.ReportGenerator.Courses;

public interface IImportCourseDisciplineJob : IScopedService
{
    [DisplayName("Import Courses from Json")]
    Task ImportAsync(List<CourseDiscipline> courseDisciplines, CancellationToken cancellationToken);
}