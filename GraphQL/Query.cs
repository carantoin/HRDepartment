using HRDepartment.Data;
using HRDepartment.Models;
using Microsoft.EntityFrameworkCore;

namespace HRDepartment.GraphQL;

public class Query
{
    // Все сотрудники и связанные сведения для личной карточки.
    public Task<List<Employee>> GetEmployeesAsync([Service] HrDbContext db) =>
        db.Employees.Include(e => e.Educations).Include(e => e.PreviousJobs)
            .Include(e => e.Children).Include(e => e.Orders).Include(e => e.Absences)
            .AsNoTracking().ToListAsync();

    public Task<Employee?> GetEmployeeAsync(int id, [Service] HrDbContext db) =>
        db.Employees.Include(e => e.Educations).Include(e => e.PreviousJobs)
            .Include(e => e.Children).Include(e => e.Orders).Include(e => e.Absences)
            .AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

    public Task<List<Employee>> GetEmployeesWithoutHigherEducationAsync([Service] HrDbContext db) =>
        db.Employees.Include(e => e.Educations)
            .Where(e => !e.Educations.Any(ed => ed.EducationLevel == "Высшее"))
            .AsNoTracking().ToListAsync();

    public Task<List<Employee>> GetEducationMismatchAsync([Service] HrDbContext db) =>
        db.Employees.Include(e => e.Educations)
            .Where(e => !e.Educations.Any(ed => ed.EducationLevel == e.RequiredEducationLevel))
            .AsNoTracking().ToListAsync();

    public Task<List<PersonnelOrder>> GetOrdersForEmployeeAsync(int employeeId, [Service] HrDbContext db) =>
        db.PersonnelOrders.Where(o => o.EmployeeId == employeeId)
            .OrderBy(o => o.OrderDate).AsNoTracking().ToListAsync();

    public async Task<List<EmployeeChildCount>> GetChildCountsAsync([Service] HrDbContext db) =>
        await db.Employees.Select(e => new EmployeeChildCount {
            EmployeeId = e.Id,
            EmployeeName = e.LastName + " " + e.FirstName + (e.MiddleName == null ? "" : " " + e.MiddleName),
            ChildrenCount = e.Children.Count
        }).AsNoTracking().ToListAsync();

    public Task<List<Child>> GetAllChildrenAsync([Service] HrDbContext db) =>
        db.Children.Include(c => c.Employee).AsNoTracking().ToListAsync();

    public Task<List<Employee>> GetEmployeesOutsideCityAsync(string city, [Service] HrDbContext db) =>
        db.Employees.Where(e => e.City.ToLower() != city.ToLower())
            .AsNoTracking().ToListAsync();

    public Task<List<Employee>> GetEmployeesByDepartmentAsync(string departmentName, [Service] HrDbContext db) =>
        db.Employees.Where(e => e.DepartmentName.ToLower() == departmentName.ToLower())
            .AsNoTracking().ToListAsync();

    // Возвращает отпуска/больничные, пересекающиеся с заданным периодом.
    public Task<List<AbsenceRecord>> GetEmployeesAbsentDuringAsync(
        DateTime from, DateTime to, [Service] HrDbContext db) =>
        db.Absences.Include(a => a.Employee)
            .Where(a => a.StartDate <= to && a.EndDate >= from)
            .OrderBy(a => a.StartDate).AsNoTracking().ToListAsync();
}

public class EmployeeChildCount
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = "";
    public int ChildrenCount { get; set; }
}
