
using Microsoft.EntityFrameworkCore;
using TaxCalculatorAPI.Data;
using TaxCalculatorAPI.Repository;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Services
{
    public class SocialSecurityService : ISocialSecurityService
    {
        private readonly IFileRepository _fileRepository;
        private readonly ApplicationDBContext _dbcontext;
        public SocialSecurityService(IFileRepository fileRepository, ApplicationDBContext dbcontext)
        {
            _fileRepository = fileRepository;
            _dbcontext = dbcontext;
        }
        public async Task MigrateDataFromJsonToDataBase(string dataDirectory)
        {
            List<SocialSecurityRates>? socialSecurityRates = await _fileRepository.GetSocialSecurityRatesAsync(dataDirectory);
            if (socialSecurityRates != null)
            {
                foreach (var item in socialSecurityRates)
                {
                    await _dbcontext.SocialSecurityRates.AddAsync(item);
                }
                await _dbcontext.SaveChangesAsync();
            }

        }
    }
}
