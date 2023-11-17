using TaxCalculatorLibary.Models;

namespace TaxCalculatorBlazor.Services
{
    public interface IMainService
    {
        public Task<BillingOutput> Calculation(BillingInput billingInput);
    }
}
