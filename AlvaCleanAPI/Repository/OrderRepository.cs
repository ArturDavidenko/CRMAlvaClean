using AlvaCleanAPI.DataContext;
using AlvaCleanAPI.Models;
using AlvaCleanAPI.Models.DTOs;
using AlvaCleanAPI.Repository.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AlvaCleanAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MongoContext _context;

        public OrderRepository(IOptions<MongoSettings> options)
        {
            _context = new MongoContext(options.Value.MongoUrl, options.Value.MongoDbName);
        }

        public async Task CreateOrder(RegisterOrderModel order, string customerId)
        {
            var newOrder = new Order
            {
                CustomerId = customerId,
                OrderType = order.OrderType,
                OrderDate = DateTime.Now,
                Status = order.Status,
                Address = order.Address,
                OrderPriceType = order.OrderPriceType,
                ClientComments = order.ClientComments
            };

            _context.Orders.InsertOne(newOrder);

            var updateDefinition = Builders<Customer>.Update.Push(c => c.Orders, newOrder.Id);
            await _context.Customers.UpdateOneAsync(c => c.Id == customerId, updateDefinition);
        }

        public Task DeleteOrder(string orderId)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrder(string orderId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetOrdersList()
        {
            throw new NotImplementedException();
        }

        public Task UpdateOrder(OrderDto orderUpdatedData, string orderId)
        {
            throw new NotImplementedException();
        }
    }
}
