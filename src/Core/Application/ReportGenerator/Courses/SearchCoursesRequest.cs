using AACSB.WebApi.Domain.ReportGenerator;
namespace AACSB.WebApi.Application.ReportGenerator.Courses;

public class SearchCoursesRequest : PaginationFilter, IRequest<PaginationResponse<CourseDto>>
{
}

public class CoursesBySearchRequestSpec : EntitiesByPaginationFilterSpec<Course, CourseDto>
{
    public CoursesBySearchRequestSpec(SearchCoursesRequest request)
        : base(request) =>
        Query.OrderBy(c => c.Name, !request.HasOrderBy());
}

public class SearchCoursesRequestHandler : IRequestHandler<SearchCoursesRequest, PaginationResponse<CourseDto>>
{
    private readonly IReadRepository<Course> _repository;

    public SearchCoursesRequestHandler(IReadRepository<Course> repository) => _repository = repository;

    public async Task<PaginationResponse<CourseDto>> Handle(SearchCoursesRequest request, CancellationToken cancellationToken)
    {
        var spec = new CoursesBySearchRequestSpec(request);
        return await _repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken);
    }
}