using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Formats.Asn1;
using System;
using TaxCalculatorLibary.Models;
using Microsoft.EntityFrameworkCore;
using TaxCalculatorAPI.Data;
using TaxCalculatorAPI.Repository;

namespace TaxCalculatorAPI.Services
{
    public class MainControllerService : IMainControllerService
    {
        private readonly IDataBaseRepository _dataBaseRepository;
        private readonly ITaxInformationService _taxInformationService;
        public MainControllerService(IDataBaseRepository dataBaseRepository, ITaxInformationService taxInformationService)
        {
            _dataBaseRepository = dataBaseRepository;
            _taxInformationService = taxInformationService;
        }

        public async Task<TaxSet> GetTaxValue(int year, decimal value, bool inChurch)
        {
            TaxSet taxSet = new();
            // Return with taxed value, taxsum, solidary tax, church tax, borderTaxSum

            TaxInformation? taxInformation = await _dataBaseRepository.GetTaxInformationAsync(year);


            if (taxInformation != null && taxInformation.TaxInformationSteps != null)
            {
                TaxInformationStep? taxSetBase = null;
                TaxInformationStep? taxSetNext = null;


                try
                {
                    // Find the last tax rates
                    taxSetBase = taxInformation.TaxInformationSteps.FindAll(x => x.StepAmount <= value).MaxBy(t => t.StepAmount);

                    //Find the next tax rates 
                    taxSetNext = taxInformation.TaxInformationSteps.FindAll(x => x.StepAmount > value).MinBy(t => t.StepAmount);
                }
                catch (Exception)
                {

                    throw;
                }




                if (taxSetBase != null && taxSetNext != null) // case when beetween tax table
                {
                    // Calculation for the last known border tax zone in sum + rest of the value in the folowing tax zone
                    decimal taxSetBaseSum = taxSetBase.TaxAmount;
                    decimal taxsetRestSum = (value - taxSetBase.StepAmount) * (taxSetNext.TaxAmount - taxSetBase.TaxAmount) / (taxSetNext.StepAmount - taxSetBase.StepAmount);

                    decimal borderTax = 0m;

                    if (value - taxSetBase.StepAmount > 0)
                    {
                        borderTax = taxsetRestSum / (value - taxSetBase.StepAmount);
                    }
                    else
                    {
                        borderTax = (taxSetNext.TaxAmount - taxSetBase.TaxAmount) / (taxSetNext.StepAmount - taxSetBase.TaxAmount);
                    }

                    taxSet = new(value, taxSetBaseSum + taxsetRestSum, 0, 0, borderTax);
                }
                else if (taxSetBase != null && taxSetNext == null) // when value over the max in table
                {
                    TaxInformationStep maxTable = taxSetBase;

                    decimal borderTax = taxInformation.MaxTaxLevel;
                    decimal sum = value;
                    decimal totalTax = Math.Round(maxTable.TaxAmount + (value - maxTable.StepAmount) * borderTax / 100, 0);

                    taxSet = new(Math.Round(sum), totalTax, 0, 0, borderTax);
                }

                // Check for solidary and church

                if (taxSet.TaxSum > taxInformation.MinLevelForSolidarityTax)
                {
                    // A approximate calculation for the solidary tax sliding zone
                    if (value < 102000)
                    {
                        taxSet = new(taxSet.TaxedValue, taxSet.TaxSum, (taxSet.TaxSum - taxInformation.MinLevelForSolidarityTax) * 0.119m, 0, taxSet.BorderTaxSum);
                    }
                    else
                    {
                        taxSet = new(taxSet.TaxedValue, taxSet.TaxSum, taxSet.TaxSum * taxInformation.SolidaryTaxRate / 100, 0, taxSet.BorderTaxSum);
                    }
                }

                if (inChurch)
                {
                    taxSet = new(taxSet.TaxedValue, taxSet.TaxSum, taxSet.SolidaryTax, taxSet.TaxSum * taxInformation.ChurchTaxRate / 100, taxSet.BorderTaxSum);
                }
            }
            return taxSet;
        }
        public async Task<BillingOutput> Calculation(BillingInput billingInput)
        {
            BillingOutput billingOutput = new BillingOutput();
            billingOutput.BillingInput = billingInput;

            decimal contributionrate = 0.00m;
            decimal maxGross = 0;
            SocialSecurityRates? socialSecurityRates = await _dataBaseRepository.GetSocialSecurityRatesAsync(billingInput.Year);
            

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

            TaxInformation? taxInformation = await _dataBaseRepository.GetTaxInformationAsync(billingInput.Year);

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
                TaxSet taxSet =  await GetTaxValue(billingInput.Year, forTax, billingInput.InChurch);

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
        public async Task<SocialSecurityRates?> FetchSocialSecurityRates(int year)
        {
            SocialSecurityRates? rates = await _dataBaseRepository.GetSocialSecurityRatesAsync(year);
            return rates;
        }
        public async Task<TaxInformation?> FetchTaxInformation(int year)
        {
            TaxInformation? taxes = await _dataBaseRepository.GetTaxInformationAsync(year);
            return taxes;
        }
    }
}
