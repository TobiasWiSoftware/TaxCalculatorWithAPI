using System.Net.Http;
using System.Net.Http.Json;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorBlazor.Services
{
    public class MainService : IMainService
    {
        private readonly HttpClient _httpClient;

        public MainService(HttpClient httpClienent)
        {
            _httpClient = httpClienent;
        }

        public async Task<BillingOutput> Calculation(BillingInput billingInput)
        {
            var response = await _httpClient.PostAsJsonAsync<BillingInput>("api/Main", billingInput);
            return await response.Content.ReadFromJsonAsync<BillingOutput>();
        }

    }
}
