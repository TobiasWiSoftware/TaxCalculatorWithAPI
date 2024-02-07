using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Formats.Asn1;
using System;
using TaxCalculatorLibary.Models;
using Microsoft.EntityFrameworkCore;
using TaxCalculatorAPI.Data;

namespace TaxCalculatorAPI.Services
{
    public class MainControllerService : IMainControllerService
    {
        private readonly ApplicationDBContext _dbcontext;
        private readonly ITaxInformationService _taxInformationService;
        public MainControllerService(ApplicationDBContext dbcontext, ITaxInformationService taxInformationService)
        {
            _dbcontext = dbcontext;
            _taxInformationService = taxInformationService;
        }
        public async Task<BillingOutput> Calculation(BillingInput billingInput)
        {
            BillingOutput billingOutput = new BillingOutput();
            billingOutput.BillingInput = billingInput;

            decimal contributionrate = 0.00m;
            decimal maxGross = 0;
            SocialSecurityRates? socialSecurityRates = await _dbcontext.SocialSecurityRates.FirstOrDefaultAsync(x => x.Year == billingInput.Year);
            

            if (billingInput.HasFederalInsurance == "true" && socialSecurityRates != null)
            {
                contributionrate = socialSecurityRates.EmployeeInsuranceRate + billingInput.InsuranceAdditionTotal / 2;
                maxGross = billingInput.GrossIncome * 12 < socialSecurityRates.InsuranceMaxGross ? billingInput.GrossIncome : socialSecurityRates.InsuranceMaxGross / 12;

                billingOutput.InsuranceSum = Math.Round(maxGross * contributionrate / 100, 2);

                contributionrate = socialSecurityRates.EmployeeInsuranceCareRate;

                if (!billingInput.HasChildren)
                {
                    contributionrate += socialSecurityRates.EmployeeInsuranceCareChildFreeRate;
                    billingOutput.InsuranceCareSum = maxGross * contributionrate / 100;
                }
                else
                {
                    int children = (int)billingInput.ChildTaxCredit;
                    contributionrate = Math.Max(socialSecurityRates.EmployeeMinChildrenDiscount, socialSecurityRates.EmployeeInsuranceCareRate - socialSecurityRates.EmployeeInsuranceCareChildDiscountRate * children);
                    billingOutput.InsuranceCareSum += maxGross * contributionrate / 100;
                }

                billingOutput.InsuranceCareSum = Math.Round(billingOutput.InsuranceCareSum, 2);
            }
            else
            {
                billingOutput.InsuranceSum = billingInput.PrivateInsurance;
            }

            TaxInformation? taxInformation = await _dbcontext.TaxInformation.FirstOrDefaultAsync(x => x.Year == billingInput.Year);

            if (taxInformation != null && socialSecurityRates != null)
            {

                if (billingInput.HasFederalPension == "true")
                {
                    contributionrate = socialSecurityRates.EmployeePensionRate;
                    maxGross = billingInput.GrossIncome * 12 < socialSecurityRates.PensionAndUnimploymentMaxGross ? billingInput.GrossIncome : socialSecurityRates.PensionAndUnimploymentMaxGross / 12;

                    billingOutput.PensionSum = Math.Round(maxGross * contributionrate / 100, 2);
                }

                if (billingInput.HasFederalUnimployment == "true")
                {
                    maxGross = billingInput.GrossIncome * 12 < socialSecurityRates.PensionAndUnimploymentMaxGross ? billingInput.GrossIncome : socialSecurityRates.PensionAndUnimploymentMaxGross / 12;
                    contributionrate = socialSecurityRates.EmployeeUnemploymentRate;

                    billingOutput.UnimploymentSum = Math.Round(maxGross * contributionrate / 100, 2);
                }
                decimal freeFromClass = 0.00m;

                if (billingInput.TaxClass == 1 || billingInput.TaxClass == 4)
                {
                    freeFromClass += taxInformation.TaxFreeBasicFlat;
                    freeFromClass += taxInformation.TaxFreeEmployeeFlat;
                    freeFromClass += taxInformation.TaxFreeChildFlat * billingInput.ChildTaxCredit;
                }
                else if (billingInput.TaxClass == 2)
                {
                    freeFromClass += taxInformation.TaxFreeBasicFlat;
                    freeFromClass += taxInformation.TaxFreeEmployeeFlat;
                    freeFromClass += taxInformation.TaxFreeChildFlat * billingInput.ChildTaxCredit;
                    freeFromClass += taxInformation.TaxFreeChildGrowingFlat;
                }
                else if (billingInput.TaxClass == 3)
                {
                    freeFromClass += taxInformation.TaxFreeBasicFlat;
                    freeFromClass += taxInformation.TaxFreeEmployeeFlat;
                    freeFromClass += taxInformation.TaxFreeChildFlat * billingInput.ChildTaxCredit;
                }
                else if (billingInput.TaxClass == 5)
                {
                    freeFromClass += taxInformation.TaxFreeEmployeeFlat;
                    freeFromClass += taxInformation.TaxFreeChildFlat * billingInput.ChildTaxCredit;
                }
                else
                {
                    freeFromClass += taxInformation.TaxFreeChildFlat * billingInput.ChildTaxCredit;
                }

                decimal forTax = 0.00m;
                decimal insurancesTotal = billingOutput.InsuranceSum + billingOutput.InsuranceCareSum + billingOutput.PensionSum + billingOutput.UnimploymentSum;

                if (billingInput.TaxClass != 3)
                {
                    forTax = billingInput.GrossIncome * 12 - insurancesTotal * 12 - freeFromClass;
                }
                else
                {
                    forTax = (billingInput.GrossIncome * 12 - insurancesTotal * 12) / 2 - freeFromClass;
                }


                //Getting the calculate tax
                // Return with taxed value, taxsum, solidary tax, church tax, borderTaxSum
                TaxSet taxSet =  await _taxInformationService.GetTaxValue(billingInput.Year, forTax, billingInput.InChurch);

                // This is bec of spliting the tax in half in class 3 for taxation and than double again
                if (billingInput.TaxClass == 3)
                {

                    taxSet = new(taxSet.TaxedValue * 2, taxSet.TaxSum * 2, taxSet.SolidaryTax * 2, taxSet.ChurchTax * 2, taxSet.BorderTaxSum);

                }
                if (taxSet != null)
                {
                    billingOutput.TaxSum = taxSet.TaxSum / 12;
                    billingOutput.SolidaryTaxSum = taxSet.SolidaryTax / 12;
                    billingOutput.ChurchTaxSum = taxSet.ChurchTax / 12;
                    billingOutput.BorderTaxSet = taxSet.BorderTaxSum;
                }

            }

            return billingOutput;
        }
        public SocialSecurityRates? FetchSocialSecurityRates(int year)
        {
            SocialSecurityRates? rates = _dbcontext.SocialSecurityRates.FirstOrDefault(x => x.Year == year);
            return rates;
        }
        public TaxInformation? FetchTaxInformation(int year)
        {
            TaxInformation? taxes = _dbcontext.TaxInformation.FirstOrDefault(x => x.Year == year);
            return taxes;
        }
        public int IncrementVisitCounter()
        {
            return Tracking.IncrementVisitCounter();
        }
    }
}
