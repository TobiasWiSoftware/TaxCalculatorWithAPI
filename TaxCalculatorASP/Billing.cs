using TaxCalculatorASP;

namespace TaxCalculatorASP
{
    public class BillingOutput
    {
        public SocialSecurityRates? SocialSecurityRatesOfYear { get; set; }
        public TaxInformation? TaxInformationOfYear { get; set; }

        public decimal InsuranceSum { get; set; }
        public decimal InsuranceCareSum { get; set; }
        public decimal PensionSum { get; set; }
        public decimal UnimploymentSum { get; set; }
        public decimal InsurancesTotal { get => InsuranceSum + InsuranceCareSum + PensionSum + UnimploymentSum; }
        public decimal TaxedIncome { get; set; }
        public decimal TaxSum { get; set; }
        public decimal BorderTaxSet { get; set; }
        public decimal AvgTaxSet { get; set; }


        public BillingOutput()
        {

        }

    }
}
