using AlvaCleanCRM.Models;
using AlvaCleanCRM.Models.DTOs;
using AlvaCleanCRM.Models.RegisterModels;

namespace AlvaCleanCRM.Repositories.Interfaces
{
    public interface IEmployeerRepository
    {
        public Task LogIn(string lastName, string password);

        public Task<List<Employeer>> GetAllEmployeers();

        public Task AddNewEmployeer(RegisterEmployeerModel model);

        public Task<List<Order>> GetAllOrdersOfEmployeer(string employeerId);

        public Task<List<Order>> GetAllOrders();

        public Task<Employeer> GetEmployeer(string id);

        public Task UpdateEmployeer(EmployeerToUpdateDto model);

        public Task<EmployeerToUpdateDto> GetEmployeerToUpdate(string id);

        public Task DeleteImageOfEmployeer(string employeerId);

        public Task DeleteEmployeer(string id);
    }
}
