using AlvaCleanAPI.DataContext;
using AlvaCleanAPI.Models;
using Microsoft.Extensions.Options;

namespace AlvaCleanAPI.Repository
{
    public class OrderRepository
    {
        private readonly MongoContext _context;

        public OrderRepository(IOptions<MongoSettings> options)
        {
            _context = new MongoContext(options.Value.MongoUrl, options.Value.MongoDbName);
        }
    }
}
