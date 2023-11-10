namespace AACSB.WebApi.Application.ReportGenerator.Teachers;

public class TeacherInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? WorkType { get; set; }
    public string? EnglishName { get; set; }
    public string? Degree { get; set; }
    public short? DegreeYear { get; set; }
    public string? Department { get; set; }
    public string? Email { get; set; }
    public string? EnglishNameInNtustCourse { get; set; }
    public string? NameInNtustCourse { get; set; }
    public string? Qualification { get; set; }
    public DateTime? ResignDate { get; set; }
    public string? Title { get; set; }
    public bool? Supervisor { get; set; }
    public Guid? LinkTo { get; set; }
}

public class ImportTeacherInfoRequest : IRequest<JobEnqueuedResponse>
{
   public List<TeacherInfo> TeacherInfos { get; set; }
}

public class ImportTeacherInfoRequestHandler : IRequestHandler<ImportTeacherInfoRequest, JobEnqueuedResponse>
{
    private readonly IJobService _jobService;
    private readonly ILogger<ImportTeacherInfoRequestHandler> _logger;
    public ImportTeacherInfoRequestHandler(IJobService jobService, ILogger<ImportTeacherInfoRequestHandler> logger)
        => (_jobService, _logger) = (jobService, logger);

    public async Task<JobEnqueuedResponse> Handle(ImportTeacherInfoRequest request, CancellationToken cancellationToken)
    {
        return new JobEnqueuedResponse(
            _jobService.Enqueue<IImportTeacherInfoJob>(x
                => x.ImportAsync(request.TeacherInfos, cancellationToken)));
    }
}