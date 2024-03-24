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
            _dbcontext.TaxInformations.Add(taxinformation);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<SocialSecurityRates?> GetSocialSecurityRatesAsync(int year)
        {
            return await _dbcontext.SocialSecurityRates.FirstOrDefaultAsync(x => x.Year == year);
        }

        public async Task<TaxInformation?> GetTaxInformationAsync(int year)
        {
            return await _dbcontext.TaxInformations.Include(ti => ti.TaxInformationSteps).FirstOrDefaultAsync(x => x.Year == year);
        }

        public async Task<int> GetVisitCounter()
        {
            return await _dbcontext.Trackings.CountAsync();
        }

        public async Task IncrementVisitCounter()
        {
            await _dbcontext.Trackings.AddAsync(new Tracking());
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteVisitCounter()
        {
            _dbcontext.Trackings.RemoveRange(_dbcontext.Trackings);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
