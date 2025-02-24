using AlvaCleanCRM.Models;
using AlvaCleanCRM.Models.DTOs;
using AlvaCleanCRM.Models.RegisterModels;
using AlvaCleanCRM.Models.ViewModels;

namespace AlvaCleanCRM.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task CreateOrder(RegisterOrderDto model, string customerName);

        public Task<Order> GetOrder(string orderId);

        public Task UpdateOrder(OrderToUpdateModel model, string customerName);

        public Task DeleteOrder(string orderId); 

        public Task AddEmployeerToOrder(string orderId, string employeerId);

        public Task DeleteOrderFromEmployeer(string orderId, string employeerId);
    }
}
