using AlvaCleanAPI.Models;
using AlvaCleanAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace AlvaCleanAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IEmployeerRepository _employeerRepository;

        public AuthController(IEmployeerRepository employeerRepository)
        {
            _employeerRepository = employeerRepository;
        }

        [HttpPost("Login-employeers")]
        public async Task<IActionResult> LogIn([FromBody] LoginEmployeerModel Model)
        {
            try
            {
                var JwtToken = await _employeerRepository.LoginEmployeer(Model);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(JwtToken),
                    expiration = JwtToken.ValidTo
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }

    }
}
