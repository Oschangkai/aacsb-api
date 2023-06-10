using AACSB.WebApi.Domain.ReportGenerator.Function;

namespace AACSB.WebApi.Application.ReportGenerator;

public class GetTableA32Request : IRequest<ICollection<TableA32>>
{
    public string Semester { get; set; }
    public string Type { get; set; }
}

public class GetTableA32RequestHandler : IRequestHandler<GetTableA32Request, ICollection<TableA32>>
{
    private readonly IDapperRepository _repository;
    public GetTableA32RequestHandler(IDapperRepository repository) => _repository = repository;
    public async Task<ICollection<TableA32>> Handle(GetTableA32Request request, CancellationToken cancellationToken)
    {
        var tableA32 = await _repository.QueryAsync<TableA32>(
            $"SELECT * FROM [ReportGenerator].[F_GetQualificationPercentage]('{request.Semester}', '{request.Type}')", cancellationToken: cancellationToken);
        _ = tableA32 ?? throw new NotFoundException("TableA32 Not Found.");

        return tableA32.ToList();
    }

}