using AlvaCleanCRM.Models.RegisterModels;
using AlvaCleanCRM.Models.SettingModels;
using AlvaCleanCRM.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using AlvaCleanCRM.Models.DTOs;

namespace AlvaCleanCRM.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _orderUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public OrderRepository(IOptions<ApiSettings> apiSettings, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpContextAccessor = httpContextAccessor;
            _orderUrl = apiSettings.Value.OrderUrl;
        }

        public async Task CreateOrder(RegisterOrderDto model, string customerId)
        {
            SetUpRequestHeaderAuthorization();
            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            await _httpClient.PostAsync($"{_orderUrl}/create-new-order/{customerId}", jsonContent);
        }


        public void SetUpRequestHeaderAuthorization()
        {
            var token = _httpContextAccessor?.HttpContext?.Session.GetString("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
