using AlvaCleanCRM.Models;
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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly string _customerUrl;
        private readonly string _orderUrl;
       
        public CustomerRepository(IOptions<ApiSettings> apiSettings)
        {
            _httpClient = new HttpClient();
            _httpContextAccessor = new HttpContextAccessor();
            _customerUrl = apiSettings.Value.CustomerUrl;
            _orderUrl = apiSettings.Value.OrderUrl;
        }

        public async Task CreateNewCustomer(RegisterCustomerModel model)
        {
            SetUpRequestHeaderAuthorization();
            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            await _httpClient.PostAsync($"{_customerUrl}/register-new-customer", jsonContent);

        }

        public async Task DeleteCustomer(string id)
        {
            SetUpRequestHeaderAuthorization();
            await _httpClient.DeleteAsync($"{_customerUrl}/delete-customer/{id}");
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            SetUpRequestHeaderAuthorization();

            var response = await _httpClient.GetAsync($"{_customerUrl}/get-list-of-customers");
            return await response.Content.ReadFromJsonAsync<List<Customer>>();
        }

        public async Task<Customer> GetCustomer(string id)
        {
            SetUpRequestHeaderAuthorization();
            var response = await _httpClient.GetAsync($"{_customerUrl}/get-customer/{id}");
            return await response.Content.ReadFromJsonAsync<Customer>();
        }

        public async Task UpdateCustomer(CustomerToUpdateInAPI model, string id)
        {
            SetUpRequestHeaderAuthorization();
            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync($"{_customerUrl}/update-customer/{id}", jsonContent);
        }

        public async Task<List<Order>> GetCustomerOrdersList(string customerId)
        {
            var response = await _httpClient.GetAsync($"{_orderUrl}/get-all-orders-of-customer/{customerId}");

            return await response.Content.ReadFromJsonAsync<List<Order>>();
        }

        public void SetUpRequestHeaderAuthorization()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

       
    }
}
