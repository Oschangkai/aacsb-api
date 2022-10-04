using System.ComponentModel.DataAnnotations;

namespace AACSB.WebApi.Domain.Catalog;

public class ImportSignature : AuditableEntity, IAggregateRoot
{
    [MaxLength(200)]
    public string Name { get; }

    public Guid CourseId { get; }
    public ICollection<Course> Courses { get; }
    public Guid TeacherId { get; }
    public ICollection<Teacher> Teachers { get; }

    public ImportSignature()
    {
        // Only needed for working with dapper (See GetProductViaDapperRequest)
        // If you're not using dapper it's better to remove this constructor.
    }

    public ImportSignature(string name)
    {
        Name = name;
    }
}