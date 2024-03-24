using Microsoft.EntityFrameworkCore;
using TaxCalculatorAPI.Data;
using TaxCalculatorAPI.Repository;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Services
{
    public class TaxInformationService : ITaxInformationService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IDataBaseRepository _dbrepository;
        public TaxInformationService(IFileRepository fileRepository, IDataBaseRepository dbrepository)
        {
            _fileRepository = fileRepository;
            _dbrepository = dbrepository;
        }
        public async Task MigrateDataFromJsonToDataBase(string dataDirectory)
        {
            List<TaxInformation>? list = await _fileRepository.GetTaxInformationAsync(dataDirectory);

            if (list != null)
            {
                foreach (var item in list)
                {
                    await _dbrepository.AddTaxInformationAsync(item);
                }
            }
        }

    }
}
