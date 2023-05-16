using AACSB.WebApi.Application.ReportGenerator;
using AACSB.WebApi.Domain.ReportGenerator.Function;

namespace AACSB.WebApi.Host.Controllers.ReportGenerator;

public class ReportController : VersionedApiController
{
    [HttpPost("a31")]
    [MustHavePermission(AACSBAction.View, AACSBResource.Report)]
    [AllowAnonymous]
    [OpenApiOperation("Get AACSSB Table 3-1", "")]
    public Task<ICollection<TableA31>> Fetch(GetTableA31Request request)
    {
        return Mediator.Send(request);
    }
}