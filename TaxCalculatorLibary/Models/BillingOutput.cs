namespace TaxCalculatorLibary.Models
{
    public class BillingOutput
    {
        public decimal GrossIncome { get; set; }
        public decimal InsuranceSum { get; set; }
        public decimal InsuranceCareSum { get; set; }
        public decimal PensionSum { get; set; }
        public decimal UnimploymentSum { get; set; }
        public decimal InsurancesTotal { get => InsuranceSum + InsuranceCareSum + PensionSum + UnimploymentSum; }
        public decimal TaxedIncome { get; set; }
        public decimal TaxSum { get; set; }
        public decimal BorderTaxSet { get; set; }
        public decimal AvgTaxSet { get; set; }
        public decimal Transferamount { get => GrossIncome - InsurancesTotal - TaxSum; }


        public BillingOutput()
        {

        }

        public void Calculation(BillingInput billingInput)
        {

            SocialSecurityRates? socialSecurityRatesOfYear = SocialSecurityRates.GetDataFromYear(billingInput.Year);
            TaxInformation? taxInformationOfYear = TaxInformation.GetDataFromYear(billingInput.Year);

            if (socialSecurityRatesOfYear != null || taxInformationOfYear != null)
            {
                GrossIncome = billingInput.GrossIncome;

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
                maxGross = billingInput.GrossIncome < socialSecurityRatesOfYear.PensionAndUnimploymentMaxGross ? billingInput.GrossIncome : socialSecurityRatesOfYear.PensionAndUnimploymentMaxGross;

                PensionSum = Math.Round(billingInput.GrossIncome * contributionrate / 100, 2);

                contributionrate = socialSecurityRatesOfYear.EmployeeUnemploymentRate;

                UnimploymentSum = Math.Round(billingInput.GrossIncome * contributionrate / 100, 2);
                decimal freeFromClass = 0.00m;

                if (billingInput.TaxClass == 1 || billingInput.TaxClass == 6 || billingInput.TaxClass == 4)
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

                Tuple<decimal, decimal, decimal>? taxSet = taxInformationOfYear.GetTaxValue(forTax);

                if (taxSet != null)
                {
                    TaxedIncome = forTax + taxInformationOfYear.TaxFreeBasicFlat;
                    TaxSum = taxSet.Item3 / 12;
                    BorderTaxSet = taxSet.Item1;
                    AvgTaxSet = TaxSum * 12 / TaxedIncome;
                }
            }
            else
            {
                throw new Exception();
            }

        }

    }
}
