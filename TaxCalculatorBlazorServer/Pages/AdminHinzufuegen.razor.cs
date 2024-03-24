using Microsoft.AspNetCore.Components;
using TaxCalculatorBlazorServer.Services;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorBlazorServer.Pages
{
    public partial class AdminHinzufuegen
    {
        [Inject]
        public IAccountService? AccountService { get; set; }
        public RegisterModel RegisterModel { get; set; } = new();
        public string ErrorMessage { get; set; } = string.Empty;

        public string confirmedPassword = string.Empty;

        public bool IsPasswordConfirmed = false;

        private bool TermsAccepted = false;

        public void CheckPasswords(ChangeEventArgs e)
        {
            confirmedPassword = e.Value.ToString();
            IsPasswordConfirmed = RegisterModel.Password.Equals(confirmedPassword);
        }
        private async void HandleValidSubmit()
        {
            try
            {
                if (AccountService != null && IsPasswordConfirmed && TermsAccepted)
                {
                    bool result = await AccountService.RegisterAsync(RegisterModel);
                    if (result != null && result)
                    {
                        ErrorMessage = "Sucessfully registered";
                    }
                    else
                    {
                        ErrorMessage = "Fehler bei der Registrierung";
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

    }
}
