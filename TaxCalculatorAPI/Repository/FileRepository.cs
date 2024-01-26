using Newtonsoft.Json;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Repository
{
    public class FileRepository : IFileRepository
    {
        public async Task<List<SocialSecurityRates>?> GetSocialSecurityRatesAsync(string dataDirectory)
        {
            List<SocialSecurityRates>? list = new();
            using (StreamReader r = new(Path.Combine(dataDirectory, "Data", "SocialSecurityRates.json")))
            {
                string s = r.ReadToEnd();
                list = JsonConvert.DeserializeObject<List<SocialSecurityRates>>(s);

                await r.ReadToEndAsync();

            }
            return list;
        }

        public async Task<List<TaxInformation>?> GetTaxInformationAsync(string dataDirectory)
        {
            using (StreamReader r = new(Path.Combine(dataDirectory, "Data", "TaxInformation.json")))
            {
                string s = r.ReadToEnd();
                List<TaxInformation>? list = JsonConvert.DeserializeObject<List<TaxInformation>>(s);

                await r.ReadToEndAsync();
                return list;
            }
        }
    }
}
