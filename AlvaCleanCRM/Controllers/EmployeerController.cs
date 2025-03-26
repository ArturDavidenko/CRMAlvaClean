using AlvaCleanCRM.Models.ViewModels;
using AlvaCleanCRM.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlvaCleanCRM.Controllers
{
    public class EmployeerController : Controller
    {
        private readonly IEmployeerRepository _employeerRepository;

        public EmployeerController(IEmployeerRepository employeerRepository)
        {
            _employeerRepository = employeerRepository;
        }

        public async Task<IActionResult> AllOrdersPageOfEmployeer(string employeerId)
        {
            var orders = await _employeerRepository.GetAllOrdersOfEmployeer(employeerId);

            return View("AllOrdersOfEmployeer", orders);
        }

        
        public async Task<IActionResult> GetProfileOfEmployeer(string employeerId)
        {
            var employeer = await _employeerRepository.GetEmployeer(employeerId);

            var orders = await _employeerRepository.GetAllOrdersOfEmployeer(employeerId);

            var viewModel = new ProfileViewModel
            {
                Employeer = employeer,
                Orders = orders
            };

            return View("EmployeerProfile", viewModel);
        }


    }
}
