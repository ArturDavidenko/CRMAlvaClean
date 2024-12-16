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

        public async Task DeleteOrder(string orderId)
        {
            var orderExist = await _context.Orders.Find(o => o.Id == orderId).SingleOrDefaultAsync();

            if (orderExist == null)
            {
                throw new Exception("Order not exist!");
            }

            var updateDefinition = Builders<Customer>.Update.Pull(c => c.Orders, orderId);

            var result = await _context.Customers.UpdateOneAsync(
                c => c.Id == orderExist.CustomerId,
                updateDefinition
            );

            await _context.Orders.DeleteOneAsync(o => o.Id == orderId);
        }

        public async Task<Order> GetOrder(string orderId)
        {
            var order = await _context.Orders.Find(o => o.Id == orderId).SingleOrDefaultAsync();

            if (order == null)
            {
                throw new Exception("Order not found!");
            }

            return order;
        }

        public async Task<List<Order>> GetOrdersList()
        {
            return await _context.Orders.Find(_ => true).ToListAsync();
        }

        public async Task UpdateOrder(OrderDto orderUpdatedData, string orderId)
        {
            var currentOrder = await GetOrder(orderId);
            
            var filter = Builders<Order>.Filter.Eq(o => o.Id, currentOrder.Id);

            var update = Builders<Order>.Update
                .Set(o => o.OrderType, orderUpdatedData.OrderType)
                .Set(o => o.Status, orderUpdatedData.Status)
                .Set(o => o.Address, orderUpdatedData.Address)
                .Set(o => o.OrderPriceType, orderUpdatedData.OrderPriceType)
                .Set(o => o.ClientComments, orderUpdatedData.ClientComments);

            await _context.Orders.UpdateOneAsync(filter, update);
        }
    }
}
