using AlvaCleanAPI.Models;

namespace AlvaCleanAPI.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        public Task<Customer> GetCustomer(string customerId);

        public Task DeleteCustomer(string customerId);

        public Task<List<Customer>> GetCustomersList();

        public Task UpdateCustomer(Customer customerUpdatedData);

        public Task CreateCustomer(RegisterCustomerModel customer);
    }
}
