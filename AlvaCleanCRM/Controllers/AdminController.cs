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
        private readonly IOrderRepository _orderRepository;

        public AdminController(IEmployeerRepository employeerRepository, ICustomerRepository customerRepository, IOrderRepository orderRepository)
        {
            _employeerRepository = employeerRepository;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
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

        public async Task<IActionResult> CreateOrderPage()
        {
            var listOfCustomers = new List<string>();
            var customers = await _customerRepository.GetAllCustomers();
            foreach (var customer in customers)
            {
                listOfCustomers.Add(customer.ClientName);
            }

            var model = new RegisterOrderModel
            {
                CustomerName = listOfCustomers
            };

            return View(model);
        }

        public async Task<IActionResult> CreateOrder(RegisterOrderModel model)
        {
            var customerName = model.CustomerName.SingleOrDefault();
            var orderDto = new RegisterOrderDto
            {
                OrderPriceType = model.OrderPriceType,
                OrderType = model.OrderType,
                ClientComments = model.ClientComments,
                Address = model.Address,
                Status = model.Status
            };
            await _orderRepository.CreateOrder(orderDto, customerName);

            return RedirectToAction("AllOrdersPage");
        }

        public async Task<IActionResult> ShowOrderPage(string Id)
        {
            var order = await _orderRepository.GetOrder(Id);
            return View(order);
        }

        public async Task<IActionResult> EditOrderPage(string Id)
        {

            var listOfCustomers = new List<string>();

            var order = await _orderRepository.GetOrder(Id);

            var customers = await _customerRepository.GetAllCustomers();

            foreach (var customer in customers)
            {
                listOfCustomers.Add(customer.ClientName);
            }

            var orderToUpdate = new OrderToUpdateModel
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderType = order.OrderType,
                ClientComments= order.ClientComments,
                Address = order.Address,
                Status = order.Status,
                CustomersList = listOfCustomers,
                OrderPriceType = order.OrderPriceType
                
            };

            return View(orderToUpdate);
        }

       
        public async Task<IActionResult> UpdateOrder(OrderToUpdateModel model)
        {
            var customerName = model.CustomersList.SingleOrDefault();
            await _orderRepository.UpdateOrder(model, customerName);
            return RedirectToAction("AllOrdersPage");
        }

        public async Task<IActionResult> DeleteOrder(string Id)
        {
            await _orderRepository.DeleteOrder(Id);
            return RedirectToAction("AllOrdersPage");
        }

        public async Task<IActionResult> AddEmployeersToOrderPage(string Id)
        {
            var employeers = await _employeerRepository.GetAllEmployeers();

            var order = await _orderRepository.GetOrder(Id);

            var model = new AddEmployeerToOrderViewModel
            {
                Employeers = employeers,
                Order = order
            };

            return View(model);
        }

        public async Task<IActionResult> AddEmployeerToOrder(AddEmployeerToOrderViewModel model)
        {
            var employeerId = model.SelectedEmployeerId;
            await _orderRepository.AddEmployeerToOrder(model.Order.Id, employeerId);
            return RedirectToAction("AllOrdersPage");
        }

        public async Task<IActionResult> DeleteOrderFromEmployeer(AddEmployeerToOrderViewModel model)
        {
            var employeerId = model.SelectedEmployeerId;
            await _orderRepository.DeleteOrderFromEmployeer(model.Order.Id, employeerId);
            return RedirectToAction("AllOrdersPage");
        }

    }
}
