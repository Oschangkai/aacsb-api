namespace AACSB.WebApi.Application.ReportGenerator.Courses;

public class CourseDiscipline
{
    public string Code { get; set; }
    public int DisciplineCode { get; set; }
    public string DisciplineName { get; set; }
}

public class ImportCourseDisciplineRequest : IRequest<JobEnqueuedResponse>
{
   public List<CourseDiscipline> CourseDisciplines { get; set; }
}

public class ImportCourseDisciplineRequestHandler : IRequestHandler<ImportCourseDisciplineRequest, JobEnqueuedResponse>
{
    private readonly IJobService _jobService;
    private readonly ILogger<ImportCourseDisciplineRequestHandler> _logger;
    public ImportCourseDisciplineRequestHandler(IJobService jobService, ILogger<ImportCourseDisciplineRequestHandler> logger)
        => (_jobService, _logger) = (jobService, logger);

    public async Task<JobEnqueuedResponse> Handle(ImportCourseDisciplineRequest request, CancellationToken cancellationToken)
    {
        return new JobEnqueuedResponse(
            _jobService.Enqueue<IImportCourseDisciplineJob>(x
                => x.ImportAsync(request.CourseDisciplines, cancellationToken)));
    }
}