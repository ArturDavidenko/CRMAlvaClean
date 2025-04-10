using AlvaCleanAPI.DataContext;
using AlvaCleanAPI.Models;
using AlvaCleanAPI.Models.DTOs;
using AlvaCleanAPI.Repository.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.IdentityModel.Tokens.Jwt;
using static System.Net.Mime.MediaTypeNames;

namespace AlvaCleanAPI.Repository
{
    public class EmployeerRepository : IEmployeerRepository
    {
        private readonly MongoContext _context;
        private readonly IAuthRepository _authRepository;
        private readonly IGridFSBucket _gridFS;

        public EmployeerRepository(IOptions<MongoSettings> options, IAuthRepository authRepository)
        {
            _context = new MongoContext(options.Value.MongoUrl, options.Value.MongoDbName);
            _authRepository = authRepository;
            _gridFS = new GridFSBucket(_context._database);
        }


        public async Task AddPhotoToSystemForUse(IFormFile file)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                   await _gridFS.UploadFromStreamAsync(file.Name, stream);
                }
            }
            catch
            {
                throw new Exception("Can't load file!");
            }
        }

        public async Task RegisterNewEmployeer(RegisterEmployeerModel model)
        {
            var existUser = await _context.Employeers.Find(e => e.LastName == model.LastName).SingleOrDefaultAsync();

            if (existUser != null)
                throw new Exception("User already exists");
  
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var newEmployeer = new Employeer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                PasswordHash = passwordHash,
                PhoneNumber = model.PhoneNumber,
                Role = model.Role,
                Orders = new List<string>(),
                ImageId = null
            };
            
            await _context.Employeers.InsertOneAsync(newEmployeer);
        }

        public async Task AddPhotoToEmployeer(IFormFile file, string employeerId)
        {
            var employeer = await _context.Employeers.Find(e => e.Id == employeerId).SingleOrDefaultAsync();

            if (employeer == null)
                throw new Exception("Employeer not exist!");

            ObjectId imageId;
            using (var stream = file.OpenReadStream())
            {
               imageId = await _gridFS.UploadFromStreamAsync(file.Name, stream);
            }

            var filter = Builders<Employeer>.Filter.Eq(e => e.Id, employeerId);
            var update = Builders<Employeer>.Update.Set(e => e.ImageId, imageId.ToString());

            await _context.Employeers.UpdateOneAsync(filter, update);
        }


        public async Task<byte[]> GetEmployeerPhoto(string imageId)
        {
            using (var memoryStream = new MemoryStream())
            {
               
                if (!ObjectId.TryParse(imageId, out var id))
                {
                    throw new Exception("Failed to upload file");
                }

                await _gridFS.DownloadToStreamAsync(id, memoryStream);
                return memoryStream.ToArray(); 
            }
        }


        public byte[] GetEmployeerPhotoNotAsync(string imageId)
        {
            using (var memoryStream = new MemoryStream())
            {

                ObjectId.TryParse(imageId, out var id);
                
                try
                {
                    _gridFS.DownloadToStream(id, memoryStream);
                    return memoryStream.ToArray();
                }
                catch 
                {
                    return null;
                }
                
            }
        }

        public async Task DeletePhotoOfEmployeer(string employeerId)
        {
            try
            {
                //Here might be error like half data can be deleted but other half can get
                //exception and second half not changed but first half aready changed
                var emp = await GetEmployeer(employeerId);

                if (emp.ImageId == null)
                    return;

                var imageId = emp.ImageId;

                var filter = Builders<Employeer>.Filter.And(
                    Builders<Employeer>.Filter.Eq(e => e.Id, employeerId),
                    Builders<Employeer>.Filter.Eq(e => e.ImageId, imageId)
                );

                var update = Builders<Employeer>.Update.Set(e => e.ImageId, null);

                await _context.Employeers.UpdateOneAsync(filter, update);

                var fileId = new ObjectId(imageId);
                await _gridFS.DeleteAsync(fileId);
            }
            catch 
            {
                throw new Exception("Failed to delete photo !");
            }
            
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
            var employeer = await _context.Employeers.Find(e => e.Id == employeerId).SingleOrDefaultAsync();

            if (employeer != null && employeer.Role != "admin")
            {
                await DeletePhotoOfEmployeer(employeer.Id);

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
