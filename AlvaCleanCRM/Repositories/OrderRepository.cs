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
        private readonly string _orderUrl;
        private readonly string _customerUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;


        public OrderRepository(IOptions<ApiSettings> apiSettings, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpContextAccessor = httpContextAccessor;
            _orderUrl = apiSettings.Value.OrderUrl;
            _customerUrl = apiSettings.Value.CustomerUrl;
        }

        public async Task CreateOrder(RegisterOrderDto model, string customerName)
        {
            SetUpRequestHeaderAuthorization();
         
            var response = await _httpClient.GetAsync($"{_customerUrl}/get-customer-by-name/{customerName}");
            var customer = await response.Content.ReadFromJsonAsync<Customer>();

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(model, options), Encoding.UTF8, "application/json");

            await _httpClient.PostAsync($"{_orderUrl}/create-new-order/{customer.Id}", jsonContent);
        }

        public async Task<Order> GetOrder(string orderId)
        {
            SetUpRequestHeaderAuthorization();
            var response = await _httpClient.GetAsync($"{_orderUrl}/get-order/{orderId}");
            return await response.Content.ReadFromJsonAsync<Order>();
        }

        public async Task UpdateOrder(OrderToUpdateModel model, string customerName)
        {
            SetUpRequestHeaderAuthorization();

            var response = await _httpClient.GetAsync($"{_customerUrl}/get-customer-by-name/{customerName}");
            var customer = await response.Content.ReadFromJsonAsync<Customer>();

            var ordelToUpdate = new OrderToUpdateDto
            {
                OrderType = model.OrderType,
                CustomerId = customer.Id,
                Status = model.Status,
                Address = model.Address,
                OrderPriceType = model.OrderPriceType,
                ClientComments = model.ClientComments
            };

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(ordelToUpdate, options), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync($"{_orderUrl}/update-order/{model.Id}", jsonContent);

        }

        public async Task DeleteOrder(string orderId)
        {
            SetUpRequestHeaderAuthorization();

            await _httpClient.DeleteAsync($"{_orderUrl}/delete-order/{orderId}");
            
        }

        public void SetUpRequestHeaderAuthorization()
        {
            var token = _httpContextAccessor?.HttpContext?.Session.GetString("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        
    }
}
