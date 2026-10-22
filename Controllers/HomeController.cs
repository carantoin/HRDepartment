using Microsoft.AspNetCore.Mvc;

namespace HRDepartment.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult Error() => View();
}
