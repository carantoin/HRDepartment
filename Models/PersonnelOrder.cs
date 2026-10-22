namespace HRDepartment.Models;

public class PersonnelOrder
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string OrderNumber { get; set; } = "";
    public DateTime OrderDate { get; set; }
    public string OrderType { get; set; } = ""; // Приём, перевод, увольнение и т.п.
    public string Description { get; set; } = "";
    public Employee? Employee { get; set; }
}
