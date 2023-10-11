using System.Collections.Concurrent;
using AACSB.WebApi.Application.Common.Importer;
using AACSB.WebApi.Domain.ReportGenerator;
using ClosedXML.Excel;
using Microsoft.Extensions.Logging;

namespace AACSB.WebApi.Infrastructure.Common.Import;

public class ExcelReader : IExcelReader
{
    private readonly ILogger<ExcelReader> _logger;
    public ExcelReader(ILogger<ExcelReader> logger) => _logger = logger;

    public List<T> ReadFromFilePath<T>(string file)
    {
        var dataList = new List<T>();
        var type = typeof(T);

        // Open the Excel file and only read the first worksheet.
        using IXLWorkbook workbook = new XLWorkbook(file);
        var worksheet = workbook.Worksheets.First();

        // Get the first row to use as the header row.
        var properties = type.GetProperties();
        var columns = worksheet.FirstRow().Cells().Select((v, i) => new { v.Value, Index = i + 1 });

        // foreach rows, create an object and set the properties.
        foreach (var row in worksheet.RowsUsed().Skip(1))
        {
            var obj = (T)Activator.CreateInstance(type)!;

            // set the properties one by one.
            foreach (var prop in properties)
            {
                var column = columns.FirstOrDefault(c => c.Value.ToString() == prop.Name);
                if (column is null) continue;

                var value = row.Cell(column.Index).Value;
                if (value.Type == XLDataType.Blank) continue;

                if (IsSimpleType(prop.PropertyType)) // Skip columns with complex type
                {
                    try
                    {
                        prop.SetValue(obj, Convert.ChangeType(value.ToString()?.Trim(), Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType));
                    }
                    catch (Exception e)
                    {
                        _logger.LogCritical($"Inserting value '{value.ToString()?.Trim()}' on column '{prop.Name}' failed. {e.Message}");
                        throw;
                    }
                }
                else if (prop.PropertyType == typeof(Teacher)) // Customized for Research importer
                {
                    prop.SetValue(obj, new Teacher() { Name = value.ToString()?.Trim() ?? string.Empty });
                }
                else if (prop.PropertyType == typeof(ICollection<ResearchResearchType>)) // Customized for Research importer
                {
                    string? val = value.ToString()?.Trim();
                    if (string.IsNullOrEmpty(val)) continue;

                    prop.SetValue(
                        obj,
                        val.Split(",").Select(x => new ResearchResearchType() { ResearchType = new ResearchType() { Code = x }}).ToList());
                }
            }

            dataList.Add(obj);
        }

        return dataList;
    }

    // https://stackoverflow.com/a/15578098/7972084
    private static readonly ConcurrentDictionary<Type, bool> IsSimpleTypeCache = new ConcurrentDictionary<System.Type, bool>();
    private static bool IsSimpleType(Type type)
    {
        return IsSimpleTypeCache.GetOrAdd(type, t =>
            type.IsPrimitive ||
            type.IsEnum ||
            type == typeof(string) ||
            type == typeof(decimal) ||
            type == typeof(DateTime) ||
            type == typeof(DateOnly) ||
            type == typeof(TimeOnly) ||
            type == typeof(DateTimeOffset) ||
            type == typeof(TimeSpan) ||
            type == typeof(Guid) ||
            IsNullableSimpleType(type));

        static bool IsNullableSimpleType(Type t)
        {
            var underlyingType = Nullable.GetUnderlyingType(t);
            return underlyingType != null && IsSimpleType(underlyingType);
        }
    }
}