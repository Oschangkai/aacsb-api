using AACSB.WebApi.Domain.ReportGenerator;
using Ardalis.Specification;

namespace AACSB.WebApi.Infrastructure.ReportGenerator.Specifications;

public class GetTeacherByChineseNameSpec : Specification<Teacher, Teacher>
{
    public GetTeacherByChineseNameSpec(string name) =>
        Query.Where(t => t.Name == name);
}