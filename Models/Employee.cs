namespace HRDepartment.Models;

public class Employee
{
    public int Id { get; set; }
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
    // Example values: "Среднее", "Среднее профессиональное", "Высшее".
    public string RequiredEducationLevel { get; set; } = "Высшее";
    public DateTime HireDate { get; set; } = DateTime.Today;
    public List<EducationRecord> Educations { get; set; } = [];
    public List<PreviousJob> PreviousJobs { get; set; } = [];
    public List<Child> Children { get; set; } = [];
    public List<PersonnelOrder> Orders { get; set; } = [];
    public List<AbsenceRecord> Absences { get; set; } = [];
}
