namespace AACSB.WebApi.Application.Common.Models;

public class JobEnqueuedResponse
{
    public string JobId { get; set; }

    public string JobUrl => "/jobs/jobs/details/" + JobId;

    public JobEnqueuedResponse(string jobId)
    {
        JobId = jobId;
    }
}