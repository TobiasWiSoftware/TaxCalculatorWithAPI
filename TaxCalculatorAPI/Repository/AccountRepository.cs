using Microsoft.AspNetCore.Identity;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<User> _userManager;
        public AccountRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<SignInResult> LoginAsync(LoginModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> RegisterAsync(RegisterModel model)
        {
            var user = new User(model.UserName);
            var result = await _userManager.CreateAsync(user, model.Password);
            return result;
        }
    }
}
