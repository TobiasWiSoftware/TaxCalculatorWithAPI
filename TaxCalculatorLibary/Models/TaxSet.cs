using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculatorLibary.Models
{
    // Return with taxed value, taxsum, solidary tax, church tax, borderTaxSum
    public class TaxSet
    {
        public decimal TaxedValue { get; set; }
        public decimal TaxSum { get; set; }
        public decimal SolidaryTax { get; set; }
        public decimal ChurchTax { get; set; }
        public decimal BorderTaxSum { get; set; }
        public TaxSet()
        {

        }
        public TaxSet(decimal taxedValue, decimal taxSum, decimal solidaryTax, decimal churchTax, decimal borderTaxSum)
        {
            TaxedValue = taxedValue;
            TaxSum = taxSum;
            SolidaryTax = solidaryTax;
            ChurchTax = churchTax;
            BorderTaxSum = borderTaxSum;
        }
    }
}
