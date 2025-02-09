using AlvaCleanCRM.Models;
using AlvaCleanCRM.Models.RegisterModels;
using AlvaCleanCRM.Models.SettingModels;
using AlvaCleanCRM.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

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

        public async Task CreateNewCustomer(RegisterCustomerModel model)
        {
            SetUpRequestHeaderAuthorization();

            //Temporary version (BAD CODE!)
            if (model.ClientName == null)
            {
                model.ClientName = "";
            }
            else
            {
                model.CompanyName = "";
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            await _httpClient.PostAsync($"{_customerUrl}/register-new-customer", jsonContent);

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
