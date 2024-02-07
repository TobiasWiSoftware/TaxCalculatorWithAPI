using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Services
{
    public interface IMainControllerService
    {
        Task<BillingOutput> Calculation(BillingInput billingInput);
        SocialSecurityRates? FetchSocialSecurityRates(int year);
        TaxInformation? FetchTaxInformation(int year);
        int IncrementVisitCounter();
    }
}
