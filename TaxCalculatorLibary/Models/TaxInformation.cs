using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace TaxCalculatorLibary.Models
{

    public class TaxInformation
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int TaxFreeBasicFlat { get; set; } // Class 1 - 4
        public int TaxFreeEmployeeFlat { get; set; } // For class 1 - 5
        public int TaxFreeChildGrowingFlat { get; set; } // Class 2
        public int TaxFreeChildFlat { get; set; } // 1 - 4 in 4 * 0.5
        public decimal MaxTaxLevel { get; set; }
        public decimal MinLevelForSolidarityTax { get; set; }
        public decimal SolidaryTaxRate { get; set; }
        public decimal ChurchTaxRate { get; set; }
        public List<TaxInformationStep>? TaxInformationSteps { get; set; }
        public TaxInformation()
        {
            
        }

        public TaxInformation(int year, int taxFreeBasic, List<TaxInformationStep> taxInformationsSteps, int taxFreeBasicFlat, int taxFreeEmployeeFlat,
                                   int taxFreeChildGrowingFlat, int taxFreeChildFlat)
        {
            Year = year;
            TaxInformationSteps = taxInformationsSteps;
            TaxFreeBasicFlat = taxFreeBasicFlat;
            TaxFreeEmployeeFlat = taxFreeEmployeeFlat;
            TaxFreeChildGrowingFlat = taxFreeChildGrowingFlat;
            TaxFreeChildFlat = taxFreeChildFlat;
        }

    }
}
