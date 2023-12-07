using System.ComponentModel.DataAnnotations;

namespace TaxCalculatorLibary.Models
{
    public class BillingInput
    {
        [Required(ErrorMessage = "Eingabe ist falsch")]
        [Range(2010, 2030, ErrorMessage = "Jahr zwischen 2010 und 2030 zulässig")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Eingabe ist falsch")]
        [Range(0, 10000000, ErrorMessage = "Jahr zwischen 0 und 10000000 zulässig")]
        public decimal GrossIncome { get; set; }

        [Required(ErrorMessage = "Eingabe ist falsch")]
        public bool BillingPeriodMonthly { get; set; }

        [Required(ErrorMessage = "Eingabe ist falsch")]
        public int TaxClass { get; set; }

        [Required(ErrorMessage = "Eingabe ist falsch")]
        public bool InChurch { get; set; }

        [Required(ErrorMessage = "Eingabe ist falsch")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Eingabe ist falsch")]
        public bool HasChildren { get; set; } = false;

        [Required(ErrorMessage = "Eingabe ist falsch")]
        public decimal ChildTaxCredit { get; set; }

        [Required(ErrorMessage = "Eingabe ist falsch")]
        public string HasFederalInsurance { get; set; }
        [Required(ErrorMessage = "Eingabe ist falsch")]
        public decimal? PrivateInsurance { get; set; }
        [Required(ErrorMessage = "Eingabe ist falsch")]
        public decimal? InsuranceAdditionTotal { get; set; }
        [Required(ErrorMessage = "Eingabe ist falsch")]
        public string HasFederalPension { get; set; }

        [Required(ErrorMessage = "Eingabe ist falsch")]
        public string HasFederalUnimployment { get; set; }

        public BillingInput()
        {

        }

        public BillingInput(int year, decimal gross, bool isMonthly, int taxclass, int age, bool hasChildren, decimal childTaxCredit, string hasFederalInsurance, decimal privateInsuarance, decimal? insuranceAdditionTotal, string hasFederalPension, string hasFederalUnimployment)
        {
            Year = year;
            GrossIncome = gross;
            BillingPeriodMonthly = isMonthly;
            TaxClass = taxclass;
            Age = age;
            HasChildren = hasChildren;
            ChildTaxCredit = childTaxCredit;
            HasFederalInsurance = hasFederalInsurance;
            PrivateInsurance = privateInsuarance;
            if (hasFederalInsurance == "true" && insuranceAdditionTotal != null)
            {
                InsuranceAdditionTotal = insuranceAdditionTotal;
            }
            HasFederalPension = hasFederalPension;
            HasFederalUnimployment = hasFederalUnimployment;
        }
    }
}
