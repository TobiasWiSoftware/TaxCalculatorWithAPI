using TaxCalculatorLibary.Models;

namespace TaxCalculatorBlazorServer.Services
{
    public interface IMainService
    {
        public Task<BillingOutput> Calculation(BillingInput billingInput);

        public Task<SocialSecurityRates> FetchSocialSecurityRates(int year);

        public Task<TaxInformation> FetchTaxInformation(int year);

        public Task<int> IncrementVisitCounter();
    }
}
