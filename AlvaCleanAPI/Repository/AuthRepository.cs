using AlvaCleanAPI.Models;
using AlvaCleanAPI.Repository.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AlvaCleanAPI.Repository
{
    public class AuthRepository : IAuthRepository
    {

        private readonly IConfiguration _configuration;

        public AuthRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<JwtSecurityToken> GenerateJWTToken(Employeer employeer)
        {
            var authClaims = new[]
            {
                new Claim(ClaimTypes.Name, employeer.FirstName),
                new Claim(ClaimTypes.Role, employeer.Role),
                new Claim(ClaimTypes.NameIdentifier, employeer.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Issuer"],
                    expires: DateTime.Now.AddHours(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;

        }
    }
}
