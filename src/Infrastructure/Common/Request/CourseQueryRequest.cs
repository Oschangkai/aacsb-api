using System.Net;
using AACSB.WebApi.Application.Common.Exceptions;
using AACSB.WebApi.Application.Common.Requests;
using Microsoft.Extensions.Logging;

namespace AACSB.WebApi.Infrastructure.Common.Request;

public class CourseQueryRequest : HttpRequest
{
    private ILogger<HttpRequest> Logger { get; }

    public CourseQueryRequest(ILogger<CourseQueryRequest> logger)
        : base(baseAddress: "https://querycourse.ntust.edu.tw/", null, null, logger: logger) => Logger = logger;

    public async Task<(Task<string> ChCourses, Task<string> EnCourses)> GetCourseByDepartment(string semester, string department, CancellationToken cancellationToken)
    {
        // make request query string
        const string path = "querycourse/api/courses";
        var queryEn = new Dictionary<string, string>
        {
            { "CourseNo", department },
            { "Semester", semester },
            { "Language", "en" }
        };
        var queryCh = new Dictionary<string, string>(queryEn)
        {
            ["Language"] = "zh"
        };

        // start request
        var queryEnTask = PostAsync(path, queryEn, null, cancellationToken);
        var queryChTask = PostAsync(path, queryCh, null, cancellationToken);
        await Task.WhenAll(queryEnTask, queryChTask);

        if ((await queryEnTask).StatusCode is not HttpStatusCode.OK || (await queryChTask).StatusCode is not HttpStatusCode.OK)
        {
            throw new ForeignResourceErrorException("Please Try Again");
        }

        var courseEn = (await queryEnTask).Content.ReadAsStringAsync(cancellationToken);
        var courseCh = (await queryEnTask).Content.ReadAsStringAsync(cancellationToken);
        return (ChCourses: courseCh, EnCourses: courseEn);
    }
}