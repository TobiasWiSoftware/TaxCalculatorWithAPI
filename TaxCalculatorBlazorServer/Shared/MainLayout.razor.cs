using TaxCalculatorLibary.Models;
using TaxCalculatorBlazorServer.Services;
using Microsoft.AspNetCore.Components;

namespace TaxCalculatorBlazorServer.Shared
{
    partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        public IMainService? MainService { get; set; }
        public bool IsFirstVisit { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {

            if (MainService != null && IsFirstVisit)
            {
                await MainService.IncrementVisitCounter();
            }
        }
    }
}
