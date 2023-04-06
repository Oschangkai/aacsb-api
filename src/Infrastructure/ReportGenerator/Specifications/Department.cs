using AACSB.WebApi.Domain.ReportGenerator;
using Ardalis.Specification;

namespace AACSB.WebApi.Infrastructure.ReportGenerator.Specifications;

public class DepartmentAbbrListSpec : Specification<Department, string>
{
    public DepartmentAbbrListSpec() =>
        Query.Select(x => x.Abbreviation).Where(d => !string.IsNullOrEmpty(d.Abbreviation) && d.Abbreviation != "OTHERS");
}

public class GetDepartmentByAbbrSpec : Specification<Department, Department>
{
    public GetDepartmentByAbbrSpec(string abbr) =>
        Query.Where(d => d.Abbreviation == abbr);
}