using AlvaCleanCRM.Models.ViewModels;
using AlvaCleanCRM.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlvaCleanCRM.Controllers
{
    public class EmployeerController : Controller
    {
        private readonly IEmployeerRepository _employeerRepository;
        private readonly IOrderRepository _orderRepository;

        public EmployeerController(IEmployeerRepository employeerRepository, IOrderRepository orderRepository)
        {
            _employeerRepository = employeerRepository;
            _orderRepository = orderRepository;
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


        public async Task<IActionResult> HistoryOfAllOrdersOfEmployeerPage(string Id)
        {
            var orders = await _orderRepository.GetAllCompletedOrdersOfEmployeer(Id);

            return View("EmployeerHistoryOfOrders", orders);
        }
    }
}
