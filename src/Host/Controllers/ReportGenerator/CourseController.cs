using AACSB.WebApi.Application.ReportGenerator;

namespace AACSB.WebApi.Host.Controllers.ReportGenerator;

public class CourseController : VersionedApiController
{
    [HttpPost("fetch")]
    // [MustHavePermission(AACSBAction.Generate, AACSBResource.Courses)]
    [AllowAnonymous]
    [OpenApiOperation("Fetch courses from NTUST.", "")]
    public Task<JobEnqueuedResponse> Fetch(FetchCoursesRequest request)
    {
        return Mediator.Send(request);
    }
}