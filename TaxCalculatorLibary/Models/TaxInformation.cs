using System.Collections.Generic;
namespace TaxCalculatorLibary.Models
{

    public class TaxInformation
    {
        private static List<TaxInformation>? _taxInformation = null;
        public int Year { get; set; }
        public int TaxFreeBasicFlat { get; set; } // Class 1 - 4
        public int TaxFreeEmployeeFlat { get; set; } // For class 1 - 5
        public int SpecialCostFlat { get; set; } // For class 1 - 5
        public int TaxFreeChildGrowingFlat { get; set; } // Class 2
        public int TaxFreeChildFlat { get; set; } // 1 - 4 in 4 * 0.5
        public List<Tuple<decimal, decimal, decimal>> TaxLevels { get; set; }

        public TaxInformation()
        {

        }

        public TaxInformation(int year, int taxFreeBasic, List<Tuple<decimal, decimal, decimal>> taxLevels, int taxFreeBasicFlat, int taxFreeEmployeeFlat, int specialCostFlat,
                                   int taxFreeChildGrowingFlat, int taxFreeChildFlat)
        {
            Year = year;
            TaxLevels = taxLevels;
            TaxFreeBasicFlat = taxFreeBasicFlat;
            TaxFreeEmployeeFlat = taxFreeEmployeeFlat;
            SpecialCostFlat = specialCostFlat;
            TaxFreeChildGrowingFlat = taxFreeChildGrowingFlat;
            TaxFreeChildFlat = taxFreeChildFlat;
        }

        public static void SetList(List<TaxInformation>? ti)
        {
            if (_taxInformation == null)
                _taxInformation ??= ti;
        }

        public static List<TaxInformation>? GetList()
        {
            return _taxInformation;
        }
        public Tuple<decimal, decimal, decimal>? GetTaxValue(decimal value)
        {
            

            Tuple<decimal, decimal, decimal>? taxSet = TaxLevels.FindAll(x => x.Item2 <= value).Max();

            if (value < TaxLevels.Max(x => x.Item2))
            {
                taxSet = new Tuple<decimal, decimal, decimal>(taxSet.Item1, value, taxSet.Item3 + (value - taxSet.Item2) * taxSet.Item1 / 100);
            }
            else
            {
                Tuple<decimal, decimal, decimal> maxTable = TaxLevels.Find(x => x.Item2 == TaxLevels.Max(y => x.Item2));

                decimal borderTax = maxTable.Item1;
                decimal sum = value;
                decimal totalTax = (int)Math.Round(maxTable.Item3 + (value - maxTable.Item2) * borderTax / 100, 0);

                taxSet = new Tuple<decimal, decimal, decimal>(borderTax, (int)Math.Round(sum), totalTax);
            }

            return taxSet;
        }
    }

}
