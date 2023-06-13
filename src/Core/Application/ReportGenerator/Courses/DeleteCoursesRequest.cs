namespace AACSB.WebApi.Application.ReportGenerator.Courses;

public class DeleteCoursesRequest : IRequest<MessageResponse>
{
    public string Semester { get; set; }
    public string ImportSignatureId { get; set; }
}

public class DeleteCoursesRequestHandler : IRequestHandler<DeleteCoursesRequest, MessageResponse>
{
    private readonly IDapperRepository _repository;
    public DeleteCoursesRequestHandler(IDapperRepository repository) => _repository = repository;
    public async Task<MessageResponse> Handle(DeleteCoursesRequest request, CancellationToken cancellationToken)
    {
        string sql = "DELETE FROM [ReportGenerator].Courses WHERE [Semester]=@Semester AND [ImportSignatureId]=@ImportSignatureId";
        var sqlParams = new { request.Semester, request.ImportSignatureId };
        await _repository.QueryAsync(sql, sqlParams, cancellationToken: cancellationToken);

        return new MessageResponse(true, $"Deleted courses with semester={request.Semester} and id={request.ImportSignatureId}");
    }

}