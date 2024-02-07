
using Microsoft.EntityFrameworkCore;
using TaxCalculatorAPI.Data;
using TaxCalculatorAPI.Repository;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Services
{
    public class SocialSecurityService : ISocialSecurityService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IDataBaseRepository _dbrepository;
        public SocialSecurityService(IFileRepository fileRepository, IDataBaseRepository dbrepository)
        {
            _fileRepository = fileRepository;
            _dbrepository = dbrepository;
        }
        public async Task MigrateDataFromJsonToDataBase(string dataDirectory)
        {
            List<SocialSecurityRates>? socialSecurityRates = await _fileRepository.GetSocialSecurityRatesAsync(dataDirectory);
            if (socialSecurityRates != null)
            {
                foreach (var item in socialSecurityRates)
                {
                    await _dbrepository.AddSocialSecurityRatesAsync(item);
                }
            }

        }
    }
}
