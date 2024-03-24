using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxCalculatorAPI.Data;
using TaxCalculatorAPI.Repository;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Services
{
    public class TrackingService : ITrackingService
    {
        private readonly IDataBaseRepository _dataBaseRepository;
        public TrackingService(IDataBaseRepository dataBaseRepository)
        {
            _dataBaseRepository = dataBaseRepository;
        }
        public async Task<int> GetVisitCounter()
        {
            return await _dataBaseRepository.GetVisitCounter();
        }
        public async Task<bool> IncrementVisitCounter()
        {
            try
            {
                await _dataBaseRepository.IncrementVisitCounter();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> DeleteVisitCounter()
        {
            try
            {
                await _dataBaseRepository.DeleteVisitCounter();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
