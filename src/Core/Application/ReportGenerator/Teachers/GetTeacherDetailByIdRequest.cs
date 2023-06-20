using AACSB.WebApi.Domain.ReportGenerator;
using Mapster;

namespace AACSB.WebApi.Application.ReportGenerator.Teachers;

public class GetTeacherDetailByIdRequest : IRequest<TeacherDetailDto>
{
    public Guid Id { get; set; }
    public GetTeacherDetailByIdRequest(Guid id) => Id = id;
}

public class GetTeacherDetailByIdRequestValidator : CustomValidator<GetTeacherDetailByIdRequest>
{
    public GetTeacherDetailByIdRequestValidator(IReadRepository<Teacher> repository)
        => RuleFor(c => c.Id)
            .NotEmpty()
            .MustAsync(async (id, ct) => await repository.GetByIdAsync(id, ct) is not null)
            .WithMessage((_, id) => $"Teacher {id} Not Found.");
}

public class GetTeacherDetailByIdRequestHandler : IRequestHandler<GetTeacherDetailByIdRequest, TeacherDetailDto>
{
    private readonly IReadRepository<Teacher> _repository;

    public GetTeacherDetailByIdRequestHandler(IReadRepository<Teacher> repository)
        => _repository = repository;

    public async Task<TeacherDetailDto> Handle(GetTeacherDetailByIdRequest request, CancellationToken cancellationToken)
    {
        return (await _repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException($"Teacher with id {request.Id} Not Found."))
            .Adapt<TeacherDetailDto>();
    }
}