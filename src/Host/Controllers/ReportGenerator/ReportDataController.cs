using AACSB.WebApi.Application.ReportGenerator;
using AACSB.WebApi.Application.ReportGenerator.Courses;
using AACSB.WebApi.Application.ReportGenerator.Reports;

namespace AACSB.WebApi.Host.Controllers.ReportGenerator;

public class ReportDataController : VersionedApiController
{
    [HttpPost("course/search")]
    [MustHavePermission(AACSBAction.View, AACSBResource.ReportData)]
    [OpenApiOperation("Search courses from database.", "")]
    public Task<PaginationResponse<CourseDto>> SearchCourses(SearchCoursesRequest request)
    {
        return Mediator.Send(request);
    }

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

    [HttpGet("department")]
    [MustHavePermission(AACSBAction.View, AACSBResource.ReportData)]
    [OpenApiOperation("Get Departments", "")]
    public Task<List<DepartmentDto>> GetDepartments()
    {
        return Mediator.Send(new GetDepartmentsRequest());
    }
}