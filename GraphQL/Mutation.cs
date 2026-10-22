using HRDepartment.Data;
using HRDepartment.Models;
using Microsoft.EntityFrameworkCore;

namespace HRDepartment.GraphQL;

public class Mutation
{
    public async Task<Employee> AddEmployeeAsync(EmployeeInput input, [Service] HrDbContext db)
    {
        if (string.IsNullOrWhiteSpace(input.PersonnelNumber))
            throw new GraphQLException("Табельный номер обязателен.");
        if (await db.Employees.AnyAsync(e => e.PersonnelNumber == input.PersonnelNumber))
            throw new GraphQLException("Сотрудник с таким табельным номером уже существует.");

        var employee = new Employee {
            PersonnelNumber = input.PersonnelNumber.Trim(),
            LastName = input.LastName.Trim(),
            FirstName = input.FirstName.Trim(),
            MiddleName = input.MiddleName,
            BirthDate = input.BirthDate,
            Gender = input.Gender,
            MaritalStatus = input.MaritalStatus,
            Phone = input.Phone,
            Email = input.Email,
            City = input.City,
            Address = input.Address,
            DepartmentName = input.DepartmentName,
            PositionTitle = input.PositionTitle,
            RequiredEducationLevel = input.RequiredEducationLevel,
            HireDate = input.HireDate
        };
        db.Employees.Add(employee);
        await db.SaveChangesAsync();
        return employee;
    }

    public async Task<bool> DeleteEmployeeAsync(int id, [Service] HrDbContext db)
    {
        var employee = await db.Employees.FindAsync(id);
        if (employee is null) return false;
        db.Employees.Remove(employee);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<PersonnelOrder> AddPersonnelOrderAsync(PersonnelOrderInput input, [Service] HrDbContext db)
    {
        if (!await db.Employees.AnyAsync(e => e.Id == input.EmployeeId))
            throw new GraphQLException("Сотрудник не найден.");

        var order = new PersonnelOrder {
            EmployeeId = input.EmployeeId,
            OrderNumber = input.OrderNumber,
            OrderDate = input.OrderDate,
            OrderType = input.OrderType,
            Description = input.Description
        };
        db.PersonnelOrders.Add(order);
        await db.SaveChangesAsync();
        return order;
    }

    public async Task<AbsenceRecord> AddAbsenceAsync(AbsenceInput input, [Service] HrDbContext db)
    {
        if (input.EndDate < input.StartDate)
            throw new GraphQLException("Дата окончания не может быть раньше даты начала.");
        if (!await db.Employees.AnyAsync(e => e.Id == input.EmployeeId))
            throw new GraphQLException("Сотрудник не найден.");

        var absence = new AbsenceRecord {
            EmployeeId = input.EmployeeId,
            Type = input.Type,
            StartDate = input.StartDate,
            EndDate = input.EndDate,
            DocumentNumber = input.DocumentNumber
        };
        db.Absences.Add(absence);
        await db.SaveChangesAsync();
        return absence;
    }
}

public class EmployeeInput
{
    public string PersonnelNumber { get; set; } = "";
    public string LastName { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string? MiddleName { get; set; }
    public DateTime BirthDate { get; set; }
    public string Gender { get; set; } = "";
    public string MaritalStatus { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Email { get; set; } = "";
    public string City { get; set; } = "";
    public string Address { get; set; } = "";
    public string DepartmentName { get; set; } = "";
    public string PositionTitle { get; set; } = "";
    public string RequiredEducationLevel { get; set; } = "Высшее";
    public DateTime HireDate { get; set; } = DateTime.Today;
}

public class PersonnelOrderInput
{
    public int EmployeeId { get; set; }
    public string OrderNumber { get; set; } = "";
    public DateTime OrderDate { get; set; }
    public string OrderType { get; set; } = "";
    public string Description { get; set; } = "";
}

public class AbsenceInput
{
    public int EmployeeId { get; set; }
    public string Type { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? DocumentNumber { get; set; }
}
