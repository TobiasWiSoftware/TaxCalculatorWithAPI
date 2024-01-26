using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Repository
{
    public interface IDataBaseRepository
    {
        Task AddSocialSecurityRates(SocialSecurityRates rates);
        Task AddTaxInformation(TaxInformation taxinformation);
    }
}
