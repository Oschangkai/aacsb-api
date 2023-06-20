using AACSB.WebApi.Domain.ReportGenerator;
using Mapster;

namespace AACSB.WebApi.Application.ReportGenerator;

public class GetQualificationsRequest : IRequest<List<QualificationDto>>
{
}

public class GetQualificationsRequestHandler : IRequestHandler<GetQualificationsRequest, List<QualificationDto>>
{
    private readonly IRepository<Qualification> _repository;

    public GetQualificationsRequestHandler(IRepository<Qualification> repository) =>
        _repository = repository;
    public async Task<List<QualificationDto>> Handle(GetQualificationsRequest request, CancellationToken cancellationToken)
    {
        return (await _repository.ListAsync(cancellationToken)).Adapt<List<QualificationDto>>()
               ?? throw new NotFoundException("Qualification Not Found.");
    }
}