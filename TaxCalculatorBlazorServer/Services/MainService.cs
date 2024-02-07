using System.Net.Http;
using System.Net.Http.Json;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorBlazorServer.Services
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
            var response = await _httpClient.PostAsJsonAsync<BillingInput>("api/Main/TransferAmount", billingInput);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<BillingOutput>();
            }
            throw new Exception("Fehler bei der API-Anfrage für Calculation");
        }

        public async Task<SocialSecurityRates?> FetchSocialSecurityRates(int year)
        {
            var response = await _httpClient.PostAsJsonAsync<int>("api/Main/TransferSocialSecurityRates", year);
            if (response != null && response.Content != null && response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SocialSecurityRates>();
            }
            throw new Exception("Fehler bei der API-Anfrage für FetchSocialAndTaxData");
        }

        public async Task<TaxInformation?> FetchTaxInformation(int year)
        {
            var response = await _httpClient.PostAsJsonAsync<int>("api/Main/TransferTaxInformation", year);
            if (response != null && response.Content != null && response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TaxInformation>();
            }
            throw new Exception("Fehler bei der API-Anfrage für FetchSocialAndTaxData");
        }


        public async Task<int> IncrementVisitCounter()
        {
            var response = await _httpClient.PostAsJsonAsync("api/Main/IncrementVisitCounter", "");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<int>();
            }
            throw new Exception("Fehler bei der API-Anfrage für IncrementVisitCounter");
        }

    }
}
