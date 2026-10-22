namespace HRDepartment.Models;

public class PreviousJob
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string Organization { get; set; } = "";
    public string Position { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Employee? Employee { get; set; }
}
