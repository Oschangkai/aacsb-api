namespace AACSB.WebApi.Application.ReportGenerator;

public class FetchCoursesRequest : IRequest<JobEnqueuedResponse>
{
    public int Year { get; set; }
    public int Semester { get; set; }
    public string[]? Department { get; set; }
}

public class FetchCoursesRequestHandler : IRequestHandler<FetchCoursesRequest, JobEnqueuedResponse>
{
    private readonly IJobService _jobService;

    public FetchCoursesRequestHandler(IJobService jobService) => _jobService = jobService;

    public Task<JobEnqueuedResponse> Handle(FetchCoursesRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(
            new JobEnqueuedResponse(
                _jobService.Enqueue<IFetchCourseJob>(x
                    => x.FetchAsync(request.Year, request.Semester, request.Department ?? Array.Empty<string>(), default))));
    }

}
