namespace TaxCalculatorAPI.Services
{
    public interface ITaxInformationService
    {
        Task MigrateDataFromJsonToDataBase(string dataDirectory);
        Task<Tuple<decimal, decimal, decimal, decimal, decimal>> GetTaxValue(int year, decimal value, bool inChurch);
    }
}
