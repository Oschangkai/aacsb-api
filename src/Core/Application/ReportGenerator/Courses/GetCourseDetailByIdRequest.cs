using AACSB.WebApi.Domain.ReportGenerator;
using Mapster;

namespace AACSB.WebApi.Application.ReportGenerator.Courses;

public class GetCourseDetailByIdRequest : IRequest<CourseDetailDto>
{
    public Guid Id { get; set; }
    public GetCourseDetailByIdRequest(Guid id) => Id = id;
}

public class GetCourseDetailByIdRequestValidator : CustomValidator<GetCourseDetailByIdRequest>
{
    public GetCourseDetailByIdRequestValidator(IReadRepository<Course> repository)
        => RuleFor(c => c.Id)
            .NotEmpty()
            .MustAsync(async (id, ct) => await repository.GetByIdAsync(id, ct) is not null)
            .WithMessage((_, id) => $"Course {id} Not Found.");
}

public class GetCourseDetailByIdRequestHandler : IRequestHandler<GetCourseDetailByIdRequest, CourseDetailDto>
{
    private readonly IReadRepository<Course> _repository;
    public GetCourseDetailByIdRequestHandler(IReadRepository<Course> repository) =>
        _repository = repository;

    public async Task<CourseDetailDto> Handle(GetCourseDetailByIdRequest request, CancellationToken cancellationToken)
    {
        return (await _repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException($"Course with id {request.Id} Not Found."))
            .Adapt<CourseDetailDto>();
    }
}