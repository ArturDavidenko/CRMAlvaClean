using AlvaCleanAPI.DataContext;
using AlvaCleanAPI.Models;
using AlvaCleanAPI.Repository.Interfaces;
using Microsoft.Extensions.Options;

namespace AlvaCleanAPI.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MongoContext _context;

        public CustomerRepository(IOptions<MongoSettings> options)
        {
            _context = new MongoContext(options.Value.MongoUrl, options.Value.MongoDbName);
        }

        public Task CreateCustomer(RegisterCustomerModel customer)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCustomer(string customerId)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetCustomer(string customerId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Customer>> GetCustomersList()
        {
            throw new NotImplementedException();
        }

        public Task UpdateCustomer(Customer customerUpdatedData)
        {
            throw new NotImplementedException();
        }
    }
}
