using TaxCalculatorLibary.Models;
using TaxCalculatorBlazorServer.Services;
using Microsoft.AspNetCore.Components;

namespace TaxCalculatorBlazorServer.Shared
{
    partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        public IMainService? MainService { get; set; }
        public int FirstVisitCounter { get; set; } = -1;

        protected override async Task OnInitializedAsync()
        {

            
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (MainService != null && FirstVisitCounter == -1)
                {
                    await MainService.IncrementVisitCounter();
                    FirstVisitCounter = await MainService.GetVisitCounter();
                }

                // For re-rendering the component bec. the wrong value is in the visit counter
                StateHasChanged();
            }
        }

    }
}
