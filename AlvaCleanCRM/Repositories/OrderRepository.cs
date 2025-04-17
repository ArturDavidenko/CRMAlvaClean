using AlvaCleanCRM.Models.RegisterModels;
using AlvaCleanCRM.Models.SettingModels;
using AlvaCleanCRM.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using AlvaCleanCRM.Models.DTOs;
using AlvaCleanCRM.Models;
using System.Text.Json.Serialization;

namespace AlvaCleanCRM.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly HttpClient _httpClient;

        public OrderRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task CreateOrder(RegisterOrderDto model, string customerName)
        {
            var response = await _httpClient.GetAsync($"Customer/get-customer-by-name/{customerName}");
            var customer = await response.Content.ReadFromJsonAsync<Customer>();

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(model, options), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync($"Order/create-new-order/{customer.Id}", jsonContent);
        }

        public async Task<Order> GetOrder(string orderId)
        {
            var response = await _httpClient.GetAsync($"Order/get-order/{orderId}");
            return await response.Content.ReadFromJsonAsync<Order>();
        }

        public async Task UpdateOrder(OrderToUpdateModel model, string customerName)
        {
            var response = await _httpClient.GetAsync($"Customer/get-customer-by-name/{customerName}");
            var customer = await response.Content.ReadFromJsonAsync<Customer>();

            var ordelToUpdate = new OrderToUpdateDto
            {
                OrderType = model.OrderType,
                CustomerId = customer.Id,
                Status = model.Status,
                Address = model.Address,
                OrderPriceType = model.OrderPriceType,
                ClientComments = model.ClientComments,
                Price = model.Price,
                Area = model.Area,
                CustomerName = customerName,
                Hour = model.Hour,
                OrderStartDate = model.OrderStartDate,
            };

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(ordelToUpdate, options), Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"Order/update-order/{model.Id}", jsonContent);

        }

        public async Task DeleteOrder(string orderId)
        {
            await _httpClient.DeleteAsync($"Order/delete-order/{orderId}");
        }

        public async Task AddEmployeerToOrder(string orderId, string employeerId)
        {
            await _httpClient.PostAsync($"Order/add-order-to-employeer/{orderId}/{employeerId}", null);
        }

        public async Task DeleteOrderFromEmployeer(string orderId, string employeerId)
        {
            await _httpClient.DeleteAsync($"Order/delete-order-from-employeer/{orderId}/{employeerId}");
        }

        public async Task<List<Order>> GetAllCompletedOrders()
        {
            var response = await _httpClient.GetAsync($"Order/get-all-completed-orders");
            return await response.Content.ReadFromJsonAsync<List<Order>>();
        }

        public async Task<List<Order>> GetAllCompletedOrdersOfEmployeer(string employeerId)
        {
            var response = await _httpClient.GetAsync($"Order/get-all-completed-orders-of-employeer/{employeerId}");
            return await response.Content.ReadFromJsonAsync<List<Order>>();
        }

        public async Task<List<Order>> GetAllCompletedOrdersOfCustomer(string customerId)
        {
            var response = await _httpClient.GetAsync($"Order/get-all-completed-orders-of-customer/{customerId}");
            return await response.Content.ReadFromJsonAsync<List<Order>>();
        }   
    }
}
