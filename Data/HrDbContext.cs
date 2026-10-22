using HRDepartment.Models;
using Microsoft.EntityFrameworkCore;

namespace HRDepartment.Data;

public class HrDbContext(DbContextOptions<HrDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<EducationRecord> Educations => Set<EducationRecord>();
    public DbSet<PreviousJob> PreviousJobs => Set<PreviousJob>();
    public DbSet<Child> Children => Set<Child>();
    public DbSet<PersonnelOrder> PersonnelOrders => Set<PersonnelOrder>();
    public DbSet<AbsenceRecord> Absences => Set<AbsenceRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>().HasIndex(e => e.PersonnelNumber).IsUnique();
        modelBuilder.Entity<Employee>().HasMany(e => e.Educations).WithOne(e => e.Employee)
            .HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Employee>().HasMany(e => e.PreviousJobs).WithOne(e => e.Employee)
            .HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Employee>().HasMany(e => e.Children).WithOne(e => e.Employee)
            .HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Employee>().HasMany(e => e.Orders).WithOne(e => e.Employee)
            .HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Employee>().HasMany(e => e.Absences).WithOne(e => e.Employee)
            .HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Cascade);
    }
}
