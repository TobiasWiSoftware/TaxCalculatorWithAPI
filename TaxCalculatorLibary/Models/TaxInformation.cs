using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace TaxCalculatorLibary.Models
{

    public class TaxInformation
    {
        private static List<TaxInformation>? LTaxInformation = null;
        public int Year { get; set; }
        public int TaxFreeBasicFlat { get; set; } // Class 1 - 4
        public int TaxFreeEmployeeFlat { get; set; } // For class 1 - 5
        public int SpecialCostFlat { get; set; } // For class 1 - 5
        public int TaxFreeChildGrowingFlat { get; set; } // Class 2
        public int TaxFreeChildFlat { get; set; } // 1 - 4 in 4 * 0.5
        public List<Tuple<decimal, decimal, decimal>>? TaxLevels { get; set; }

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

        public static void LoadDataFromJson(string dataDirectory)
        {
            using (StreamReader r = new(Path.Combine(dataDirectory, "Data", "TaxInformation.json")))
            {
                string s = r.ReadToEnd();
                List<TaxInformation>? list = JsonConvert.DeserializeObject<List<TaxInformation>>(s);

                if (list != null)
                {
                    LTaxInformation = list;
                }
            }
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
        public static TaxInformation? GetDataFromYear(int year)
        {
            return LTaxInformation != null ? LTaxInformation.Find(x => x.Year == year) : null;
        }
        public static Tuple<int, int> YearRange()
        {
            if (LTaxInformation != null)
                return new Tuple<int, int>(LTaxInformation.Min(x => x.Year), LTaxInformation.Max(x => x.Year));
            else
                return new Tuple<int, int>(DateTime.Now.Year, DateTime.Now.Year);

        }


    }

}
