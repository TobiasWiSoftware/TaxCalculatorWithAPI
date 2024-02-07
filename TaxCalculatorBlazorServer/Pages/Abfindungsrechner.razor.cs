using TaxCalculatorLibary.Models;
using TaxCalculatorBlazorServer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;


namespace TaxCalculatorBlazorServer.Pages
{
    public partial class Abfindungsrechner : ComponentBase
    {
        [Inject]
        public IMainService? MainService { get; set; }
        public BillingInput Input { get; set; } = new();
        public bool Fiftrule { get; set; } = false;
        public BillingOutput? Output { get; set; }
        public bool ChildrenTaxCreditDisplayed { get; set; } = false;
        public bool WithPrivateInsurance { get; set; } = false;
        public string ChildTaxCreditString { get; set; } = "0";
        private bool IsCurrentYear(int year)
        {
            return DateTime.Now.Year == year;
        }
        private void HandleChurchTaxChange() => Input.InChurch = Input.InChurch == true ? false : true;
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
        private void HandleChildrenChange()
        {
            Input.HasChildren = Input.HasChildren == true ? false : true;

            ChildrenTaxCreditDisplayed = Input.HasChildren ? true : false;
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
                    Input = new(DateTime.Now.Year, 30000, false, 6, 30, false, 0.0m, "false", 0m, 0.00m, "false", "false");
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
                    if(Fiftrule)
                    {
                        Input.GrossIncome = Input.GrossIncome / 5;
                    }

                    Output = await MainService.Calculation(Input);



                    if (Fiftrule)
                    {
                        Output.BillingInput.GrossIncome = Output.BillingInput.GrossIncome * 5;
                        // Has not the same reference because of api connection
                        Input.GrossIncome = Input.GrossIncome * 5;
                        Output.TaxSum = Output.TaxSum * 5;
                    } 


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
