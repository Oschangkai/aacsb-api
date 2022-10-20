using AACSB.WebApi.Domain.ReportGenerator;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AACSB.WebApi.Infrastructure.Persistence.Configuration;

public class CourseConfig : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.IsMultiTenant();
        builder.ToTable("Courses", SchemaNames.ReportGenerator);
    }
}

public class TeacherConfig : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.IsMultiTenant();
        builder.ToTable("Teachers", SchemaNames.ReportGenerator);
    }
}

public class DisciplineConfig : IEntityTypeConfiguration<Discipline>
{
    public void Configure(EntityTypeBuilder<Discipline> builder)
    {
        builder.IsMultiTenant();
        builder.ToTable("Discipline", SchemaNames.ReportGenerator);
    }
}

public class ImportSignatureConfig : IEntityTypeConfiguration<ImportSignature>
{
    public void Configure(EntityTypeBuilder<ImportSignature> builder)
    {
        builder.IsMultiTenant();
        builder.ToTable("ImportSignatures", SchemaNames.ReportGenerator);
    }
}