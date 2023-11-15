using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Formats.Asn1;
using System;
using TaxCalculatorASP;

namespace TaxCalculatorAPI.Services
{
    public class MainService : IMainService
    {
        public MainService() { }
        public BillingOutput Calculation(BillingInput billingInput)
        {
            BillingOutput billingOutput = new();

            List<SocialSecurityRates>? lsr = SocialSecurityRates.GetList();
            List<TaxInformation>? lti = TaxInformation.GetList();
            if (lsr != null && lti != null)
            {
                billingOutput.SocialSecurityRatesOfYear = lsr.Find(x => x.Year == billingInput.Year);
                billingOutput.TaxInformationOfYear = lti.Find(x => x.Year == billingInput.Year);

            }

            decimal contributionrate = billingOutput.SocialSecurityRatesOfYear.EmployeeInsuranceRate + billingOutput.SocialSecurityRatesOfYear.EmployeeInsuranceBonusRate;
            decimal maxGross = billingInput.GrossIncome * 12 < billingOutput.SocialSecurityRatesOfYear.InsuranceMaxGross ? billingInput.GrossIncome : billingOutput.SocialSecurityRatesOfYear.InsuranceMaxGross / 12;

            billingOutput.InsuranceSum = Math.Round(maxGross * contributionrate / 100, 2);

            contributionrate = billingOutput.SocialSecurityRatesOfYear.EmployeeInsuranceCareRate;

            if (!billingInput.HasChildren)
            {
                contributionrate += billingOutput.SocialSecurityRatesOfYear.EmployeeInsuranceCareChildFreeRate;
                billingOutput.InsuranceCareSum = maxGross * contributionrate / 100;
            }
            else
            {
                int children = billingInput.ChildTaxCredit % 1 == 0 ? Convert.ToInt32(billingInput.ChildTaxCredit) : Convert.ToInt32(billingInput.ChildTaxCredit * 2);
                contributionrate -= children - billingOutput.SocialSecurityRatesOfYear.EmployeeMinChildrenDiscount * billingOutput.SocialSecurityRatesOfYear.EmployeeInsuranceCareChildDiscountRate;
                billingOutput.InsuranceCareSum += maxGross * contributionrate / 100;
            }

            billingOutput.InsuranceCareSum = Math.Round(billingOutput.InsuranceCareSum, 2);

            contributionrate = billingOutput.SocialSecurityRatesOfYear.EmployeePensionRate;
            maxGross = billingInput.GrossIncome <   billingOutput.SocialSecurityRatesOfYear.PensionAndUnimploymentMaxGross ? billingInput.GrossIncome : billingOutput.SocialSecurityRatesOfYear.PensionAndUnimploymentMaxGross;

            billingOutput.PensionSum = Math.Round(billingInput.GrossIncome * contributionrate / 100, 2);

            contributionrate = billingOutput.SocialSecurityRatesOfYear.EmployeeUnemploymentRate;

            billingOutput.UnimploymentSum = Math.Round(billingInput.GrossIncome * contributionrate / 100, 2);
            decimal freeFromClass = 0.00m;

            if (billingInput.TaxClass == 1 || billingInput.TaxClass == 6 || billingInput.TaxClass == 4)
            {
                freeFromClass += billingOutput.TaxInformationOfYear.TaxFreeBasicFlat;
                freeFromClass += billingOutput.TaxInformationOfYear.TaxFreeEmployeeFlat;
                freeFromClass += billingOutput.TaxInformationOfYear.TaxFreeChildFlat * billingInput.ChildTaxCredit;
            }
            else if (billingInput.TaxClass == 2)
            {
                freeFromClass += billingOutput.TaxInformationOfYear.TaxFreeBasicFlat;
                freeFromClass += billingOutput.TaxInformationOfYear.TaxFreeEmployeeFlat;
                freeFromClass += billingOutput.TaxInformationOfYear.TaxFreeChildFlat * billingInput.ChildTaxCredit;
                freeFromClass += billingOutput.TaxInformationOfYear.TaxFreeChildGrowingFlat;
            }
            else if (billingInput.TaxClass == 3)
            {
                freeFromClass += billingOutput.TaxInformationOfYear.TaxFreeBasicFlat * 2;
                freeFromClass += billingOutput.TaxInformationOfYear.TaxFreeEmployeeFlat;
                freeFromClass += billingOutput.TaxInformationOfYear.TaxFreeChildFlat * billingInput.ChildTaxCredit;
            }
            else if (billingInput.TaxClass == 5)
            {
                freeFromClass += billingOutput.TaxInformationOfYear.TaxFreeEmployeeFlat;
            }

            decimal forTax = billingInput.GrossIncome * 12 - billingOutput.InsurancesTotal * 12 - freeFromClass;



            //Getting the Tuples to calculate tax

            Tuple<decimal, decimal, decimal>? taxSet = billingOutput.TaxInformationOfYear.GetTaxValue(forTax);

            if (taxSet != null)
            {
                billingOutput.TaxedIncome = forTax + billingOutput.TaxInformationOfYear.TaxFreeBasicFlat;
                billingOutput.TaxSum = taxSet.Item3 / 12;
                billingOutput.BorderTaxSet = taxSet.Item1;
                billingOutput.AvgTaxSet = billingOutput.TaxSum * 12 / billingOutput.TaxedIncome;
            }


            return billingOutput;
        }
    }
}
