using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AACSB.WebApi.Domain.Catalog;

public class Discipline : AuditableEntity, IAggregateRoot
{
    [Precision(2, 0)]
    public decimal Code { get; private set; }
    [MaxLength(200)]
    public string Name { get; private set; }

    public ICollection<Course> Courses { get; private set; }

    public Discipline(decimal code, string name)
    {
        Code = code;
        Name = name;
    }
}