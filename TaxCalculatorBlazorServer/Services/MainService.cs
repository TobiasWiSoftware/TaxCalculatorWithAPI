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

        public async Task<Tuple<SocialSecurityRates, TaxInformation>> FetchSocialAndTaxData(int year)
        {
            var response = await _httpClient.PostAsJsonAsync<int>("api/Main/TransferInput", year);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Tuple<SocialSecurityRates, TaxInformation>>();
            }
            throw new Exception("Fehler bei der API-Anfrage für FetchSocialAndTaxData");
        }

        public async Task<bool> IncrementVisitCounter()
        {
            var response = await _httpClient.PostAsJsonAsync("api/Main/IncrementVisitCounter", "");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            throw new Exception("Fehler bei der API-Anfrage für IncrementVisitCounter");
        }

    }
}
