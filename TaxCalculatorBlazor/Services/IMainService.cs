using TaxCalculatorLibary.Models;

namespace TaxCalculatorBlazor.Services
{
    public interface IMainService
    {
        public Task<BillingOutput> Calculation(BillingInput billingInput);

        public Task<Tuple<SocialSecurityRates, TaxInformation>> FetchSocialAndTaxData(int year);
    }
}
