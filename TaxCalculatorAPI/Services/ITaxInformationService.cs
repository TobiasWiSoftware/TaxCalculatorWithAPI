using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Services
{
    public interface ITaxInformationService
    {
        Task MigrateDataFromJsonToDataBase(string dataDirectory);
        Task<TaxSet> GetTaxValue(int year, decimal value, bool inChurch);
    }
}
