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
        public async Task<TaxSet> GetTaxValue(int year, decimal value, bool inChurch)
        {
            TaxSet taxSet = new();
            // Return with taxed value, taxsum, solidary tax, church tax, borderTaxSum

            TaxInformation? taxInformation = await _dbrepository.GetTaxInformationAsync(year);


            if (taxInformation != null && taxInformation.TaxInformationSteps != null)
            {
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

                    decimal borderTax = 0m;

                    if (value - taxSetBase.StepAmount > 0)
                    {
                        borderTax = taxsetRestSum / (value - taxSetBase.StepAmount);
                    }
                    else
                    {
                        borderTax = (taxSetNext.TaxAmount - taxSetBase.TaxAmount) / (taxSetNext.StepAmount - taxSetBase.TaxAmount); 
                    }

                    taxSet = new(value, taxSetBaseSum + taxsetRestSum, 0, 0, borderTax);
                }
                else if (taxSetBase != null && taxSetNext == null) // when value over the max in table
                {
                    TaxInformationStep maxTable = taxSetBase;

                    decimal borderTax = taxInformation.MaxTaxLevel;
                    decimal sum = value;
                    decimal totalTax = Math.Round(maxTable.TaxAmount + (value - maxTable.StepAmount) * borderTax / 100, 0);

                    taxSet = new(Math.Round(sum), totalTax, 0, 0, borderTax);
                }

                // Check for solidary and church

                if (taxSet.TaxSum > taxInformation.MinLevelForSolidarityTax)
                {
                    // A approximate calculation for the solidary tax sliding zone
                    if (value < 102000)
                    {
                        taxSet = new(taxSet.TaxedValue, taxSet.TaxSum, (taxSet.TaxSum - taxInformation.MinLevelForSolidarityTax) * 0.119m, 0, taxSet.BorderTaxSum);
                    }
                    else
                    {
                        taxSet = new(taxSet.TaxedValue, taxSet.TaxSum, taxSet.TaxSum * taxInformation.SolidaryTaxRate / 100, 0, taxSet.BorderTaxSum);
                    }
                }

                if (inChurch)
                {
                    taxSet = new(taxSet.TaxedValue, taxSet.TaxSum, taxSet.SolidaryTax, taxSet.TaxSum * taxInformation.ChurchTaxRate / 100, taxSet.BorderTaxSum);
                }
            }
            return taxSet;
        }
    }
}
