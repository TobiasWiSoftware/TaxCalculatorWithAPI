using TaxCalculatorASP;

namespace TaxCalculatorAPI.Services
{
    public interface IMainService
    {
        BillingOutput Calculation(BillingInput billingInput);
    }
}
