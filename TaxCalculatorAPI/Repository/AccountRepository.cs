using Microsoft.AspNetCore.Identity;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountRepository(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<SignInResult> LoginAsync(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            return result;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterModel model)
        {
            var user = new User(model.UserName);
            var result = await _userManager.CreateAsync(user, model.Password);
            return result;
        }
    }
}
