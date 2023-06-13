using AACSB.WebApi.Domain.ReportGenerator;

namespace AACSB.WebApi.Application.ReportGenerator.Teachers;

public class UpdateTeacherRequest : IRequest<MessageResponse>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? WorkTypeAbbr { get; set; }
    public string? EnglishName { get; set; }
    public string? Degree { get; set; }
    public decimal? DegreeYear { get; set; }
    public Guid? DepartmentId { get; set; }
    public string? Email { get; set; }
    public Guid? QualificationId { get; set; }
    public DateTime? ResignDate { get; set; }
    public string? Title { get; set; }
    public string? Responsibilities { get; set; }
}

public class UpdateTeacherRequestValidator : CustomValidator<UpdateTeacherRequest>
{
    public UpdateTeacherRequestValidator(IReadRepository<Teacher> repository)
      => RuleFor(c => c.Id)
            .NotEmpty()
            .MustAsync(async (id, ct) => await repository.GetByIdAsync(id, ct) is not null)
            .WithMessage((_, id) => $"Teacher {id} Not Found.");
}

public class UpdateTeacherRequestHandler : IRequestHandler<UpdateTeacherRequest, MessageResponse>
{
    private readonly IRepositoryWithEvents<Teacher> _repository;

    public UpdateTeacherRequestHandler(IRepositoryWithEvents<Teacher> repository)
        => _repository = repository;

    public async Task<MessageResponse> Handle(UpdateTeacherRequest request, CancellationToken cancellationToken)
    {
        var teacher = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (request.Name is not null && request.Name != teacher!.Name)
            teacher.Name = request.Name;
        if (request.DepartmentId is not null && request.DepartmentId != teacher!.DepartmentId!)
            teacher.DepartmentId = request.DepartmentId;
        if (request.WorkTypeAbbr is not null && request.WorkTypeAbbr != teacher!.WorkTypeAbbr)
            teacher.WorkTypeAbbr = request.WorkTypeAbbr;
        if (request.EnglishName is not null && request.EnglishName != teacher!.EnglishName)
            teacher.EnglishName = request.EnglishName;
        if (request.Degree is not null && request.Degree != teacher!.Degree)
            teacher.Degree = request.Degree;
        if (request.DegreeYear is not null && request.DegreeYear != teacher!.DegreeYear)
            teacher.DegreeYear = request.DegreeYear;
        if (request.Email is not null && request.Email != teacher!.Email)
            teacher.Email = request.Email;
        if (request.QualificationId is not null && request.QualificationId != teacher!.QualificationId)
            teacher.QualificationId = request.QualificationId;
        if (request.ResignDate is not null && request.ResignDate != teacher!.ResignDate)
            teacher.ResignDate = request.ResignDate;
        if (request.Title is not null && request.Title != teacher!.Title)
            teacher.Title = request.Title;
        if (request.Responsibilities is not null && request.Responsibilities != teacher!.Responsibilities)
            teacher.Responsibilities = request.Responsibilities;

        await _repository.UpdateAsync(teacher!, cancellationToken);
        return new MessageResponse(true, $"Updated Teacher {teacher!.Name}");
    }
}