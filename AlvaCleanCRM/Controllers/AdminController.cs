using AlvaCleanCRM.Models;
using AlvaCleanCRM.Models.DTOs;
using AlvaCleanCRM.Models.RegisterModels;
using AlvaCleanCRM.Models.ViewModels;
using AlvaCleanCRM.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IdentityModel.Tokens.Jwt;

namespace AlvaCleanCRM.Controllers
{
    public class AdminController : Controller
    {
        private readonly IEmployeerRepository _employeerRepository;
        private readonly ICustomerRepository _customerRepository;

        public AdminController(IEmployeerRepository employeerRepository, ICustomerRepository customerRepository)
        {
            _employeerRepository = employeerRepository;
            _customerRepository = customerRepository;
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


        public async Task<IActionResult> DeletePhotoOfEmployeer(string Id)
        {
            await _employeerRepository.DeleteImageOfEmployeer(Id);
            return RedirectToAction("EmployeersPage");
        }


        public async Task<IActionResult> GetContentOfEmployeer(string Id)
        {
            var employeer = await _employeerRepository.GetEmployeer(Id);

            var orders = await _employeerRepository.GetAllOrdersOfEmployeer(employeer.Id);

            var viewModel = new ContentOfEmployeerViewModel
            {
                Employeer = employeer,
                Orders = orders
            };

            return View("ContentOfEmployeerPage", viewModel);
        }

        
        public async Task<IActionResult> DeleteEmployeer(string Id)
        {
            await _employeerRepository.DeleteEmployeer(Id);
            return RedirectToAction("EmployeersPage");
        }


        public async Task<IActionResult> GetListOfAllCustomer()
        {
            var customersList = await _customerRepository.GetAllCustomers();
            return View("AllCustomersListPage", customersList);
        }

        public async Task<IActionResult> AddNewCustomerPage()
        {
            return View();
        }

        public async Task<IActionResult> AddNewCustomer(RegisterCustomerModel model)
        {
            await _customerRepository.CreateNewCustomer(model);
            return RedirectToAction("GetListOfAllCustomer");
        }

        public async Task<IActionResult> DeleteCustomer(string Id)
        {
            await _customerRepository.DeleteCustomer(Id);
            return RedirectToAction("GetListOfAllCustomer");
        }

        public async Task<IActionResult> EditCustomerPage(string Id)
        {
            var customer =  await _customerRepository.GetCustomer(Id);

            var customerToUpdate = new CustomerToUpdateDto
            {
                Id = customer.Id,
                ClientName = customer.ClientName,
                ContactPhone = customer.ContactPhone,
                CustomerType = customer.CustomerType
            };

            return View(customerToUpdate);
        }

        public async Task<IActionResult> UpdateCustomer(CustomerToUpdateDto model)
        {
            var modelToUpdate = new CustomerToUpdateInAPI
            {
                ClientName = model.ClientName,
                ContactPhone = model.ContactPhone,
                CustomerType = model.CustomerType
            };

            await _customerRepository.UpdateCustomer(modelToUpdate, model.Id);
            return RedirectToAction("GetListOfAllCustomer");
        }


        public async Task<IActionResult> CustomerViewPage(string Id)
        {
            var customer =  await _customerRepository.GetCustomer(Id);
            var ordersOfCustomer = await _customerRepository.GetCustomerOrdersList(Id);

            var viewModel = new CustomerViewModel
            {
                Id = customer.Id,
                ClientName = customer.ClientName,
                ContactPhone = customer.ContactPhone,
                CustomerType = customer.CustomerType,
                Orders = ordersOfCustomer
            };

            return View(viewModel);
        }
    }
}
