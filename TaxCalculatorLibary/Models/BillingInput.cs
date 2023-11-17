namespace TaxCalculatorLibary.Models
{
    public class BillingInput
    {
        public int Year { get; set; }
        public decimal GrossIncome { get; set; }
        public string? BillingPeriod { get; set; }
        public int TaxClass { get; set; }
        public decimal TaxFree { get; set; } = 0;
        public bool InChurch { get; set; }
        public string? FederalState { get; set; }
        public int? Age { get; set; }
        public bool HasChildren { get; set; } = false;
        public decimal ChildTaxCredit { get; set; }
        public string? Insurance { get; set; }

        public decimal InsuranceAdditionTotal { get; set; }
        public string? Pension { get; set; }
        public string? Unimployment { get; set; }

        public BillingInput()
        {

        }
    }
}
