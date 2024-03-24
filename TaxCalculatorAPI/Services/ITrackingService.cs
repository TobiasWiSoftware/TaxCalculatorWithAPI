using Microsoft.AspNetCore.Mvc;

namespace TaxCalculatorAPI.Services
{
    public interface ITrackingService
    {
        Task<int> GetVisitCounter();
        Task<bool> IncrementVisitCounter();
        Task<bool> DeleteVisitCounter();
    }
}
