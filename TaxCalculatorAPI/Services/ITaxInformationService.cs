using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Services
{
    public interface ITaxInformationService
    {
        Task MigrateDataFromJsonToDataBase(string dataDirectory);

    }
}
