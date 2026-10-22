namespace HRDepartment.Models;

public class EducationRecord
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string Institution { get; set; } = "";
    public string Specialty { get; set; } = "";
    public string EducationLevel { get; set; } = "";
    public int GraduationYear { get; set; }
    public Employee? Employee { get; set; }
}
