﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaxCalculatorAPI.Services;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<IdentityResult>> RegisterUser(RegisterModel model)
        {
            var result = await _accountService.RegisterAsync(model);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            
            return Ok("Erfolgreich registriert");
        }
    }
}
