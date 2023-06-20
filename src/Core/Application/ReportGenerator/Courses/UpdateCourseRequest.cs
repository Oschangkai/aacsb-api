using AACSB.WebApi.Domain.ReportGenerator;

namespace AACSB.WebApi.Application.ReportGenerator.Courses;

public class UpdateCourseRequest : IRequest<MessageResponse>
{
    public Guid Id { get; set; }
    public decimal? Credit { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? DisciplineId { get; set; }
}

public class UpdateCourseRequestValidator : CustomValidator<UpdateCourseRequest>
{
    public UpdateCourseRequestValidator(IReadRepository<Course> repository)
      => RuleFor(c => c.Id)
            .NotEmpty()
            .MustAsync(async (id, ct) => await repository.GetByIdAsync(id, ct) is not null)
            .WithMessage((_, id) => $"Course {id} Not Found.");
}

public class UpdateCourseRequestHandler : IRequestHandler<UpdateCourseRequest, MessageResponse>
{
    private readonly IRepositoryWithEvents<Course> _repository;

    public UpdateCourseRequestHandler(IRepositoryWithEvents<Course> repository)
        => _repository = repository;

    public async Task<MessageResponse> Handle(UpdateCourseRequest request, CancellationToken cancellationToken)
    {
        var course = await _repository.GetByIdAsync(request.Id, cancellationToken);
        _ = course ?? throw new NotFoundException($"Course with id {request.Id} Not Found.");

        if (request.Credit is not null && request.Credit != course!.Credit)
            course.Credit = request.Credit.Value;
        if (request.DepartmentId is not null && request.DepartmentId != course!.DepartmentId)
            course.DepartmentId = request.DepartmentId.Value;
        if (request.DisciplineId != course.DisciplineId)
            course.DisciplineId = request.DisciplineId;

        await _repository.UpdateAsync(course!, cancellationToken);
        return new MessageResponse(true, $"Updated Course {course!.Code} {course.Name}");
    }

}