using AACSB.WebApi.Application.ReportGenerator;
using AACSB.WebApi.Domain.ReportGenerator;

namespace AACSB.WebApi.Host.Controllers.ReportGenerator;

public class ReportDataController : VersionedApiController
{
    [HttpPost("course/collect")]
    [MustHavePermission(AACSBAction.Import, AACSBResource.ReportData)]
    [OpenApiOperation("Fetch courses from NTUST.", "")]
    public Task<JobEnqueuedResponse> CollectCourses(FetchCoursesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("discipline")]
    [MustHavePermission(AACSBAction.View, AACSBResource.ReportData)]
    [OpenApiOperation("Get Disciplines", "")]
    public Task<List<DisciplineDto>> GetDisciplines()
    {
        return Mediator.Send(new GetDisciplinesRequest());
    }

    [HttpGet("academic-year")]
    [MustHavePermission(AACSBAction.View, AACSBResource.ReportData)]
    [OpenApiOperation("Get Academic Years", "")]
    public Task<int[]> GetAcademicYears()
    {
        return Mediator.Send(new GetTableA31AcademicYearRequest());
    }
}