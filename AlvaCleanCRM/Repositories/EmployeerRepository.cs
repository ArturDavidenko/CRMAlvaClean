using AlvaCleanCRM.Models.SettingModels;
using AlvaCleanCRM.Models.ViewModels;
using AlvaCleanCRM.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using AlvaCleanCRM.Models;
using AlvaCleanCRM.Models.RegisterModels;

namespace AlvaCleanCRM.Repositories
{
    public class EmployeerRepository : IEmployeerRepository
    {
        public readonly string _autAPIUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly string _empAPIUrl;
        private readonly string _adminAPIUrl;
        private readonly string _orderAPIUrl;

        public EmployeerRepository(IOptions<ApiSettings> apiSettings, HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _autAPIUrl = apiSettings.Value.AuthUrl;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _empAPIUrl = apiSettings.Value.EmployeerUrl;
            _orderAPIUrl = apiSettings.Value.OrderUrl;
            _adminAPIUrl = apiSettings.Value.AdminUrl;
        }

        public async Task<List<Employeer>> GetAllEmployeers()
        {
            SetUpRequestHeaderAuthorization();

            var response = await _httpClient.GetAsync($"{_empAPIUrl}/get-all-employeers");
            return await response.Content.ReadFromJsonAsync<List<Employeer>>();
        }

        public async Task LogIn(string lastName, string password)
        {
            var loginViewModel = new LoginViewModel
            {
                employeerLastName = lastName,
                employeerPassword = password
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(loginViewModel), Encoding.UTF8, "application/json");
            var responseLogin = await _httpClient.PostAsync(_autAPIUrl, jsonContent);
            var token = await responseLogin.Content.ReadFromJsonAsync<JwtTokenResponse>();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.Now.AddHours(1)
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("authToken", token.token, cookieOptions);
            _httpContextAccessor.HttpContext.Session.SetString("authToken", token.token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.token);
        }

        public async Task AddNewEmployeer(RegisterEmployeerModel model)
        {
            SetUpRequestHeaderAuthorization();

            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync($"{_empAPIUrl}/register-new-employeer", jsonContent);
        }


        public async Task<List<Order>> GetAllOrdersOfEmployeer(string employeerId)
        {
            SetUpRequestHeaderAuthorization();

            var response = await _httpClient.GetAsync($"{_orderAPIUrl}/get-all-orders-of-employeer/{employeerId}");
            var orders = await response.Content.ReadFromJsonAsync<List<Order>>();

            if (orders != null)
            {
                return orders;
            }

            return new List<Order>();

        }

        public void SetUpRequestHeaderAuthorization()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        
    }
}
