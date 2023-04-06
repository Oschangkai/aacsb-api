using AACSB.WebApi.Domain.ReportGenerator;
using Microsoft.EntityFrameworkCore;

namespace AACSB.WebApi.Infrastructure.Persistence.Context;

public partial class ApplicationDbContext
{
    // Report Generator
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Teacher> Teachers => Set<Teacher>();

    public DbSet<TeacherQualification> TeacherQualifications => Set<TeacherQualification>();
    public DbSet<Qualification> Qualifications => Set<Qualification>();
    public DbSet<TeacherProfessional> TeacherProfessionals => Set<TeacherProfessional>();

    public DbSet<Professional> Professionals => Set<Professional>();
    public DbSet<TeacherResearch> TeacherResearch => Set<TeacherResearch>();
    public DbSet<Research> Research => Set<Research>();
    public DbSet<TeacherResponsibility> TeacherResponsibilities => Set<TeacherResponsibility>();
    public DbSet<Responsibility> Responsibilities => Set<Responsibility>();
    public DbSet<Discipline> Disciplines => Set<Discipline>();
    public DbSet<ImportSignature> ImportSignatures => Set<ImportSignature>();
    public DbSet<Department> Departments => Set<Department>();
}