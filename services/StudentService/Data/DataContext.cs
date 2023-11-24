using MassTransit;
using Microsoft.EntityFrameworkCore;
using StudentService.Models;

namespace StudentService.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Grade> Grades { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.AddInboxStateEntity();
        builder.AddOutboxMessageEntity();
        builder.AddOutboxStateEntity();

        builder.Entity<Grade>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasMany(x => x.Classes)
                .WithOne(x => x.Grade)
                .HasForeignKey(x => x.GradeId);
        });

        builder.Entity<Department>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasMany(x => x.Classes)
                .WithOne(x => x.Department)
                .HasForeignKey(x => x.DepartmentId);
        });

        builder.Entity<Class>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasMany(x => x.Students)
                .WithOne(x => x.Class)
                .HasForeignKey(x => x.ClassId);

            e.HasOne(x => x.Grade)
                .WithMany(x => x.Classes)
                .HasForeignKey(x => x.GradeId);

            e.HasOne(x => x.Department)
                .WithMany(x => x.Classes)
                .HasForeignKey(x => x.DepartmentId);
        });
    }
}