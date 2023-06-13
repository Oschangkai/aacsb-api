using AACSB.WebApi.Domain.ReportGenerator.Function;

namespace AACSB.WebApi.Application.ReportGenerator.Reports;

public class GetTableA32Request : IRequest<ICollection<TableA32>>
{
    public string Semester { get; set; }
    public string Type { get; set; }
    public Guid? DepartmentId { get; set; }
}

public class GetTableA32RequestHandler : IRequestHandler<GetTableA32Request, ICollection<TableA32>>
{
    private readonly IDapperRepository _repository;
    public GetTableA32RequestHandler(IDapperRepository repository) => _repository = repository;
    public async Task<ICollection<TableA32>> Handle(GetTableA32Request request, CancellationToken cancellationToken)
    {
        string sql = "SELECT * FROM [ReportGenerator].[F_GetQualificationPercentage](@Semester, @Type, @DepartmentId)";
        object sqlParams = new { request.Semester, request.Type, request.DepartmentId };

        var tableA32 = await _repository.QueryAsync<TableA32>(sql, sqlParams, cancellationToken: cancellationToken);
        _ = tableA32 ?? throw new NotFoundException("TableA32 Not Found.");

        return tableA32.ToList();
    }
}