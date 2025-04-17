using AlvaCleanAPI.Models;
using AlvaCleanAPI.Models.DTOs;
using AlvaCleanAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlvaCleanAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "employeer,admin")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpPost("create-new-order/{customerId}")]
        public async Task<IActionResult> CreteOrder([FromBody] RegisterOrderModel order, string customerId)
        {
            try
            {
                await _orderRepository.CreateOrder(order, customerId);
                return Ok("Order created!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-all-orders")]
        public async Task<IActionResult> GetAllOrdersList()
        {
            try
            {
                var ordersList = await _orderRepository.GetOrdersList();
                return Ok(ordersList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-order/{orderId}")]
        public async Task<IActionResult> GetOrder(string orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrder(orderId);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("update-order/{orderId}")]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderDto order, string orderId)
        {
            try
            {
                await _orderRepository.UpdateOrder(order, orderId);
                return Ok("Order updated!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpDelete("delete-order/{orderId}")]
        public async Task<IActionResult> DeleteOrder(string orderId)
        {
            try
            {
                await _orderRepository.DeleteOrder(orderId);
                return Ok("Order deleted!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("add-order-to-employeer/{orderId}/{employeerId}")]
        public async Task<IActionResult> AddOrderToEmployeer(string orderId, string employeerId)
        {
            try
            {
                await _orderRepository.AddOrderToEmployeer(orderId, employeerId);
                return Ok("Order added to employeer!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-order-from-employeer/{orderId}/{employeerId}")]
        public async Task<IActionResult> DeleteOrderFromEmployeer(string orderId, string employeerId)
        {
            try
            {
                await _orderRepository.DeleteOrderFromEmployeer(orderId, employeerId);
                return Ok("Order deleted from employeer!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-all-orders-of-employeer/{employeerId}")]
        public async Task<IActionResult> GetAllOrdersOfEmployeer(string employeerId)
        {
            try
            {
                var orders = await _orderRepository.GetAllOrdersOfEmployeer(employeerId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-all-orders-of-customer/{customerId}")]
        public async Task<IActionResult> GetAllOrdersOfCustomer(string customerId)
        {
            try
            {
                var orders = await _orderRepository.GetAllOrdersOfCustomer(customerId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all-completed-orders")]
        public async Task<IActionResult> GetAllCompletedOrders()
        {
            try
            {
                var orders = await _orderRepository.GetListOfAllCompletedOrders();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all-completed-orders-of-employeer/{employeerId}")]
        public async Task<IActionResult> GetAllCompletedOrdersOfEmployeer(string employeerId)
        {
            try
            {
                var orders = await _orderRepository.GetListOfAllCompletedOrdersOfEmployeer(employeerId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all-completed-orders-of-customer/{customerId}")]
        public async Task<IActionResult> GetAllCompletedOrdersOfCustomer(string customerId)
        {
            try
            {
                var orders = await _orderRepository.GetListOfAllCompletedOrdersOfCustomer(customerId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
