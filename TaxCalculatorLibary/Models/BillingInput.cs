namespace TaxCalculatorLibary.Models
{
    public class BillingInput
    {
        public int Year { get; set; }
        public decimal GrossIncome { get; set; }
        public bool BillingPeriod { get; set; }
        public int TaxClass { get; set; }
        public decimal TaxFree { get; set; } = 0;
        public bool InChurch { get; set; }
        public string FederalState { get; set; }
        public int Age { get; set; }
        public bool HasChildren { get; set; } = false;
        public decimal ChildTaxCredit { get; set; }
        public bool HasFederalInsurance { get; set; }
        public decimal? PrivateInsurance { get; set; }
        public decimal? InsuranceAdditionTotal { get; set; }
        public bool HasFederalPension { get; set; }
        public bool HasFederalUnimployment { get; set; }

        public BillingInput()
        {

        }

        public BillingInput(int year, decimal gross, bool isMonthly, int taxclass, string federalState, int age, bool hasChildren, decimal childTaxCredit, bool hasFederalInsurance, decimal? insuranceAdditionTotal, bool hasFederalPension, bool hasFederalUnimployment)
        {
            Year = year;
            GrossIncome = gross;
            BillingPeriod = isMonthly;
            TaxClass = taxclass;
            FederalState = federalState;
            Age = age;
            HasChildren = hasChildren;
            ChildTaxCredit = childTaxCredit;
            HasFederalInsurance = hasFederalInsurance;
            if (hasFederalInsurance && insuranceAdditionTotal != null)
            {
                InsuranceAdditionTotal = insuranceAdditionTotal;
            }
            HasFederalPension = hasFederalPension;
            HasFederalUnimployment = hasFederalUnimployment;
        }
    }
}
