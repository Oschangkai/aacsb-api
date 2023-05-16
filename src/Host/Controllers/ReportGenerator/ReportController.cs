using AACSB.WebApi.Application.ReportGenerator;
using AACSB.WebApi.Domain.ReportGenerator.Function;

namespace AACSB.WebApi.Host.Controllers.ReportGenerator;

public class ReportController : VersionedApiController
{
    [HttpPost("a31")]
    // [MustHavePermission(AACSBAction.Generate, AACSBResource.Courses)]
    [AllowAnonymous]
    [OpenApiOperation("Fetch courses from NTUST.", "")]
    public Task<ICollection<TableA31>> Fetch(GetTableA31Request request)
    {
        return Mediator.Send(request);
    }
}