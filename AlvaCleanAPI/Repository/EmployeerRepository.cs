using AlvaCleanAPI.DataContext;
using AlvaCleanAPI.Models;
using AlvaCleanAPI.Models.DTOs;
using AlvaCleanAPI.Repository.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;

namespace AlvaCleanAPI.Repository
{
    public class EmployeerRepository : IEmployeerRepository
    {
        private readonly MongoContext _context;
        private readonly IAuthRepository _authRepository;

        public EmployeerRepository(IOptions<MongoSettings> options, IAuthRepository authRepository)
        {
            _context = new MongoContext(options.Value.MongoUrl, options.Value.MongoDbName);
            _authRepository = authRepository;
        }

        public async Task RegisterNewEmployeer(RegisterEmployeerModel model)
        {
            var existUser = await _context.Employeers.Find(e => e.LastName == model.LastName).SingleOrDefaultAsync();

            if (existUser != null)
            {
                throw new Exception("User already exists");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var newEmployeer = new Employeer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                PasswordHash = passwordHash,
                PhoneNumber = model.PhoneNumber,
                Role = model.Role,
            };

            await _context.Employeers.InsertOneAsync(newEmployeer);
        }

        public async Task<JwtSecurityToken> LoginEmployeer(LoginEmployeerModel model)
        {
            var employeer = await _context.Employeers.Find(e => e.LastName == model.EmployeerLastName).SingleOrDefaultAsync();

            if (employeer == null || !BCrypt.Net.BCrypt.Verify(model.EmployeerPassword, employeer.PasswordHash))
            {
                throw new Exception("Invalid credentials");
            }

            return await _authRepository.GenerateJWTToken(employeer);
        }

        public async Task DeleteEmployeer(string employeerId)
        {
            var employeer = await _context.Employeers.Find(e => e.Id.ToString() == employeerId).SingleOrDefaultAsync();

            if (employeer != null && employeer.Role != "admin")
            {
                await _context.Employeers.DeleteOneAsync(e => e.Id == employeerId);
            }
            else
            {
                throw new Exception("Employeer not exist or you try delete admin!");
            }
        }

        public async Task<Employeer> GetEmployeer(string employeerId)
        {
            var employeer = await _context.Employeers.Find(e => e.Id == employeerId).SingleOrDefaultAsync();

            if (employeer == null)
            {
                throw new Exception("Employeer not exist!");
            }

            return employeer;
        }

        public async Task UpdateEmployeer(EmployeerDto toUpdateEmployeer, string employeerId)
        {

            var currentEmployer = await GetEmployeer(employeerId);
            
            var filter = Builders<Employeer>.Filter.Eq(e => e.Id, currentEmployer.Id);

            var update = Builders<Employeer>.Update
                .Set(e => e.FirstName, toUpdateEmployeer.FirstName)
                .Set(e => e.LastName, toUpdateEmployeer.LastName)
                .Set(e => e.PhoneNumber, toUpdateEmployeer.PhoneNumber)
                .Set(e => e.Role, toUpdateEmployeer.Role);

            await _context.Employeers.UpdateOneAsync(filter, update);
        }

        public async Task<List<Employeer>> GetListOfEmployeers()
        {
            return await _context.Employeers.Find(_ => true).ToListAsync();
        }



    }
}
