namespace AACSB.WebApi.Application.Common.Importer;

public interface IExcelReader : ITransientService
{
    List<T> ReadFromFilePath<T>(string file);
}