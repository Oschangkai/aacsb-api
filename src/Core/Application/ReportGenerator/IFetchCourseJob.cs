using System.ComponentModel;

namespace AACSB.WebApi.Application.ReportGenerator;

public interface IFetchCourseJob : IScopedService
{
    [DisplayName("Collect Courses from NTUST")]
    Task FetchAsync(int year, int semester, string[] departments, CancellationToken cancellationToken);
}