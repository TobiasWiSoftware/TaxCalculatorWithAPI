using Microsoft.AspNetCore.Identity;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterModel model);
        Task<SignInResult> LoginAsync(LoginModel model);
    }
}
