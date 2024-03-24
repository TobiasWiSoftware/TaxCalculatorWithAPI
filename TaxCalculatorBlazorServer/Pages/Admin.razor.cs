using TaxCalculatorLibary.Models;
using TaxCalculatorBlazorServer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Security.Principal;

namespace TaxCalculatorBlazorServer.Pages
{
    public partial class Admin : ComponentBase
    {
        [Inject]
        public IAccountService AccountService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        public RegisterModel RegisterModel { get; set; } = new RegisterModel();

        public LoginModel LoginModel { get; set; } = new LoginModel();

        public bool IsRegister { get; set; } = true;

        public bool IsLogin { get; set; } = false;

        public bool IsLoading { get; set; } = false;

        public bool IsError { get; set; } = false;

        public string ErrorMessage { get; set; } = string.Empty;

        public async Task LoginAsync()
        {
            IsLoading = true;
            IsError = false;

            var result = await AccountService.LoginAsync(LoginModel);

            if (result)
            {
                NavigationManager.NavigateTo("/");
            }
            else
            {
                IsError = true;
                ErrorMessage = "Login failed";
            }

            IsLoading = false;
        }

        public void ToggleRegister()
        {
            IsRegister = true;
            IsLogin = false;
        }

        public void ToggleLogin()
        {
            IsRegister = false;
            IsLogin = true;
        }

    }
}
