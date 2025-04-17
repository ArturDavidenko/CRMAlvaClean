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
        private readonly HttpClient _httpClient;
        
        public CustomerRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task CreateNewCustomer(RegisterCustomerModel model)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync($"Customer/register-new-customer", jsonContent);
        }

        public async Task DeleteCustomer(string id)
        {
            await _httpClient.DeleteAsync($"Customer/delete-customer/{id}");
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            var response = await _httpClient.GetAsync($"Customer/get-list-of-customers");
            return await response.Content.ReadFromJsonAsync<List<Customer>>();
        }

        public async Task<Customer> GetCustomer(string id)
        {
            var response = await _httpClient.GetAsync($"Customer/get-customer/{id}");
            return await response.Content.ReadFromJsonAsync<Customer>();
        }

        public async Task UpdateCustomer(CustomerToUpdateInAPI model, string id)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"Customer/update-customer/{id}", jsonContent);
        }

        public async Task<List<Order>> GetCustomerOrdersList(string customerId)
        {
            var response = await _httpClient.GetAsync($"Order/get-all-orders-of-customer/{customerId}");
            return await response.Content.ReadFromJsonAsync<List<Order>>();
        }
       
    }
}
