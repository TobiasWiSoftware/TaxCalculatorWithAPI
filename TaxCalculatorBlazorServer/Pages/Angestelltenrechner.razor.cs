using TaxCalculatorLibary.Models;
using TaxCalculatorBlazorServer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Security.Principal;

namespace TaxCalculatorBlazorServer.Pages
{
    public partial class Angestelltenrechner : ComponentBase
    {
        [Inject]
        public IMainService? MainService { get; set; }
        public BillingInput Input { get; set; } = new();
        public BillingOutput? Output { get; set; }
        public bool ChildrenTaxCreditDisplayed { get; set; } = false;
        public string ChildTaxCreditString { get; set; } = "0";
        private bool IsCurrentYear(int year)
        {
            return DateTime.Now.Year == year;
        }
        private async void HandleYearChange(int year)
        {
            if (Input != null)
            {
                Input.Year = year;
                Tuple<SocialSecurityRates, TaxInformation>? tuple = await MainService.FetchSocialAndTaxData(Input.Year);
                Input.InsuranceAdditionTotal = Math.Round(tuple.Item1.EmployeeInsuranceBonusRate + tuple.Item1.EmployerInsuranceBonusRate,2);
                StateHasChanged();
            }
        }

        private void HandleChurchTaxChange() => Input.InChurch = Input.InChurch == true ? false : true;
        private void HandleChildrenChange()
        {
            Input.HasChildren = Input.HasChildren == true ? false : true;

            ChildrenTaxCreditDisplayed = Input.HasChildren ? true : false;
        }
        protected override async Task OnInitializedAsync()
        {

            if (MainService != null)
            {
                Tuple<SocialSecurityRates, TaxInformation>? tuple = await MainService.FetchSocialAndTaxData(DateTime.Now.Year);

                SocialSecurityRates? sr = tuple.Item1;
                TaxInformation? tr = tuple.Item2;

                if (Input.Year == 0)
                {
                    Input.Year = DateTime.Now.Year;
                }

                if (sr != null)
                {
                    decimal socialAddition = Math.Round(sr.EmployeeInsuranceBonusRate + sr.EmployerInsuranceBonusRate,2);
                    Input = new(Input.Year, 3000m, true, 1, 30, false, 0.0m, "true", 0, socialAddition, "true", "true");
                }

            }
        }
        public async Task CalculateTax()
        {
            if (MainService != null && Input != null)
            {
                try
                {
                    Input.ChildTaxCredit = Convert.ToDecimal(ChildTaxCreditString);
                    if (!Input.BillingPeriodMonthly)
                    {
                        Input.GrossIncome = Input.GrossIncome / 12;
                    }
                    Output = await MainService.Calculation(Input);
                    StateHasChanged();
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    if (!Input.BillingPeriodMonthly)
                    {
                        Input.GrossIncome = Math.Round(Input.GrossIncome * 12, 2);
                    }
                }
            }
        }
        public async Task Print()
        {
            await jsRuntime.InvokeVoidAsync("myPrint");
        }
    }
}
