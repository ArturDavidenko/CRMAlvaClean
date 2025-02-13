using AlvaCleanCRM.Models;
using AlvaCleanCRM.Models.DTOs;
using AlvaCleanCRM.Models.RegisterModels;

namespace AlvaCleanCRM.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        public Task<List<Customer>> GetAllCustomers();

        public Task CreateNewCustomer(RegisterCustomerModel model);

        public Task DeleteCustomer(string id);

        public Task<Customer> GetCustomer(string id);

        public Task UpdateCustomer(CustomerToUpdateInAPI model, string id);

        public Task<List<Order>> GetCustomerOrdersList(string customerId);
    }
}
