using AlvaCleanCRM.Models.RegisterModels;
using AlvaCleanCRM.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlvaCleanCRM.Controllers
{
    public class AdminController : Controller
    {

        private readonly IEmployeerRepository _employeerRepository;

        public AdminController(IEmployeerRepository employeerRepository) 
        {
            _employeerRepository = employeerRepository;        
        }


        public async Task<IActionResult> EmployeersPage()
        {
            var listOfEmployeers = await _employeerRepository.GetAllEmployeers();
            return View(listOfEmployeers);
        }

        public async Task<IActionResult> EmployeerOrdersPage(string Id)
        {
            return View("EmployeerOrdersPage", Id);
        }

        public IActionResult AddNewEmployeerPage() 
        { 
            return View();
        }


        public async Task<IActionResult> AddNewEmployeer(RegisterEmployeerModel model)
        {
            await _employeerRepository.AddNewEmployeer(model);
            return RedirectToAction("EmployeersPage");
        }
    }
}
