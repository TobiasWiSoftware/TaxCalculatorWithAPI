using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Services
{
    public interface IMainService
    {
        BillingOutput Calculation(BillingInput billingInput);
    }
}
