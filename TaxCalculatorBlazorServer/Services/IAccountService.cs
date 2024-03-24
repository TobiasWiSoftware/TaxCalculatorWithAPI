using Microsoft.AspNetCore.Identity;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorBlazorServer.Services
{
    public interface IAccountService
    {
        public Task<bool> RegisterAsync(RegisterModel model);

        public Task<bool> LoginAsync(LoginModel model);
    }
}
