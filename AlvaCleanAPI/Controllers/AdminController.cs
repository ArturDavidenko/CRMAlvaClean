﻿using AlvaCleanAPI.Models;
using AlvaCleanAPI.Models.DTOs;
using AlvaCleanAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlvaCleanAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "admin")]
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
                return Ok(employeer);
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
                return Ok(employeersList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            } 
        }





    }
}