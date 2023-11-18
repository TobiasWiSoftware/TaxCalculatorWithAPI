using TaxCalculatorLibary.Models;
using TaxCalculatorBlazor.Services;
using Microsoft.AspNetCore.Components;

namespace TaxCalculatorBlazor.Pages
{
    public partial class EmployeeCalculator : ComponentBase
    {
        [Inject]
        public IMainService MainService { get; set; }

        public BillingInput Input { get; set; } = new BillingInput();
        public BillingOutput Output { get; set; }

        public bool IsChecked2023 { get; set; } = DateTime.Now.Year == 2023;
        public bool IsChecked2024 { get; set; } = DateTime.Now.Year == 2024;

        public async Task CalculateTax()
        {
            Output = await MainService.Calculation(Input);
            StateHasChanged();
        }

        private void CreateRole() { }
    }
}
