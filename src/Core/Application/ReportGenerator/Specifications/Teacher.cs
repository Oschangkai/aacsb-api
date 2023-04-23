using AACSB.WebApi.Domain.ReportGenerator;
using Ardalis.Specification;

namespace AACSB.WebApi.Infrastructure.ReportGenerator.Specifications;

public class GetTeacherByChineseNameSpec : Specification<Teacher, Teacher>
{
    public GetTeacherByChineseNameSpec(string name) =>
        Query.Where(t => t.Name == name);
}

public class GetTeacherByNameSpec : Specification<Teacher, Teacher>
{
    public GetTeacherByNameSpec(string name, string englishName, Guid importId) =>
        Query.Where(t => t.Name == name && t.EnglishNameInNtustCourse == englishName && t.ImportSignatureId == importId);
}