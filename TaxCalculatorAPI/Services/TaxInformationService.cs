using Microsoft.EntityFrameworkCore;
using TaxCalculatorAPI.Data;
using TaxCalculatorAPI.Repository;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Services
{
    public class TaxInformationService : ITaxInformationService
    {
        private readonly IFileRepository _fileRepository;
        private readonly ApplicationDBContext _dbcontext;
        public TaxInformationService(IFileRepository fileRepository, ApplicationDBContext dbcontext)
        {
            _fileRepository = fileRepository;
            _dbcontext = dbcontext;
        }
        public async Task MigrateDataFromJsonToDataBase(string dataDirectory)
        {
            List<TaxInformation>? list = await _fileRepository.GetTaxInformationAsync(dataDirectory);

            if (list != null)
            {
                foreach (var item in list)
                {
                    await _dbcontext.TaxInformation.AddAsync(item);

                    if (item.TaxInformationSteps != null)
                    {
                        foreach (var step in item.TaxInformationSteps)
                        {
                            await _dbcontext.TaxInformationStep.AddAsync(step);
                        }
                    }
                }
                await _dbcontext.SaveChangesAsync();
            }
        }
        public async Task<Tuple<decimal, decimal, decimal, decimal, decimal>> GetTaxValue(int year, decimal value, bool inChurch)
        {
            Tuple<decimal, decimal, decimal, decimal, decimal> taxSet = new Tuple<decimal, decimal, decimal, decimal, decimal>(0, 0, 0, 0, 0);
            // Return a tuple with taxed value, taxsum, solidary tax, church tax, borderTaxSum

            TaxInformation? taxInformation = await _dbcontext.TaxInformation.Include(ti => ti.TaxInformationSteps).FirstOrDefaultAsync(x => x.Year == year);


            if (taxInformation != null && taxInformation.TaxInformationSteps != null)
            {
                taxSet = new Tuple<decimal, decimal, decimal, decimal, decimal>(0, 0, 0, 0, 0);

                TaxInformationStep? taxSetBase = null;
                TaxInformationStep? taxSetNext = null;


                try
                {
                    // Find the last tax rates
                    taxSetBase = taxInformation.TaxInformationSteps.FindAll(x => x.StepAmount <= value).MaxBy(t => t.StepAmount);

                    //Find the next tax rates 
                    taxSetNext = taxInformation.TaxInformationSteps.FindAll(x => x.StepAmount > value).MinBy(t => t.StepAmount);
                }
                catch (Exception)
                {

                    throw;
                }




                if (taxSetBase != null && taxSetNext != null) // case when beetween tax table
                {
                    // Calculation for the last known border tax zone in sum + rest of the value in the folowing tax zone
                    decimal taxSetBaseSum = taxSetBase.TaxAmount;
                    decimal taxsetRestSum = (value - taxSetBase.StepAmount) * (taxSetNext.TaxAmount - taxSetBase.TaxAmount) / (taxSetNext.StepAmount - taxSetBase.StepAmount);
                    decimal borderTax = taxsetRestSum / (value - taxSetBase.StepAmount);

                    taxSet = new Tuple<decimal, decimal, decimal, decimal, decimal>(value, taxSetBaseSum + taxsetRestSum, 0, 0, borderTax);
                }
                else if (taxSetBase != null && taxSetNext == null) // when value over the max in table
                {
                    TaxInformationStep maxTable = taxSetBase;

                    decimal borderTax = taxInformation.MaxTaxLevel;
                    decimal sum = value;
                    decimal totalTax = Math.Round(maxTable.TaxAmount + (value - maxTable.StepAmount) * borderTax / 100, 0);

                    taxSet = new Tuple<decimal, decimal, decimal, decimal, decimal>(Math.Round(sum), totalTax, 0, 0, borderTax);
                }

                // Check for solidary and church

                if (taxSet.Item2 > taxInformation.MinLevelForSolidarityTax)
                {
                    // A approximate calculation for the solidary tax sliding zone
                    if (value < 102000)
                    {
                        taxSet = new(taxSet.Item1, taxSet.Item2, (taxSet.Item2 - taxInformation.MinLevelForSolidarityTax) * 0.119m, 0, taxSet.Item5);
                    }
                    else
                    {
                        taxSet = new(taxSet.Item1, taxSet.Item2, taxSet.Item2 * taxInformation.SolidaryTaxRate / 100, 0, taxSet.Item5);
                    }
                }

                if (inChurch)
                {
                    taxSet = new(taxSet.Item1, taxSet.Item2, taxSet.Item3, taxSet.Item2 * taxInformation.ChurchTaxRate / 100, taxSet.Item5);
                }
            }
            return taxSet;
        }
    }
}
