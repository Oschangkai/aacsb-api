using AACSB.WebApi.Domain.ReportGenerator;
using AACSB.WebApi.Domain.ReportGenerator.Function;
using AACSB.WebApi.Domain.ReportGenerator.View;
using AACSB.WebApi.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AACSB.WebApi.Infrastructure.Persistence.Context;

public partial class ApplicationDbContext
{
    // Report Generator
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<CourseTeacher> CourseTeachers => Set<CourseTeacher>();

    public DbSet<Department> Departments => Set<Department>();

    public DbSet<ImportSignature> ImportSignatures => Set<ImportSignature>();

    // Course Related
    public DbSet<Discipline> Disciplines => Set<Discipline>();

    // Teacher Related
    public DbSet<Qualification> Qualifications => Set<Qualification>();

    public DbSet<Professional> Professionals => Set<Professional>();

    public DbSet<Research> Research => Set<Research>();

    public DbSet<Responsibility> Responsibilities => Set<Responsibility>();

    // Views
    public DbSet<TableA31Course> TableA31Course => Set<TableA31Course>();
}