using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Services
{
    public interface IMainControllerService
    {
        Task<BillingOutput> Calculation(BillingInput billingInput);
        Tuple<SocialSecurityRates?, TaxInformation?> FetchSocialAndTaxData(int year);
        int IncrementVisitCounter();
    }
}
