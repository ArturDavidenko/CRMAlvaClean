using AlvaCleanAPI.Models;
using MongoDB.Driver;

namespace AlvaCleanAPI.DataContext
{
    public class MongoContext
    {
        public readonly IMongoDatabase _database;
        public MongoContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Customer> Customers => _database.GetCollection<Customer>("customers");
        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("orders");
        public IMongoCollection<Employeer> Employeers => _database.GetCollection<Employeer>("employeers");
    }
}
