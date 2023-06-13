using AACSB.WebApi.Domain.ReportGenerator;

namespace AACSB.WebApi.Application.ReportGenerator.Courses;

public class DeleteCourseRequest : IRequest<MessageResponse>
{
    public Guid Id { get; set; }

    public DeleteCourseRequest(Guid id) => Id = id;
}

public class DeleteCourseRequestHandler : IRequestHandler<DeleteCourseRequest, MessageResponse>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Course> _courseRepo;

    public DeleteCourseRequestHandler(IRepositoryWithEvents<Course> courseRepo) =>
        _courseRepo = courseRepo;

    public async Task<MessageResponse> Handle(DeleteCourseRequest request, CancellationToken cancellationToken)
    {
        var course = await _courseRepo.GetByIdAsync(request.Id, cancellationToken);

        _ = course ?? throw new NotFoundException($"Course with id {request.Id} Not Found.");

        await _courseRepo.DeleteAsync(course, cancellationToken);
        return new MessageResponse(true, $"Deleted course {course.Code} {course.Name}");
    }
}