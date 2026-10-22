namespace HRDepartment.Models;

public class Child
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string FullName { get; set; } = "";
    public DateTime BirthDate { get; set; }
    public Employee? Employee { get; set; }
}
