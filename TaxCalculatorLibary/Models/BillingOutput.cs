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
        public decimal AvgTaxSet { get => TotalTaxSum / TaxedIncome; }
        public decimal Transferamount { get => BillingInput.GrossIncome - InsurancesTotal - TotalTaxSum; }
        public TaxInformation? TaxInformation { get => TaxInformation.GetDataFromYear(DateTime.Now.Year); }
        public SocialSecurityRates? SocialSecurityRates { get => SocialSecurityRates.GetDataFromYear(DateTime.Now.Year); }



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

        public void Calculation(BillingInput billingInput)
        {

            BillingInput = billingInput;


            decimal contributionrate = 0.00m;
            decimal maxGross = 0;

            if (BillingInput.HasFederalInsurance == "true" && SocialSecurityRates != null)
            {
                contributionrate = SocialSecurityRates.EmployeeInsuranceRate + this.BillingInput.InsuranceAdditionTotal / 2;
                maxGross = this.BillingInput.GrossIncome * 12 < SocialSecurityRates.InsuranceMaxGross ? this.BillingInput.GrossIncome : SocialSecurityRates.InsuranceMaxGross / 12;

                InsuranceSum = Math.Round(maxGross * contributionrate / 100, 2);

                contributionrate = SocialSecurityRates.EmployeeInsuranceCareRate;

                if (!this.BillingInput.HasChildren)
                {
                    contributionrate += SocialSecurityRates.EmployeeInsuranceCareChildFreeRate;
                    InsuranceCareSum = maxGross * contributionrate / 100;
                }
                else
                {
                    int children = (int)this.BillingInput.ChildTaxCredit;
                    contributionrate = Math.Max(SocialSecurityRates.EmployeeMinChildrenDiscount, SocialSecurityRates.EmployeeInsuranceCareRate - SocialSecurityRates.EmployeeInsuranceCareChildDiscountRate * children);
                    InsuranceCareSum += maxGross * contributionrate / 100;
                }

                InsuranceCareSum = Math.Round(InsuranceCareSum, 2);
            }
            else
            {
                InsuranceSum = this.BillingInput.PrivateInsurance;
            }

            if (TaxInformation != null && SocialSecurityRates != null)
            {

                if (this.BillingInput.HasFederalPension == "true")
                {
                    contributionrate = SocialSecurityRates.EmployeePensionRate;
                    maxGross = this.BillingInput.GrossIncome * 12 < SocialSecurityRates.PensionAndUnimploymentMaxGross ? this.BillingInput.GrossIncome : SocialSecurityRates.PensionAndUnimploymentMaxGross / 12;

                    PensionSum = Math.Round(maxGross * contributionrate / 100, 2);
                }

                if (this.BillingInput.HasFederalUnimployment == "true")
                {
                    maxGross = this.BillingInput.GrossIncome * 12 < SocialSecurityRates.PensionAndUnimploymentMaxGross ? this.BillingInput.GrossIncome : SocialSecurityRates.PensionAndUnimploymentMaxGross / 12;
                    contributionrate = SocialSecurityRates.EmployeeUnemploymentRate;

                    UnimploymentSum = Math.Round(maxGross * contributionrate / 100, 2);
                }
                decimal freeFromClass = 0.00m;

                if (this.BillingInput.TaxClass == 1 || this.BillingInput.TaxClass == 4)
                {
                    freeFromClass += TaxInformation.TaxFreeBasicFlat;
                    freeFromClass += TaxInformation.TaxFreeEmployeeFlat;
                    freeFromClass += TaxInformation.TaxFreeChildFlat * this.BillingInput.ChildTaxCredit;
                }
                else if (this.BillingInput.TaxClass == 2)
                {
                    freeFromClass += TaxInformation.TaxFreeBasicFlat;
                    freeFromClass += TaxInformation.TaxFreeEmployeeFlat;
                    freeFromClass += TaxInformation.TaxFreeChildFlat * this.BillingInput.ChildTaxCredit;
                    freeFromClass += TaxInformation.TaxFreeChildGrowingFlat;
                }
                else if (this.BillingInput.TaxClass == 3)
                {
                    freeFromClass += TaxInformation.TaxFreeBasicFlat;
                    freeFromClass += TaxInformation.TaxFreeEmployeeFlat;
                    freeFromClass += TaxInformation.TaxFreeChildFlat * this.BillingInput.ChildTaxCredit;
                }
                else if (this.BillingInput.TaxClass == 5)
                {
                    freeFromClass += TaxInformation.TaxFreeEmployeeFlat;
                }

                decimal forTax = 0.00m;
                if (this.BillingInput.TaxClass != 3)
                {
                    forTax = this.BillingInput.GrossIncome * 12 - InsurancesTotal * 12 - freeFromClass;
                }
                else
                {
                    forTax = (this.BillingInput.GrossIncome * 12  - InsurancesTotal * 12) / 2 - freeFromClass;
                }


                //Getting the Tuples to calculate tax
                Tuple<decimal, decimal, decimal, decimal, decimal> taxSet = TaxInformation.GetTaxValue(forTax, this.BillingInput.InChurch);

                // This is bec of spliting the tax in half in class 3 for taxation and than double again
                if (this.BillingInput.TaxClass == 3)
                {
                    taxSet = new(taxSet.Item1 * 2, taxSet.Item2 * 2, taxSet.Item3 * 2, taxSet.Item4 * 2, taxSet.Item5);

                }
                if (taxSet != null)
                {
                    TaxSum = taxSet.Item2 / 12;
                    SolidaryTaxSum = taxSet.Item3 / 12;
                    ChurchTaxSum = taxSet.Item4 / 12;
                    BorderTaxSet = taxSet.Item1;
                }
            }

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
