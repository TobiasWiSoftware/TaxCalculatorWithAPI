using TaxCalculatorLibary.Models;
using TaxCalculatorBlazor.Services;
using Microsoft.AspNetCore.Components;

namespace TaxCalculatorBlazor.Pages
{
    public partial class SelfemployeeCalculator : ComponentBase
    {
        [Inject]
        public IMainService? MainService { get; set; }
        public BillingInput? Input { get; set; }
        public BillingOutput? Output { get; set; }
        public bool ChildrenTaxCreditDisplayed { get; set; } = false;
        public bool WithPrivateInsurance { get; set; } = false;
        public string ChildTaxCreditString { get; set; }
        private bool IsCurrentYear(int year)
        {
            return DateTime.Now.Year == year;
        }
        private void HandleYearChange(int year)
        {
            Input.Year = year;
        }

        private void HandleChurchTaxChange() => Input.InChurch = Input.InChurch == true ? false : true;
        private void HandleChildrenChange()
        {
            Input.HasChildren = Input.HasChildren == true ? false : true;

            ChildrenTaxCreditDisplayed = Input.HasChildren ? true : false;
        }

        private void HandlePrivateInsuranceChange(ChangeEventArgs e)
        {
            WithPrivateInsurance = bool.Parse(e.Value.ToString());
            if (WithPrivateInsurance && Input.PrivateInsurance == 0)
            {
                Input.PrivateInsurance = 600m;
            }
            else if (!WithPrivateInsurance)
            {
                Input.PrivateInsurance = 0m;
            }
        }


        protected override async Task OnInitializedAsync()
        {

            if (MainService != null)
            {
                Tuple<SocialSecurityRates, TaxInformation>? tuple = await MainService.FetchSocialAndTaxData(2023);

                SocialSecurityRates? sr = tuple.Item1;
                TaxInformation? tr = tuple.Item2;

                if (sr != null)
                {
                    decimal socialAddition = sr.EmployeeInsuranceBonusRate + sr.EmployerInsuranceBonusRate;
                    Input = new(DateTime.Now.Year, 3000m, true, 1, 30, false, 0.0m, "false", 0m, socialAddition, "false", "false");
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
    }
}
