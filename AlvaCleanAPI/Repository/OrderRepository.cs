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
                OrderStartDate = order.OrderStartDate.ToLocalTime(),
                Status = order.Status,
                Address = order.Address,
                OrderPriceType = order.OrderPriceType,
                ClientComments = order.ClientComments,
                Area = order.Area,
                Hour = order.Hour,
                CustomerName = order.CustomerName,
                Price = order.Price,
                Employeers = new List<string>()
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

            var updateEmployeer = Builders<Employeer>.Update.Pull(e => e.Orders, orderId);
            await _context.Employeers.UpdateManyAsync(
                e => e.Orders.Contains(orderId),
                updateEmployeer
            );

            var updateCustomer = Builders<Customer>.Update.Pull(c => c.Orders, orderId);
            await _context.Customers.UpdateOneAsync(
                c => c.Id == orderExist.CustomerId,
                updateCustomer
            );

            await _context.Orders.DeleteOneAsync(o => o.Id == orderId);
        }

        public async Task<Order> GetOrder(string orderId)
        {
            var order = await _context.Orders.Find(o => o.Id == orderId).SingleOrDefaultAsync();

            if (order == null)
                throw new Exception("Order not found!");

            return order;
        }

        public async Task<List<Order>> GetOrdersList()
        {
            var filter = Builders<Order>.Filter.Eq(o => o.Status, "In process");
            return await _context.Orders.Find(filter).ToListAsync();
        }

        public async Task UpdateOrder(OrderDto orderUpdatedData, string orderId)
        {
            var currentOrder = await GetOrder(orderId);

            var currentCustomer = await GetCustomer(currentOrder.CustomerId);

            var newCustomer = await GetCustomer(orderUpdatedData.CustomerId);

            if (currentCustomer != newCustomer)
            {
                var removeOrderFromCurrentCustomerFilter = Builders<Customer>.Filter.Eq(c => c.Id, currentCustomer.Id);
                var removeOrderFromCurrentCustomerUpdate = Builders<Customer>.Update.Pull(c => c.Orders, currentOrder.Id);
                await _context.Customers.UpdateOneAsync(removeOrderFromCurrentCustomerFilter, removeOrderFromCurrentCustomerUpdate);

                var addOrderToNewCustomerFilter = Builders<Customer>.Filter.Eq(c => c.Id, newCustomer.Id);
                var addOrderToNewCustomerUpdate = Builders<Customer>.Update.Push(c => c.Orders, currentOrder.Id);
                await _context.Customers.UpdateOneAsync(addOrderToNewCustomerFilter, addOrderToNewCustomerUpdate);
            }

            var filter = Builders<Order>.Filter.Eq(o => o.Id, currentOrder.Id);
            var update = Builders<Order>.Update
                .Set(o => o.OrderType, orderUpdatedData.OrderType)
                .Set(o => o.CustomerId, orderUpdatedData.CustomerId)
                .Set(o => o.Status, orderUpdatedData.Status)
                .Set(o => o.Address, orderUpdatedData.Address)
                .Set(o => o.OrderPriceType, orderUpdatedData.OrderPriceType)
                .Set(o => o.ClientComments, orderUpdatedData.ClientComments)
                .Set(o => o.OrderStartDate, orderUpdatedData.OrderStartDate.ToLocalTime())
                .Set(o => o.Area, orderUpdatedData.Area)
                .Set(o => o.Hour, orderUpdatedData.Hour)
                .Set(o => o.CustomerName, orderUpdatedData.CustomerName)
                .Set(o => o.Price, orderUpdatedData.Price);

            await _context.Orders.UpdateOneAsync(filter, update);
        }

        public async Task AddOrderToEmployeer(string orderId, string employeerId)
        {

            var order = await _context.Orders
                .Find(o => o.Employeers.Contains(employeerId) && o.Id == orderId)
                .FirstOrDefaultAsync();

            if (order != null)
                throw new Exception("This order already exists!");

            var updateEmployeer = Builders<Employeer>.Update.Push(c => c.Orders, orderId);

            await _context.Employeers.UpdateOneAsync(
                c => c.Id == employeerId,
                updateEmployeer
            );

            var updateOrder = Builders<Order>.Update.Push(o => o.Employeers, employeerId);
            await _context.Orders.UpdateOneAsync(
                o => o.Id == orderId,
                updateOrder
            );
        }

        public async Task DeleteOrderFromEmployeer(string orderId, string employeerId)
        {
            var updateEmployeer = Builders<Employeer>.Update.Pull(c => c.Orders, orderId);

            await _context.Employeers.UpdateOneAsync(
                c => c.Id == employeerId,
                updateEmployeer
            );


            var updateOrder = Builders<Order>.Update.Pull(o => o.Employeers, employeerId);
            await _context.Orders.UpdateOneAsync(
                o => o.Id == orderId,
                updateOrder
            );

        }

        public async Task<List<Order>> GetAllOrdersOfEmployeer(string employeerId)
        {
            return await _context.Orders
                 .Find(order =>
                     (order.Status == "In process")
                     && order.Employeers.Contains(employeerId)
                 )
                 .ToListAsync();
        }

        public async Task<List<Order>> GetAllOrdersOfCustomer(string customerId)
        {
            var customer = await _context.Customers
                .Find(c => c.Id == customerId)
                .SingleOrDefaultAsync();

            if (customer == null || customer.Orders == null)
            {
                return new List<Order>(); 
            }

            var orders = await _context.Orders
                .Find(o => customer.Orders.Contains(o.Id) && o.Status == "In process")
                .ToListAsync();

            return orders;
        }

        private async Task<Customer> GetCustomer(string customerId)
        {
            var filter = Builders<Customer>.Filter.Eq(c => c.Id, customerId);
            return await _context.Customers.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetListOfAllCompletedOrders()
        {
            var filter = Builders<Order>.Filter.Or(
                 Builders<Order>.Filter.Eq(o => o.Status, "Completed"),
                 Builders<Order>.Filter.Eq(o => o.Status, "Canceled")
            );

            var sort = Builders<Order>.Sort.Descending(o => o.OrderStartDate);

            return await _context.Orders.Find(filter).Sort(sort).ToListAsync();
        }

        public async Task<List<Order>> GetListOfAllCompletedOrdersOfEmployeer(string employeerId)
        {
            var statusFilter = Builders<Order>.Filter.Or(
                Builders<Order>.Filter.Eq(o => o.Status, "Completed"),
                Builders<Order>.Filter.Eq(o => o.Status, "Canceled")
            );

            var employeerFilter = Builders<Order>.Filter.AnyEq(o => o.Employeers, employeerId);

            var combinedFilter = Builders<Order>.Filter.And(statusFilter, employeerFilter);

            var sort = Builders<Order>.Sort.Descending(o => o.OrderStartDate);

            return await _context.Orders.Find(combinedFilter).Sort(sort).ToListAsync();
        }

        public async Task<List<Order>> GetListOfAllCompletedOrdersOfCustomer(string customerId)
        {
            var statusFilter = Builders<Order>.Filter.Or(
                Builders<Order>.Filter.Eq(o => o.Status, "Completed"),
                Builders<Order>.Filter.Eq(o => o.Status, "Canceled")
            );

            var customerFilter = Builders<Order>.Filter.Eq(o => o.CustomerId, customerId);

            var combinedFilter = Builders<Order>.Filter.And(statusFilter, customerFilter);

            var sort = Builders<Order>.Sort.Descending(o => o.OrderStartDate);

            return await _context.Orders.Find(combinedFilter).Sort(sort).ToListAsync();
        }
    }
}
