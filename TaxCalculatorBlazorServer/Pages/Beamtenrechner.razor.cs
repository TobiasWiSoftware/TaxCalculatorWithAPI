using TaxCalculatorLibary.Models;
using TaxCalculatorBlazorServer;
using Microsoft.AspNetCore.Components;
using TaxCalculatorBlazorServer.Services;
using Microsoft.JSInterop;


namespace TaxCalculatorBlazorServer.Pages
{
    public partial class Beamtenrechner : ComponentBase
    {
        [Inject]
        public IMainService? MainService { get; set; }
        public BillingInput Input { get; set; } = new();
        public BillingOutput? Output { get; set; }
        public bool ChildrenTaxCreditDisplayed { get; set; } = false;
        public bool WithPrivateInsurance { get; set; } = true;
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
                SocialSecurityRates? sc = null;

                if (MainService != null)
                    sc = await MainService.FetchSocialSecurityRates(Input.Year);

                if (sc != null)
                    Input.InsuranceAdditionTotal = Math.Round(sc.EmployeeInsuranceBonusRate + sc.EmployerInsuranceBonusRate, 2);
                StateHasChanged();
            }
        }
        private void HandleChurchTaxChange() => Input.InChurch = Input.InChurch == true ? false : true;
        private void HandleChildrenChange()
        {
            Input.HasChildren = Input.HasChildren == true ? false : true;

            ChildrenTaxCreditDisplayed = Input.HasChildren ? true : false;
        }
        private void HandlePrivateInsuranceChange(ChangeEventArgs e)
        {
            if (e.Value != null)
            {
                WithPrivateInsurance = bool.Parse(e.Value.ToString());
                if (WithPrivateInsurance && Input.PrivateInsurance == 0)
                {
                    Input.PrivateInsurance = 300m;
                }
                else if (!WithPrivateInsurance)
                {
                    Input.PrivateInsurance = 0m;
                }
            }
        }
        protected override async Task OnInitializedAsync()
        {

            if (MainService != null)
            {
                if (Input.Year == 0)
                {
                    Input.Year = DateTime.Now.Year;
                }

                SocialSecurityRates? sr = await MainService.FetchSocialSecurityRates(Input.Year);
                TaxInformation? tr = await MainService.FetchTaxInformation(Input.Year);

                if (sr != null)
                {
                    decimal socialAddition = sr.EmployeeInsuranceBonusRate + sr.EmployerInsuranceBonusRate;
                    Input = new(DateTime.Now.Year, 3000m, true, 1, 30, false, 0.0m, "false", 300.00m, socialAddition, "false", "false");
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

