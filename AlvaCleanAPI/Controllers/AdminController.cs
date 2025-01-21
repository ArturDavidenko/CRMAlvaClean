using AlvaCleanAPI.Models;
using AlvaCleanAPI.Models.DTOs;
using AlvaCleanAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlvaCleanAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    //[Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IEmployeerRepository _employeerRepository;

        public AdminController(IEmployeerRepository employeerRepository)
        {
            _employeerRepository = employeerRepository;
        }

        [HttpPost("register-new-employeer")]
        public async Task<IActionResult> CreateEmployeer([FromBody] RegisterEmployeerModel model)
        {
            try
            {
                await _employeerRepository.RegisterNewEmployeer(model);
                return Ok("Employeer Created!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("add-photo-to-employeer/{employeerId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddPhoto([FromForm] IFormFile file, string employeerId)
        {
            try
            {
                await _employeerRepository.AddPhotoToEmployeer(file, employeerId);
                return Ok("Image Upload!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-employeer-photo/{imageId}")]
        public async Task<IActionResult> GetEmployeerPhoto(string imageId)
        {
            try
            {
                var photo = await _employeerRepository.GetEmployeerPhoto(imageId);
                return Ok(photo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);         
            }
        }

        [HttpDelete("delete-photo-of-employeer/{imageId}")]
        public async Task<IActionResult> DeletePhotoOfEmployeer(string imageId)
        {
            try
            {
                await _employeerRepository.DeletePhotoOfEmployeer(imageId);
                return Ok("Photo deleted!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("delete-employeer/{employeerId}")]
        public async Task<IActionResult> DeleteEmployeer(string employeerId)
        {
            try
            {
                await _employeerRepository.DeleteEmployeer(employeerId);
                return Ok("Employeer deleted!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-employeer/{employeerId}")]
        public async Task<IActionResult> GetEmployeer(string employeerId)
        {
            try
            {
                var employeer = await _employeerRepository.GetEmployeer(employeerId);
                var employeerDto = new EmployeerDtoForFetchData
                {
                    Id = employeer.Id,
                    FirstName = employeer.FirstName,
                    LastName = employeer.LastName,
                    PhoneNumber = employeer.PhoneNumber,
                    Role = employeer.Role,
                    Orders = employeer.Orders,
                    Image = _employeerRepository.GetEmployeerPhotoNotAsync(employeer.ImageId),
                    ImageId = employeer.ImageId
                };
                return Ok(employeerDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("update-employeer/{employeerId}")]
        public async Task<IActionResult> UpdateEmployeer([FromBody]EmployeerDto toUpdateEmployeer, string employeerId)
        {
            try
            {
                await _employeerRepository.UpdateEmployeer(toUpdateEmployeer, employeerId);
                return Ok("Employeer updated!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all-employeers")]
        public async Task<IActionResult> GetAllEmployeers()
        {
            try
            {
                var employeersList = await _employeerRepository.GetListOfEmployeers();
                var employeerDtos = employeersList.Select(e => new EmployeerDtoForFetchData
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    PhoneNumber = e.PhoneNumber,
                    Role = e.Role,
                    Orders = e.Orders,
                    Image = _employeerRepository.GetEmployeerPhotoNotAsync(e.ImageId),
                    ImageId = e.ImageId
                }).ToList();

                return Ok(employeerDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 
        }
    }
}
