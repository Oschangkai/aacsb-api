using AACSB.WebApi.Infrastructure.ReportGenerator.Request;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Infrastructure.Test.Common.Request;

public class CourseQueryRequestTest
{
    private readonly ITestOutputHelper _output;
    private readonly CourseQueryRequest _query;

    public CourseQueryRequestTest(ITestOutputHelper output)
    {
        _output = output;

        var logger = Mock.Of<ILogger<CourseQueryRequest>>();
        _query = new CourseQueryRequest(logger);
    }

    [Fact]
    public async void GetCourseByDepartment()
    {
        var result = await _query.GetCourseByDepartment("1091", "CS", CancellationToken.None);

        // _output.WriteLine(await result.ChCourses);
        // _output.WriteLine(await result.EnCourses);

        Assert.NotEmpty(await result.ChCourses);
        Assert.NotEmpty(await result.EnCourses);
    }
}