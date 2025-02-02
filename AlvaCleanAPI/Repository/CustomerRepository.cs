using AlvaCleanAPI.DataContext;
using AlvaCleanAPI.Models;
using AlvaCleanAPI.Models.DTOs;
using AlvaCleanAPI.Repository.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AlvaCleanAPI.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MongoContext _context;

        public CustomerRepository(IOptions<MongoSettings> options)
        {
            _context = new MongoContext(options.Value.MongoUrl, options.Value.MongoDbName);
        }

        public async Task CreateCustomer(RegisterCustomerModel customer)
        {
            var customerExist = await _context.Customers.Find(c => c.ClientName == customer.ClientName).SingleOrDefaultAsync();
            
            if (customerExist != null)
            {
                throw new Exception("Customer already exist!");
            }

            var newCustomer = new Customer
            {
                ClientName = customer.ClientName,
                CustomerType = customer.CustomerType,
                CompanyName = customer.CompanyName,
                ContactPhone = customer.ContactPhone,
                Orders = new List<string>()
            };

            await _context.Customers.InsertOneAsync(newCustomer);
        }

        public async Task DeleteCustomer(string customerId)
        {
            var customer = await _context.Customers.Find(e => e.Id == customerId).SingleOrDefaultAsync();

            if (customer == null)
            {
                throw new Exception("Customer not exist !");
            }

            await _context.Customers.DeleteOneAsync(e => e.Id == customerId);
        }

        public async Task<Customer> GetCustomer(string customerId)
        {
            var customer = await _context.Customers.Find(e => e.Id == customerId).SingleOrDefaultAsync();

            if (customer == null) 
            {
                throw new Exception("Customer not exist!");
            }

            return customer;
        }

        public async Task<List<Customer>> GetCustomersList()
        {
            var customers = await _context.Customers.Find(_ => true).ToListAsync();
            return customers;
        }

        public async Task UpdateCustomer(CustomerDto customerUpdatedData, string customerId)
        {
            var currentCustomer = await GetCustomer(customerId);

            var filter = Builders<Customer>.Filter.Eq(c => c.Id, currentCustomer.Id);

            var update = Builders<Customer>.Update
                .Set(c => c.ClientName, customerUpdatedData.ClientName)
                .Set(c => c.CompanyName, customerUpdatedData.CompanyName)
                .Set(c => c.ContactPhone, customerUpdatedData.ContactPhone)
                .Set(c => c.CustomerType, customerUpdatedData.CustomerType);

            await _context.Customers.UpdateOneAsync(filter, update);
        }
    }
}
