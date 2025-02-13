using AlvaCleanAPI.Models;
using AlvaCleanAPI.Models.DTOs;

namespace AlvaCleanAPI.Repository.Interfaces
{
    public interface IOrderRepository
    {
        public Task<Order> GetOrder(string orderId);

        public Task DeleteOrder(string orderId);

        public Task CreateOrder(RegisterOrderModel order, string customerId);

        public Task<List<Order>> GetOrdersList();

        public Task UpdateOrder(OrderDto orderUpdatedData, string orderId);

        public Task AddOrderToEmployeer(string orderId, string employeerId);

        public Task DeleteOrderFromEmployeer(string orderId, string employeerId);

        public Task<List<Order>> GetAllOrdersOfEmployeer(string employeerId);

        public Task<List<Order>> GetAllOrdersOfCustomer(string customerId);
    }
}
