using AlvaCleanAPI.Models;
using System.IdentityModel.Tokens.Jwt;

namespace AlvaCleanAPI.Repository.Interfaces
{
    public interface IAuthRepository
    {
        public Task<JwtSecurityToken> GenerateJWTToken(Employeer employeer);
    }
}
