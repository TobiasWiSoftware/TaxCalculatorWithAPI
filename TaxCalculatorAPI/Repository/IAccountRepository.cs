using Microsoft.AspNetCore.Identity;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> RegisterAsync(RegisterModel model);
        Task<SignInResult> LoginAsync(LoginModel model);
    }
}
