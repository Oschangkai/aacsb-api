using System.ComponentModel.DataAnnotations;
using AACSB.WebApi.Application.ReportGenerator.Reports;
using AACSB.WebApi.Domain.ReportGenerator.Function;

namespace AACSB.WebApi.Host.Controllers.ReportGenerator;

public class ReportController : VersionedApiController
{
    [HttpPost("a31")]
    [MustHavePermission(AACSBAction.View, AACSBResource.Report)]
    [OpenApiOperation("Get AACSSB Table 3-1", "")]
    public Task<ICollection<TableA31>> GetTableA31(GetTableA31Request request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("a31/{discipline}")]
    [MustHavePermission(AACSBAction.View, AACSBResource.Report)]
    [OpenApiOperation("Get AACSSB Table 3-1 By Discipline", "")]
    public Task<ICollection<TableA31>> GetTableA31ByDiscipline(GetTableA31Request request, [Required, FromRoute]string discipline)
    {
        var req = new GetTableA31ByDisciplineRequest()
        {
            Semester = request.Semester,
            Discipline = discipline
        };
        return Mediator.Send(req);
    }

    [HttpPost("a32")]
    [MustHavePermission(AACSBAction.View, AACSBResource.Report)]
    [OpenApiOperation("Get AACSSB Table 3-2", "")]
    public Task<ICollection<TableA32>> GetTableA32(GetTableA32Request request)
    {
        return Mediator.Send(request);
    }
}