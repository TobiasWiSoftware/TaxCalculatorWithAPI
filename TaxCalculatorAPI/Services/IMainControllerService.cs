using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Services
{
    public interface IMainControllerService
    {
        Task<TaxSet> GetTaxValue(int year, decimal value, bool inChurch);
        Task<BillingOutput> Calculation(BillingInput billingInput);
        Task<SocialSecurityRates?> FetchSocialSecurityRates(int year);
        Task<TaxInformation?> FetchTaxInformation(int year);
 
    }
}
