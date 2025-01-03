using Microsoft.AspNetCore.Mvc;

namespace AlvaCleanCRM.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult EmployeersPage()
        {
            return View();
        }
    }
}
