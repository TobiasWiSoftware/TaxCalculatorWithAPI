using TaxCalculatorLibary.Models;
using TaxCalculatorBlazor.Services;
using Microsoft.AspNetCore.Components;

namespace TaxCalculatorBlazor.Pages
{
    public partial class EmployeeCalculator : ComponentBase
    {
        [Inject]
        public IMainService MainService { get; set; }

        public BillingInput Input { get; set; }
        public BillingOutput Output { get; set; }

        protected override void OnInitialized()
        {


            SocialSecurityRates? sr = MainService.FetchSocialAndTaxData(DateTime.Now.Year).Result.Item1;
            TaxInformation? tr = MainService.FetchSocialAndTaxData(DateTime.Now.Year).Result.Item2;

            if (sr != null)
            {
                decimal socialAddition = sr.EmployeeInsuranceBonusRate + sr.EmployerInsuranceBonusRate;
                Input = new(DateTime.Now.Year, 3000m, true, 3, "BY", 30, false, 0, true, socialAddition, true, true);
            }


        }

        public async Task CalculateTax()
        {
            Output = await MainService.Calculation(Input);
            StateHasChanged();
        }

        private void CreateRole() { }
    }
}
