using AlvaCleanAPI.DataContext;
using AlvaCleanAPI.Models;
using AlvaCleanAPI.Models.DTOs;
using AlvaCleanAPI.Repository.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using RabbitMQ.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace AlvaCleanAPI.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly MongoContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(IConfiguration configuration, IOptions<MongoSettings> options)
        {
            _context = new MongoContext(options.Value.MongoUrl, options.Value.MongoDbName);
            _configuration = configuration;
        }

        public async Task<JwtSecurityToken> GenerateJWTToken(Employeer employeer)
        {
            var authClaims = new[]
            {
                new Claim(ClaimTypes.Name, employeer.LastName),
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

        public async Task<EmployeeDtoToMonitoringService> PublishLoginEventToQueue(LoginEmployeerModel Model)
        {
            var employeer = await _context.Employeers.Find(e => e.LastName == Model.EmployeerLastName).SingleOrDefaultAsync();

            EmployeeDtoToMonitoringService monitoringData;

            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(queue: "LogIn", durable: false, exclusive: false, autoDelete: false, arguments: null);

                monitoringData = new EmployeeDtoToMonitoringService
                {
                    Id = employeer.Id,
                    FirstName = employeer.FirstName,
                    LastName = employeer.LastName,
                    PhoneNumber = employeer.PhoneNumber,
                    Role = employeer.Role,
                    PasswordHash = employeer.PasswordHash,
                    ImageId = employeer.ImageId,
                    Orders = employeer.Orders,
                    DateTime = DateTime.Now,
                };

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(monitoringData));

                await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "LogIn", body: body);

                return monitoringData;
            }
            catch
            {
                throw new Exception("RabbitMQ not connected!");
            }

        }
    }
}
