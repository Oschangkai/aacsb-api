using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AACSB.WebApi.Domain.Catalog;

public class Teacher : AuditableEntity, IAggregateRoot
{
    [MaxLength(80)]
    public string Name { get; private set; }
    [Comment("SA/IP/...，單一值")]
    [MaxLength(10)]
    public string Qualification { get; private set; }
    [Comment("IM/FM/...，單一值")]
    [MaxLength(10)]
    public string DepartmentAbbr { get; private set; }
    [MaxLength(15)]
    public string? Type { get; private set; }
    [Comment("P=Full Time, F=Part Time, C=Contractual")]
    [MaxLength(10)]
    public string? TypeAbbr { get; private set; }
    [MaxLength(200)]
    public string? EnglishName { get; private set; }
    [MaxLength(10)]
    public string? Degree { get; private set; }
    [Precision(4, 0)]
    public decimal? DegreeYear { get; private set; }
    [MaxLength(100)]
    public string? Department { get; private set; }
    [Comment("MT/RES/... 可以為多值，以逗點分割")]
    [MaxLength(100)]
    public string? Responsibility { get; private set; }

    public Guid ImportSignatureId { get; private set; }
    public ImportSignature ImportSignature { get; private set; }
    public ICollection<Course> Courses { get; private set; }

    public Teacher() {}
    public Teacher(string name, string qualification, string departmentAbbr, string? type, string? typeAbbr,
        string? englishName, string? degree, decimal? degreeYear, string? department,
        string? responsibility, Guid importSignatureId)
    {
        Name = name;
        Qualification = qualification;
        DepartmentAbbr = departmentAbbr;
        Type = type;
        TypeAbbr = typeAbbr;
        EnglishName = englishName;
        Degree = degree;
        DegreeYear = degreeYear;
        Department = department;
        Responsibility = responsibility;
        ImportSignatureId = importSignatureId;
    }
}