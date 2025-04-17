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
using AlvaCleanCRM.Models.DTOs;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace AlvaCleanCRM.Repositories
{
    public class EmployeerRepository : IEmployeerRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
 
        public EmployeerRepository(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Employeer>> GetAllEmployeers()
        {
            var response = await _httpClient.GetAsync("Admin/get-all-employeers");
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
            var responseLogin = await _httpClient.PostAsync("Auth/Login-employeers", jsonContent);
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
            var jsonContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            await _httpClient.PostAsync($"Admin/register-new-employeer", jsonContent);
        }


        public async Task<List<Order>> GetAllOrdersOfEmployeer(string employeerId)
        {
            var response = await _httpClient.GetAsync($"Order/get-all-orders-of-employeer/{employeerId}");
            var orders = await response.Content.ReadFromJsonAsync<List<Order>>();

            if (orders != null)
                return orders;
            
            return new List<Order>();
        }

        public async Task<List<Order>> GetAllOrders()
        {
            var response = await _httpClient.GetAsync($"Order/get-all-orders");

            var orders = await response.Content.ReadFromJsonAsync<List<Order>>();

            if (orders != null)
            {
                return orders;
            }

            return new List<Order>();
        }

        public async Task<Employeer> GetEmployeer(string id)
        {
            var response = await _httpClient.GetAsync($"Admin/get-employeer/{id}");
            return await response.Content.ReadFromJsonAsync<Employeer>();
        }

        public async Task<EmployeerToUpdateDto> GetEmployeerToUpdate(string id)
        {
            var response = await _httpClient.GetAsync($"Admin/get-employeer/{id}");

            var emp = await response.Content.ReadFromJsonAsync<Employeer>();

            if (emp.Image != null)
            {
                var stream = new MemoryStream(emp.Image);
                var fromFileImage = new FormFile(stream, 0, emp.Image.Length, "file", "Image")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "application/octet-stream"
                };

                var empToUpdateDto = new EmployeerToUpdateDto
                {
                    Id = emp.Id,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    PhoneNumber = emp.PhoneNumber,
                    Role = emp.Role,
                    Image = fromFileImage,
                    ImageId = emp.ImageId
                };

                return empToUpdateDto;
            }

            var empToUpdateDtoWithoutImage = new EmployeerToUpdateDto
            {
                Id = emp.Id,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                PhoneNumber = emp.PhoneNumber,
                Role = emp.Role,
                Image = null,
                ImageId = null
            };

            return empToUpdateDtoWithoutImage;
        }

        public async Task UpdateEmployeer(EmployeerToUpdateDto model)
        {
            var employeerDto = new EmployeerToUpdateDto 
            { 
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Role = model.Role
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(employeerDto), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync($"Admin/update-employeer/{model.Id}", jsonContent);

            //this block can anyway doing when block upper is failed. that bad example of code

            if (model.Image != null)
            {
                var employeer = await GetEmployeer(model.Id);

                if (employeer.Image != null) 
                {
                    await _httpClient.DeleteAsync($"Admin/delete-photo-of-employeer/{employeer.ImageId}");
                }

                var formData = new MultipartFormDataContent();

                var fileContent = new StreamContent(model.Image.OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.Image.ContentType);

                formData.Add(fileContent, "file", model.Image.FileName);

                formData.Add(new StringContent(model.Id), "employeerId");

                //var jsonContentImage = new StringContent(JsonSerializer.Serialize(formData), Encoding.UTF8, "application/json");

                await _httpClient.PostAsync($"Admin/add-photo-to-employeer/{model.Id}", formData);
            }
        }

        public async Task DeleteImageOfEmployeer(string employeerId)
        {
            if (employeerId != null)
            {
                await _httpClient.DeleteAsync($"Admin/delete-photo-of-employeer/{employeerId}");
            }
        }

        public async Task DeleteEmployeer(string id)
        {
            await _httpClient.DeleteAsync($"Admin/delete-employeer/{id}");
        }  
    }
}
