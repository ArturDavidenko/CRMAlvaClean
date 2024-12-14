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
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }


        [HttpPost("register-new-customer")]
        public async Task<IActionResult> CreateNewCustomer([FromBody] RegisterCustomerModel model)
        {
            try
            {
                await _customerRepository.CreateCustomer(model);
                return Ok("Customer created!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("delete-customer/{customerId}")]
        public async Task<IActionResult> DeleteCustomer(string customerId)
        {
            try
            {
                await _customerRepository.DeleteCustomer(customerId);
                return Ok("Customer deleted!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-customer/{customerId}")]
        public async Task<IActionResult> GetCustomer(string customerId)
        {
            try
            {
                var customer = await _customerRepository.GetCustomer(customerId);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-list-of-customers")]
        public async Task<IActionResult> GetCustomersList()
        {
            try
            {
                var customersList = await _customerRepository.GetCustomersList();
                return Ok(customersList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }


        [HttpPut("update-customer/{customerId}")]
        public async Task<IActionResult> UpdateCustomer([FromBody]CustomerDto toUpdateCustomer, string customerId)
        {
            try
            {
                await _customerRepository.UpdateCustomer(toUpdateCustomer, customerId); 
                return Ok("Customer updated!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
