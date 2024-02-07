using Microsoft.EntityFrameworkCore;
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
        public async Task AddSocialSecurityRatesAsync(SocialSecurityRates rates)
        {
            _dbcontext.SocialSecurityRates.Add(rates);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task AddTaxInformationAsync(TaxInformation taxinformation)
        {
            _dbcontext.TaxInformation.Add(taxinformation);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<SocialSecurityRates?> GetSocialSecurityRatesAsync(int year)
        {
            return await _dbcontext.SocialSecurityRates.FirstOrDefaultAsync(x => x.Year == year);
        }

        public async Task<TaxInformation?> GetTaxInformationAsync(int year)
        {
            return await _dbcontext.TaxInformation.Include(ti => ti.TaxInformationSteps).FirstOrDefaultAsync(x => x.Year == year);
        }
    }
}
