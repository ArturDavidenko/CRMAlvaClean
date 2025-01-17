using AlvaCleanCRM.Models;
using AlvaCleanCRM.Models.DTOs;
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
            var empOrders = await _employeerRepository.GetAllOrdersOfEmployeer(Id);
            return View("EmployeerOrdersPage", empOrders);
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


        public async Task<IActionResult> AllOrdersPage()
        {
            var orders = await _employeerRepository.GetAllOrders();
            return View("AllOrdersPage", orders);
        }

        public async Task<IActionResult> EditEmployeerPage(string id)
        {
            var employeer = await _employeerRepository.GetEmployeerToUpdate(id);
            return View("EditEmployeerPage", employeer);
        }


        public async Task<IActionResult> UpdateEmployeer(EmployeerToUpdateDto model)
        {
            await _employeerRepository.UpdateEmployeer(model);
            return RedirectToAction("EmployeersPage");
        }

    }
}
