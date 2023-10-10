namespace AACSB.WebApi.Application.ReportGenerator.Research;

public class ImportResearchRequest : IRequest<JobEnqueuedResponse>
{
    public List<string> Files = new();
}

public class ImportResearchRequestHandler : IRequestHandler<ImportResearchRequest, JobEnqueuedResponse>
{
    private readonly IJobService _jobService;
    private readonly ILogger<ImportResearchRequestHandler> _logger;

    public ImportResearchRequestHandler(IJobService jobService, ILogger<ImportResearchRequestHandler> logger) => (_jobService, _logger) = (jobService, logger);

    public async Task<JobEnqueuedResponse> Handle(ImportResearchRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Importing {request.Files.Count} researches...");
        return await Task.FromResult(
            new JobEnqueuedResponse(
                _jobService.Enqueue<IImportResearchJob>(x
                    => x.ImportAsync(request.Files, default))));
    }
}