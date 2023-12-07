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

            SocialSecurityRates? socialSecurityRatesOfYear = SocialSecurityRates.GetDataFromYear(billingInput.Year);
            TaxInformation? taxInformationOfYear = TaxInformation.GetDataFromYear(billingInput.Year);

            if (socialSecurityRatesOfYear != null || taxInformationOfYear != null)
            {
                BillingInput = billingInput;

                decimal contributionrate = socialSecurityRatesOfYear.EmployeeInsuranceRate + socialSecurityRatesOfYear.EmployeeInsuranceBonusRate;
                decimal maxGross = billingInput.GrossIncome * 12 < socialSecurityRatesOfYear.InsuranceMaxGross ? billingInput.GrossIncome : socialSecurityRatesOfYear.InsuranceMaxGross / 12;

                InsuranceSum = Math.Round(maxGross * contributionrate / 100, 2);

                contributionrate = socialSecurityRatesOfYear.EmployeeInsuranceCareRate;

                if (!billingInput.HasChildren)
                {
                    contributionrate += socialSecurityRatesOfYear.EmployeeInsuranceCareChildFreeRate;
                    InsuranceCareSum = maxGross * contributionrate / 100;
                }
                else
                {
                    int children = billingInput.ChildTaxCredit % 1 == 0 ? Convert.ToInt32(billingInput.ChildTaxCredit) : Convert.ToInt32(billingInput.ChildTaxCredit * 2);
                    contributionrate -= children - socialSecurityRatesOfYear.EmployeeMinChildrenDiscount * socialSecurityRatesOfYear.EmployeeInsuranceCareChildDiscountRate;
                    InsuranceCareSum += maxGross * contributionrate / 100;
                }

                InsuranceCareSum = Math.Round(InsuranceCareSum, 2);

                contributionrate = socialSecurityRatesOfYear.EmployeePensionRate;
                maxGross = billingInput.GrossIncome * 12 < socialSecurityRatesOfYear.PensionAndUnimploymentMaxGross ? billingInput.GrossIncome : socialSecurityRatesOfYear.PensionAndUnimploymentMaxGross / 12;

                PensionSum = Math.Round(maxGross * contributionrate / 100, 2);

                contributionrate = socialSecurityRatesOfYear.EmployeeUnemploymentRate;

                UnimploymentSum = Math.Round(maxGross * contributionrate / 100, 2);
                decimal freeFromClass = 0.00m;

                if (billingInput.TaxClass == 1 || billingInput.TaxClass == 4)
                {
                    freeFromClass += taxInformationOfYear.TaxFreeBasicFlat;
                    freeFromClass += taxInformationOfYear.TaxFreeEmployeeFlat;
                    freeFromClass += taxInformationOfYear.TaxFreeChildFlat * billingInput.ChildTaxCredit;
                }
                else if (billingInput.TaxClass == 2)
                {
                    freeFromClass += taxInformationOfYear.TaxFreeBasicFlat;
                    freeFromClass += taxInformationOfYear.TaxFreeEmployeeFlat;
                    freeFromClass += taxInformationOfYear.TaxFreeChildFlat * billingInput.ChildTaxCredit;
                    freeFromClass += taxInformationOfYear.TaxFreeChildGrowingFlat;
                }
                else if (billingInput.TaxClass == 3)
                {
                    freeFromClass += taxInformationOfYear.TaxFreeBasicFlat * 2;
                    freeFromClass += taxInformationOfYear.TaxFreeEmployeeFlat;
                    freeFromClass += taxInformationOfYear.TaxFreeChildFlat * billingInput.ChildTaxCredit;
                }
                else if (billingInput.TaxClass == 5)
                {
                    freeFromClass += taxInformationOfYear.TaxFreeEmployeeFlat;
                }

                decimal forTax = billingInput.GrossIncome * 12 - InsurancesTotal * 12 - freeFromClass;



                //Getting the Tuples to calculate tax

                Tuple<decimal, decimal, decimal, decimal, decimal> taxSet = taxInformationOfYear.GetTaxValue(forTax, billingInput.InChurch);

                if (taxSet != null)
                {
                    TaxSum = taxSet.Item2 / 12;
                    SolidaryTaxSum = taxSet.Item3 / 12;
                    ChurchTaxSum = taxSet.Item4 / 12;
                    BorderTaxSet = taxSet.Item1;
                }
            }
            else
            {
                throw new Exception();
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
