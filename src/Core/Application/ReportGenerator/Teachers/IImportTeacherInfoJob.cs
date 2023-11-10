using System.ComponentModel;

namespace AACSB.WebApi.Application.ReportGenerator.Teachers;

public interface IImportTeacherInfoJob : IScopedService
{
    [DisplayName("Import Teachers from Json")]
    Task ImportAsync(List<TeacherInfo> teacherInfos, CancellationToken cancellationToken);
}