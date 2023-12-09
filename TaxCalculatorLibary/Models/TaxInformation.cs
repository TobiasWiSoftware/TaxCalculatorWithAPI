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
        public int TaxFreeChildGrowingFlat { get; set; } // Class 2
        public int TaxFreeChildFlat { get; set; } // 1 - 4 in 4 * 0.5
        public decimal MaxTaxLevel { get; set; }
        public decimal MinLevelForSolidarityTax { get; set; }
        public decimal SolidaryTaxRate { get; set; }
        public decimal ChurchTaxRate { get; set; }
        public List<Tuple<decimal, decimal>>? TaxLevels { get; set; }

        public TaxInformation()
        {

        }

        public TaxInformation(int year, int taxFreeBasic, List<Tuple<decimal, decimal>> taxLevels, int taxFreeBasicFlat, int taxFreeEmployeeFlat,
                                   int taxFreeChildGrowingFlat, int taxFreeChildFlat)
        {
            Year = year;
            TaxLevels = taxLevels;
            TaxFreeBasicFlat = taxFreeBasicFlat;
            TaxFreeEmployeeFlat = taxFreeEmployeeFlat;
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
        
        public static void LoadDataFromJsonForTesting()
        {
            string path = Path.Combine("../../../../", "TaxCalculatorAPI", "Data", "TaxInformation.json");
            using (StreamReader r = new(path))
            {
                string s = r.ReadToEnd();
                List<TaxInformation>? list = JsonConvert.DeserializeObject<List<TaxInformation>>(s);

                if (list != null)
                {
                    LTaxInformation = list;
                }
            }
        }


        public Tuple<decimal, decimal, decimal, decimal, decimal> GetTaxValue(decimal value, bool inChurch)
        {
            // Return a tuple with taxed value, taxsum, solidary tax, church tax, borderTaxSum

            Tuple<decimal, decimal, decimal, decimal, decimal>? taxSet = new Tuple<decimal, decimal, decimal, decimal, decimal>(0, 0,0,0,0);

            // Find the last tax rates
            Tuple<decimal, decimal>? taxSetBase = TaxLevels.FindAll(x => x.Item1 <= value).Max();

            //Find the next tax rates 
            Tuple<decimal, decimal>? taxSetNext = TaxLevels.FindAll(x => x.Item1 > value).Min();

            if (taxSetBase != null && taxSetNext != null) // case when beetween tax table
            {
                // Calculation for the last known border tax zone in sum + rest of the value in the folowing tax zone
                decimal taxSetBaseSum = taxSetBase.Item2;
                decimal taxsetRestSum = (value - taxSetBase.Item1) * (taxSetNext.Item2 - taxSetBase.Item2) / (taxSetNext.Item1 - taxSetBase.Item1);
                decimal borderTax = taxsetRestSum / (value - taxSetBase.Item1);

                taxSet = new Tuple<decimal, decimal, decimal, decimal, decimal>(value, taxSetBaseSum + taxsetRestSum,0,0, borderTax );
            }
            else if(taxSetBase != null && taxSetNext == null) // when value over the max in table
            {
                Tuple<decimal, decimal> maxTable = taxSetBase;

                decimal borderTax = MaxTaxLevel;
                decimal sum = value;
                decimal totalTax = Math.Round(maxTable.Item2 + (value - maxTable.Item1) * borderTax / 100, 0);

                taxSet = new Tuple<decimal, decimal, decimal, decimal, decimal>(Math.Round(sum), totalTax,0,0, borderTax);
            }

            // Check for solidary and church

            if (taxSet.Item2 > this.MinLevelForSolidarityTax)
            {
                taxSet = new(taxSet.Item1, taxSet.Item2, (taxSet.Item2 - this.MinLevelForSolidarityTax) * this.SolidaryTaxRate / 100, 0, taxSet.Item5);
            }

            if (inChurch)
            {
                taxSet = new(taxSet.Item1, taxSet.Item2, taxSet.Item3, taxSet.Item2 * this.ChurchTaxRate / 100, taxSet.Item5);
            }

            return taxSet;
        }
        public static TaxInformation? GetDataFromYear(int year)
        {
            return LTaxInformation != null ? LTaxInformation.Find(x => x.Year == year) : null;
        }
    }

}
