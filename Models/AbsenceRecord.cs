namespace HRDepartment.Models;

public class AbsenceRecord
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string Type { get; set; } = ""; // "Отпуск" или "Больничный"
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? DocumentNumber { get; set; }
    public Employee? Employee { get; set; }
}
