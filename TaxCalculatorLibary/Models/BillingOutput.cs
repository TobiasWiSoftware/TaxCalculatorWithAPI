using System.Reflection.Metadata;

namespace TaxCalculatorLibary.Models
{
    public class BillingOutput
    {
        public BillingInput BillingInput { get; set; }
        public decimal InsuranceSum { get; set; }
        public decimal InsuranceCareSum { get; set; }
        public decimal PensionSum { get; set; }
        public decimal UnimploymentSum { get; set; }
        public decimal InsurancesTotal { get => InsuranceSum + InsuranceCareSum + PensionSum + UnimploymentSum; }
        public decimal TaxedIncome { get => BillingInput.GrossIncome - InsurancesTotal; }
        public decimal TaxSum { get; set; }
        public decimal SolidaryTaxSum { get; set; }
        public decimal ChurchTaxSum { get; set; }
        public decimal TotalTaxSum { get => TaxSum + SolidaryTaxSum + ChurchTaxSum; }
        public decimal BorderTaxSet { get; set; }
        public decimal AvgTaxSet { get => TaxedIncome != 0 ? TotalTaxSum / TaxedIncome : 0m; }
        public decimal Transferamount { get => BillingInput.GrossIncome - InsurancesTotal - TotalTaxSum; }

        public BillingOutput()
        {
            BillingInput = new();
        }
        // Only for testing
        public BillingOutput(BillingInput bi, decimal insurancesum, decimal insurancecaresum, decimal pensionsum, decimal unimploymentsum, decimal taxsum, decimal solidarytaxsum, decimal churchtaxsum)
        {
            BillingInput = bi;
            InsuranceSum = insurancesum;
            InsuranceCareSum = insurancecaresum;
            PensionSum = pensionsum;
            UnimploymentSum = unimploymentsum;
            TaxSum = taxsum;
            SolidaryTaxSum = solidarytaxsum;
            ChurchTaxSum = churchtaxsum;
        }

        public Tuple<bool, string> CheckForTestingWithTolerance(BillingOutput test)
        {
            bool result = false;
            string dataString = string.Empty;
            decimal testToleranceTax = 60m;

            bool isInsuraceSumOk = InsuranceSum == test.InsuranceSum;
            bool isTaxSumOk = TotalTaxSum - test.TotalTaxSum < testToleranceTax;

            dataString = $"Testdata =>" +
                $"Year: {this.BillingInput.Year} " +
                $"TaxClass: {this.BillingInput.TaxClass} " +
                $"Church: {this.BillingInput.InChurch.ToString().PadLeft(5)} " +
                $"Age: {this.BillingInput.Age} " +
                $"Children: {this.BillingInput.HasChildren.ToString().PadLeft(5)}  " +
                $"ChildrenTaxCredit: {this.BillingInput.ChildTaxCredit.ToString().PadLeft(2)} " +
                $"FederalInsurance: {this.BillingInput.HasFederalInsurance.ToString().PadLeft(5)} " +
                $"IsuranceAdditionTotal: {this.BillingInput.InsuranceAdditionTotal}" +
                $"FederalPension: {this.BillingInput.HasFederalPension.ToString().PadLeft(5)}" +

                $"\n" +

                $"Outputdata =>";


            if (isInsuraceSumOk && isTaxSumOk)
            {
                result = true;
            }

            return new(result, dataString);
        }


    }
}
