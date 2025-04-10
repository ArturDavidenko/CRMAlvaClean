using AlvaCleanAPI.Models;
using AlvaCleanAPI.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;

namespace AlvaCleanAPI.Repository.Interfaces
{
    public interface IAuthRepository
    {
        public Task<JwtSecurityToken> GenerateJWTToken(Employeer employeer);

        public Task<EmployeeDtoToMonitoringService> PublishLoginEventToQueue(LoginEmployeerModel Model);
    }
}
