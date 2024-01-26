using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Services
{
    public interface ISocialSecurityService
    {
        Task MigrateDataFromJsonToDataBase(string dataDirectory);
    }
}
