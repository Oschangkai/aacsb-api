namespace AACSB.WebApi.Application.ReportGenerator.Courses;

public class CopyDisciplineRequest : IRequest<MessageResponse>
{
    public int From { get; set; }
    public int To { get; set; }
}

public class CopyDisciplineRequestValidator : AbstractValidator<CopyDisciplineRequest>
{
    public CopyDisciplineRequestValidator()
    {
        RuleFor(x => x.From).NotEmpty();
        RuleFor(x => x.To).NotEmpty();
    }
}

public class CopyDisciplineRequestHandler : IRequestHandler<CopyDisciplineRequest, MessageResponse>
{
    private readonly IDapperRepository _repository;
    public CopyDisciplineRequestHandler(IDapperRepository repository) => _repository = repository;

    public async Task<MessageResponse> Handle(CopyDisciplineRequest request, CancellationToken cancellationToken)
    {
        string sql = $"UPDATE c1 SET DisciplineId = c2.DisciplineId " +
            $"FROM ReportGenerator.Courses c1 " +
            $"JOIN ReportGenerator.Courses c2 " +
                $"ON c2.Semester = @From AND c1.Code = c2.Code " +
            $"WHERE c1.Semester = @To";
        var sqlParams = new { request.From, request.To };

        int result = await _repository.UpdateAsync(sql, sqlParams, cancellationToken: cancellationToken);
        return new MessageResponse(true, $"Copy Discipline From {request.From} To {request.To} Success, {result} rows affected.");
    }
}