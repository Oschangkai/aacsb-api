using System.ComponentModel;

namespace AACSB.WebApi.Application.ReportGenerator.Research;

public interface IImportResearchJob : IScopedService
{
    [DisplayName("Import Research from Excel")]
    Task ImportAsync(List<string> files, CancellationToken cancellationToken);
}