using AACSB.WebApi.Domain.ReportGenerator;
using Microsoft.EntityFrameworkCore;

namespace AACSB.WebApi.Infrastructure.Persistence.Context;

public partial class ApplicationDbContext
{
    // Report Generator
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<CourseTeacher> CourseTeachers => Set<CourseTeacher>();

    public DbSet<Qualification> Qualifications => Set<Qualification>();

    public DbSet<Professional> Professionals => Set<Professional>();
    public DbSet<Research> Research => Set<Research>();
    public DbSet<Responsibility> Responsibilities => Set<Responsibility>();
    public DbSet<Discipline> Disciplines => Set<Discipline>();
    public DbSet<ImportSignature> ImportSignatures => Set<ImportSignature>();
    public DbSet<Department> Departments => Set<Department>();
}