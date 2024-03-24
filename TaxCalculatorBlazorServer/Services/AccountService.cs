using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorBlazorServer.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> RegisterAsync(RegisterModel model)
        {
            var response = await _httpClient.PostAsJsonAsync<RegisterModel>("api/Account/Register", model);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            var errorContent = await response.Content.ReadAsStringAsync();
      
            // Loggen Sie den Fehler oder werfen Sie eine Ausnahme mit den Fehlerdetails
            throw new Exception($"Fehler bei der API-Anfrage für Register: {errorContent}");
        }

        public async Task<bool> LoginAsync(LoginModel model)
        {
            var response = await _httpClient.PostAsJsonAsync<LoginModel>("api/Account/Login", model);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            throw new Exception("Error in API call login");
        }
    }
}
