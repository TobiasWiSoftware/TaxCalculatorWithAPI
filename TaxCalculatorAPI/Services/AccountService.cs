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
            return _accountRepository.LoginAsync(model);
        }

        public async Task<IdentityResult> RegisterAsync(RegisterModel model)
        {
            return await _accountRepository.RegisterAsync(model); // With automatic saving in db
        }
    }
}
