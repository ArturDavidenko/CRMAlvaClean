using AlvaCleanCRM.Models;
using AlvaCleanCRM.Models.SettingModels;
using AlvaCleanCRM.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace AlvaCleanCRM.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly string _customerUrl;
       
        public CustomerRepository(IOptions<ApiSettings> apiSettings)
        {
            _httpClient = new HttpClient();
            _httpContextAccessor = new HttpContextAccessor();
            _customerUrl = apiSettings.Value.CustomerUrl;
        }

        public Task CreateNewCustomer()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            SetUpRequestHeaderAuthorization();

            var response = await _httpClient.GetAsync($"{_customerUrl}/get-list-of-customers");
            return await response.Content.ReadFromJsonAsync<List<Customer>>();
        }

        public void SetUpRequestHeaderAuthorization()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
