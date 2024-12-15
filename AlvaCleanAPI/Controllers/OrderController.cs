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
        public async Task<IActionResult> CreteOrder([FromBody]RegisterOrderModel order, string customerId)
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
    }
}
