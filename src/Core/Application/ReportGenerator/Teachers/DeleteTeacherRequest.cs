using AACSB.WebApi.Domain.ReportGenerator;

namespace AACSB.WebApi.Application.ReportGenerator.Teachers;

public class DeleteTeacherRequest : IRequest<MessageResponse>
{
    public Guid Id { get; set; }

    public DeleteTeacherRequest(Guid id) => Id = id;
}

public class DeleteTeacherRequestHandler : IRequestHandler<DeleteTeacherRequest, MessageResponse>
{
    // Add Domain Events automatically by using IRepositoryWithEvents
    private readonly IRepositoryWithEvents<Teacher> _teacherRepo;

    public DeleteTeacherRequestHandler(IRepositoryWithEvents<Teacher> teacherRepo) =>
        _teacherRepo = teacherRepo;

    public async Task<MessageResponse> Handle(DeleteTeacherRequest request, CancellationToken cancellationToken)
    {
        var teacher = await _teacherRepo.GetByIdAsync(request.Id, cancellationToken);

        _ = teacher ?? throw new NotFoundException($"Teacher with id {request.Id} Not Found.");

        await _teacherRepo.DeleteAsync(teacher, cancellationToken);
        return new MessageResponse(true, $"Deleted Teacher {teacher.Name}");
    }
}