using AACSB.WebApi.Domain.ReportGenerator;
using Mapster;

namespace AACSB.WebApi.Application.ReportGenerator;

public class GetDisciplinesRequest : IRequest<List<DisciplineDto>>
{
}

public class GetDisciplinesRequestHandler : IRequestHandler<GetDisciplinesRequest, List<DisciplineDto>>
{
    private readonly IRepository<Discipline> _repository;

    public GetDisciplinesRequestHandler(IRepository<Discipline> repository) =>
        _repository = repository;
    public async Task<List<DisciplineDto>> Handle(GetDisciplinesRequest request, CancellationToken cancellationToken)
    {
       return (await _repository.ListAsync(cancellationToken)).Adapt<List<DisciplineDto>>()
              ?? throw new NotFoundException("Discipline Not Found.");
    }
}
