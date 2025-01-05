using AlvaCleanCRM.Models;

namespace AlvaCleanCRM.Repositories.Interfaces
{
    public interface IEmployeerRepository
    {
        public Task LogIn(string lastName, string password);

        public Task<List<Employeer>> GetAllEmployeers();

        public void SetUpRequestHeaderAuthorization();
    }
}
