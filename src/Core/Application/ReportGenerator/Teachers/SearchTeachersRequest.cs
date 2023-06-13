using AACSB.WebApi.Domain.ReportGenerator;

namespace AACSB.WebApi.Application.ReportGenerator.Teachers;

public class SearchTeachersRequest : PaginationFilter, IRequest<PaginationResponse<TeacherDto>>
{
}

public class TeachersBySearchRequestSpec : EntitiesByPaginationFilterSpec<Teacher, TeacherDto>
{
    public TeachersBySearchRequestSpec(SearchTeachersRequest request)
        : base(request) =>
        Query.OrderBy(c => c.Name, !request.HasOrderBy());
}

public class SearchTeachersRequestHandler : IRequestHandler<SearchTeachersRequest, PaginationResponse<TeacherDto>>
{
    private readonly IReadRepository<Teacher> _repository;

    public SearchTeachersRequestHandler(IReadRepository<Teacher> repository) => _repository = repository;

    public async Task<PaginationResponse<TeacherDto>> Handle(SearchTeachersRequest request, CancellationToken cancellationToken)
    {
        var spec = new TeachersBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}