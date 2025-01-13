using AlvaCleanCRM.Models;
using AlvaCleanCRM.Models.RegisterModels;

namespace AlvaCleanCRM.Repositories.Interfaces
{
    public interface IEmployeerRepository
    {
        public Task LogIn(string lastName, string password);

        public Task<List<Employeer>> GetAllEmployeers();

        public void SetUpRequestHeaderAuthorization();

        public Task AddNewEmployeer(RegisterEmployeerModel model);

        public Task<List<Order>> GetAllOrdersOfEmployeer(string employeerId);
    }
}
