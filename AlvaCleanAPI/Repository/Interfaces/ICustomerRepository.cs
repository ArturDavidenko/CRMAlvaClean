using AlvaCleanAPI.Models;
using AlvaCleanAPI.Models.DTOs;

namespace AlvaCleanAPI.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        public Task<Customer> GetCustomer(string customerId);

        public Task DeleteCustomer(string customerId);

        public Task<List<Customer>> GetCustomersList();

        public Task UpdateCustomer(CustomerDto customerUpdatedData, string customerId);

        public Task CreateCustomer(RegisterCustomerModel customer);

        public Task<Customer> GetCustomerByName(string customerName);
    }
}
