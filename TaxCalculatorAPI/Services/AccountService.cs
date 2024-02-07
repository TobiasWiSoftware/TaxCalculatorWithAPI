using Microsoft.AspNetCore.Identity;
using TaxCalculatorAPI.Repository;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public Task<SignInResult> LoginAsync(LoginModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> RegisterAsync(RegisterModel model)
        {
            if(model.Password != model.ConfirmPassword)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Passwords do not match" });
            }
            return await _accountRepository.RegisterAsync(model); // With automatic saving in db
        }
    }
}
