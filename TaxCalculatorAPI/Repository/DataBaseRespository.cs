using TaxCalculatorAPI.Data;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Repository
{
    public class DataBaseRespository : IDataBaseRepository
    {
        private readonly ApplicationDBContext _dbcontext;
        public DataBaseRespository(ApplicationDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task AddSocialSecurityRates(SocialSecurityRates rates)
        {
            _dbcontext.SocialSecurityRates.Add(rates);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task AddTaxInformation(TaxInformation taxinformation)
        {
            _dbcontext.TaxInformation.Add(taxinformation);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
