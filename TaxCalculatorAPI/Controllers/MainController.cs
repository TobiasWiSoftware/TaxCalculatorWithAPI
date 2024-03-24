using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxCalculatorAPI.Services;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IMainControllerService _mainService;
        public MainController(IMainControllerService mainService)
        {
            _mainService = mainService;
        }

        [HttpPost("TransferAmount")]
        public async Task<ActionResult<BillingOutput>> TransferAmount(BillingInput billingInput)
        {
            var result = await _mainService.Calculation(billingInput);
            return Ok(result);
        }
        [HttpPost("TransferSocialSecurityRates")]
        public ActionResult<SocialSecurityRates> TransferSocialSecurityRates([FromBody]int year)
        {
            
            return Ok(_mainService.FetchSocialSecurityRates(year));
        }

        [HttpPost("TransferTaxInformation")]
        public ActionResult<TaxInformation> TransferTaxInformation([FromBody] int year)
        {

            return Ok(_mainService.FetchTaxInformation(year));
        }

    }
}
