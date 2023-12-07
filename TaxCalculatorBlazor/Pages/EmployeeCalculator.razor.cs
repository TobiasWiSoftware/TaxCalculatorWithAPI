using TaxCalculatorLibary.Models;
using TaxCalculatorBlazor.Services;
using Microsoft.AspNetCore.Components;

namespace TaxCalculatorBlazor.Pages
{
    public partial class EmployeeCalculator : ComponentBase
    {
        [Inject]
        public IMainService? MainService { get; set; }
        public BillingInput? Input { get; set; }
        public BillingOutput? Output { get; set; }
        public bool ChildrenTaxCreditDisplayed { get; set; } = false;

        private bool IsCurrentYear(int year)
        {
            return DateTime.Now.Year == year;
        }
        private void HandleYearChange(int year)
        {
            Input.Year = year;
        }

        private void CheckCommaInsuranceAddition(ChangeEventArgs e)
        {
            Input.InsuranceAdditionTotal = e.Value.ToString().Contains(",") ? decimal.Parse(e.Value.ToString().Replace(",", ".")) : decimal.Parse(e.Value.ToString());
        }
        private void CheckCommaGrossIncome(ChangeEventArgs e)
        {
            Input.GrossIncome = e.Value.ToString().Contains(",") ? decimal.Parse(e.Value.ToString().Replace(",", ".")) : decimal.Parse(e.Value.ToString());
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
                Tuple<SocialSecurityRates, TaxInformation>? tuple = await MainService.FetchSocialAndTaxData(2023);

                SocialSecurityRates? sr = tuple.Item1;
                TaxInformation? tr = tuple.Item2;

                if (sr != null)
                {
                    decimal socialAddition = sr.EmployeeInsuranceBonusRate + sr.EmployerInsuranceBonusRate;
                    Input = new(DateTime.Now.Year, 3000m, true, 1, 30, false, 0, "true", 0, socialAddition, "true", "true");
                }

            }
        }

        public async Task CalculateTax()
        {
            if (MainService != null && Input != null)
            {
                try
                {
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
                        Input.GrossIncome = Input.GrossIncome * 12;
                    }
                }
            }
        }
    }
}
