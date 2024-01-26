using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Repository
{
    public interface IFileRepository
    {
        public Task<List<SocialSecurityRates>?> GetSocialSecurityRatesAsync(string dataDirectory);
        public Task<List<TaxInformation>?> GetTaxInformationAsync(string dataDirectory);
    }
}
