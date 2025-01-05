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
    }
}
