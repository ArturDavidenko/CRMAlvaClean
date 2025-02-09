using AlvaCleanCRM.Models;

namespace AlvaCleanCRM.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        public Task<List<Customer>> GetAllCustomers();

        public Task CreateNewCustomer();
    }
}
