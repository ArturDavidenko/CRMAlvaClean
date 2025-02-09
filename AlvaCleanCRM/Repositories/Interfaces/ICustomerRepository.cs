using AlvaCleanCRM.Models;
using AlvaCleanCRM.Models.RegisterModels;

namespace AlvaCleanCRM.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        public Task<List<Customer>> GetAllCustomers();

        public Task CreateNewCustomer(RegisterCustomerModel model);
    }
}
