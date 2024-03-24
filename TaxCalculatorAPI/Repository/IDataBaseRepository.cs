using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Repository
{
    public interface IDataBaseRepository
    {
        Task AddSocialSecurityRatesAsync(SocialSecurityRates rates);
        Task AddTaxInformationAsync(TaxInformation taxinformation);
        Task<SocialSecurityRates?> GetSocialSecurityRatesAsync(int year);
        Task<TaxInformation?> GetTaxInformationAsync(int year);
        Task<int> GetVisitCounter();
        Task IncrementVisitCounter();
        Task DeleteVisitCounter();
    }
}
