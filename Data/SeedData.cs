using HRDepartment.Models;
using Microsoft.EntityFrameworkCore;

namespace HRDepartment.Data;

public static class SeedData
{
    public static async Task InitializeAsync(HrDbContext db)
    {
        if (await db.Employees.AnyAsync()) return;

        var e1 = new Employee {
            PersonnelNumber = "HR-001", LastName = "Иванов", FirstName = "Иван",
            MiddleName = "Петрович", BirthDate = new DateTime(1988, 4, 12),
            Gender = "Мужской", MaritalStatus = "Женат", Phone = "+7 900 000-00-01",
            Email = "ivanov@example.com", City = "Москва", Address = "ул. Лесная, 10",
            DepartmentName = "ИТ", PositionTitle = "Разработчик", RequiredEducationLevel = "Высшее",
            HireDate = new DateTime(2020, 2, 1),
            Educations = [new EducationRecord { Institution = "МГТУ", Specialty = "Информатика", EducationLevel = "Высшее", GraduationYear = 2010 }],
            PreviousJobs = [new PreviousJob { Organization = "ООО Альфа", Position = "Стажёр", StartDate = new DateTime(2010, 7, 1), EndDate = new DateTime(2012, 8, 31) }],
            Children = [new Child { FullName = "Иван Иванов", BirthDate = new DateTime(2015, 5, 20) }],
            Orders = [new PersonnelOrder { OrderNumber = "К-15", OrderDate = new DateTime(2020, 2, 1), OrderType = "Приём", Description = "Приём на должность разработчика" }],
            Absences = [new AbsenceRecord { Type = "Отпуск", StartDate = new DateTime(2025, 7, 1), EndDate = new DateTime(2025, 7, 14), DocumentNumber = "ОТ-12" }]
        };
        var e2 = new Employee {
            PersonnelNumber = "HR-002", LastName = "Смирнова", FirstName = "Анна",
            MiddleName = "Сергеевна", BirthDate = new DateTime(1994, 9, 3),
            Gender = "Женский", MaritalStatus = "Не замужем", Phone = "+7 900 000-00-02",
            Email = "smirnova@example.com", City = "Тверь", Address = "ул. Центральная, 5",
            DepartmentName = "Бухгалтерия", PositionTitle = "Бухгалтер", RequiredEducationLevel = "Высшее",
            HireDate = new DateTime(2022, 3, 10),
            Educations = [new EducationRecord { Institution = "Колледж экономики", Specialty = "Бухгалтерский учёт", EducationLevel = "Среднее профессиональное", GraduationYear = 2015 }],
            Orders = [new PersonnelOrder { OrderNumber = "К-28", OrderDate = new DateTime(2022, 3, 10), OrderType = "Приём", Description = "Приём на должность бухгалтера" }],
            Absences = [new AbsenceRecord { Type = "Больничный", StartDate = new DateTime(2025, 2, 3), EndDate = new DateTime(2025, 2, 8), DocumentNumber = "БЛ-77" }]
        };

        db.Employees.AddRange(e1, e2);
        await db.SaveChangesAsync();
    }
}
