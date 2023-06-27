using AACSB.WebApi.Application.ReportGenerator;
using AACSB.WebApi.Application.ReportGenerator.Courses;
using AACSB.WebApi.Application.ReportGenerator.Reports;
using AACSB.WebApi.Application.ReportGenerator.Teachers;

namespace AACSB.WebApi.Host.Controllers.ReportGenerator;

public class ReportDataController : VersionedApiController
{
    [HttpPost("course/search")]
    [MustHavePermission(AACSBAction.View, AACSBResource.ReportData)]
    [OpenApiOperation("Search courses from database.", "")]
    public Task<PaginationResponse<CourseDto>> SearchCourses(SearchCoursesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("course/{id}")]
    [MustHavePermission(AACSBAction.View, AACSBResource.ReportData)]
    [OpenApiOperation("Get a Course By Id.", "")]
    public Task<CourseDetailDto> GetCourseDetailById(Guid id)
    {
        return Mediator.Send(new GetCourseDetailByIdRequest(id));
    }

    [HttpPost("course/collect")]
    [MustHavePermission(AACSBAction.Import, AACSBResource.ReportData)]
    [OpenApiOperation("Fetch courses from NTUST.", "")]
    public Task<JobEnqueuedResponse> CollectCourses(FetchCoursesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPost("course/inspect")]
    [MustHavePermission(AACSBAction.View, AACSBResource.ReportData)]
    [OpenApiOperation("Get Null Discipline Courses by Academic Year", "")]
    public Task<List<ACourseDto>> InspectCourses(GetNullDisciplineCoursesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPatch("course")]
    [MustHavePermission(AACSBAction.Update, AACSBResource.ReportData)]
    [OpenApiOperation("Update course information.", "")]
    public Task<MessageResponse> UpdateCourse(UpdateCourseRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpDelete("course")]
    [MustHavePermission(AACSBAction.Delete, AACSBResource.ReportData)]
    [OpenApiOperation("delete courses from database by semester.", "")]
    public Task<MessageResponse> DeleteCoursesBySemester(DeleteCoursesRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpDelete("course/{id}")]
    [MustHavePermission(AACSBAction.Delete, AACSBResource.ReportData)]
    [OpenApiOperation("mark deleted a course.", "")]
    public Task<MessageResponse> DeleteCourseById(Guid id)
    {
        return Mediator.Send(new DeleteCourseRequest(id));
    }

    [HttpGet("semester")]
    [MustHavePermission(AACSBAction.View, AACSBResource.ReportData)]
    [OpenApiOperation("Get semesters from database.", "")]
    public Task<int[]> GetSemesters()
    {
        return Mediator.Send(new GetSemesterRequest());
    }

    [HttpPost("teacher/search")]
    [MustHavePermission(AACSBAction.View, AACSBResource.ReportData)]
    [OpenApiOperation("Search teachers from database.", "")]
    public Task<PaginationResponse<TeacherDto>> SearchTeachers(SearchTeachersRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPatch("teacher")]
    [MustHavePermission(AACSBAction.Update, AACSBResource.ReportData)]
    [OpenApiOperation("Update teacher information.", "")]
    public Task<MessageResponse> UpdateTeacher(UpdateTeacherRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("teacher/{id}")]
    [MustHavePermission(AACSBAction.View, AACSBResource.ReportData)]
    [OpenApiOperation("Get Teacher Detail By Id.", "")]
    public Task<TeacherDetailDto> GetTeacherDetailById(Guid id)
    {
        return Mediator.Send(new GetTeacherDetailByIdRequest(id));
    }

    [HttpDelete("teacher/{id}")]
    [MustHavePermission(AACSBAction.Delete, AACSBResource.ReportData)]
    [OpenApiOperation("mark deleted a teacher.", "")]
    public Task<MessageResponse> DeleteTeacherById(Guid id)
    {
        return Mediator.Send(new DeleteTeacherRequest(id));
    }

    [HttpGet("discipline")]
    [MustHavePermission(AACSBAction.View, AACSBResource.ReportData)]
    [OpenApiOperation("Get Disciplines from database.", "")]
    public Task<List<DisciplineDto>> GetDisciplines()
    {
        return Mediator.Send(new GetDisciplinesRequest());
    }

    [HttpGet("academic-year")]
    [MustHavePermission(AACSBAction.View, AACSBResource.ReportData)]
    [OpenApiOperation("Get Academic Years", "")]
    public Task<int[]> GetAcademicYears()
    {
        return Mediator.Send(new GetTableA31AcademicYearRequest());
    }

    [HttpGet("department")]
    [MustHavePermission(AACSBAction.View, AACSBResource.ReportData)]
    [OpenApiOperation("Get Departments", "")]
    public Task<List<DepartmentDto>> GetDepartments()
    {
        return Mediator.Send(new GetDepartmentsRequest());
    }

    [HttpGet("qualification")]
    [MustHavePermission(AACSBAction.View, AACSBResource.ReportData)]
    [OpenApiOperation("Get Qualifications", "")]
    public Task<List<QualificationDto>> GetQualifications()
    {
        return Mediator.Send(new GetQualificationsRequest());
    }
}