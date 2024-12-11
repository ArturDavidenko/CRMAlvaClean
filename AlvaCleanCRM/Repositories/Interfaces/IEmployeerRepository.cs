namespace AlvaCleanCRM.Repositories.Interfaces
{
    public interface IEmployeerRepository
    {
        public Task LogIn(string lastName, string password);
    }
}
