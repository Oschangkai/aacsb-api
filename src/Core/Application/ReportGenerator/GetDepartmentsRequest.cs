using AACSB.WebApi.Domain.ReportGenerator;
using Mapster;

namespace AACSB.WebApi.Application.ReportGenerator;

public class GetDepartmentsRequest : IRequest<List<DepartmentDto>>
{
}

public class GetDepartmentsRequestHandler : IRequestHandler<GetDepartmentsRequest, List<DepartmentDto>>
{
    private readonly IRepository<Department> _repository;

    public GetDepartmentsRequestHandler(IRepository<Department> repository) =>
        _repository = repository;
    public async Task<List<DepartmentDto>> Handle(GetDepartmentsRequest request, CancellationToken cancellationToken)
    {
        return (await _repository.ListAsync(cancellationToken)).Adapt<List<DepartmentDto>>()
               ?? throw new NotFoundException("Department Not Found.");
    }
}