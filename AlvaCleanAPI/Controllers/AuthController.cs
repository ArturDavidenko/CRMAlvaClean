using AlvaCleanAPI.Models;
using AlvaCleanAPI.Models.DTOs;
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
        private readonly IAuthRepository _authRepository;

        public AuthController(IEmployeerRepository employeerRepository, IAuthRepository authRepository)
        {
            _employeerRepository = employeerRepository;
            _authRepository = authRepository;   
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

        [HttpPost("Login-employeersV2")]
        public async Task<IActionResult> PublishLogInToQueue([FromBody] LoginEmployeerModel Model)
        {
            try
            {
                EmployeeDtoToMonitoringService dataToPublish;
                var JwtToken = await _employeerRepository.LoginEmployeer(Model);

                if (JwtToken != null)
                {
                    dataToPublish = await _authRepository.PublishLoginEventToQueue(Model);
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(JwtToken),
                        expiration = JwtToken.ValidTo,
                        message = "Data was send to queue !!!",
                        data = dataToPublish
                    });
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
