using AlvaCleanCRM.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AlvaCleanCRM.Controllers
{
    public class HomeController : Controller
    {

        public readonly IEmployeerRepository _employeerRepository;

        public HomeController(IEmployeerRepository employeerRepository)
        {
            _employeerRepository = employeerRepository;
        }

        public IActionResult LoginPage()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return View();
        }


        [HttpPost]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> LoginAction(string employeerLastName, string employeerPassword)
        {
            await _employeerRepository.LogIn(employeerLastName, employeerPassword);
            return RedirectToAction("HomePage");
        }

        public IActionResult HomePage()
        {
            return View();
        }




    }
}
