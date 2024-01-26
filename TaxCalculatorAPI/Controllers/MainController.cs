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
        [HttpPost("TransferInput")]
        public ActionResult<Tuple<SocialSecurityRates, TaxInformation>> TransferInput([FromBody]int year)
        {
            
            return Ok(_mainService.FetchSocialAndTaxData(year));
        }

        [HttpPost("IncrementVisitCounter")]
        public ActionResult<bool> IncrementVisitCounter()
        {
            return Ok(_mainService.IncrementVisitCounter());
        }
    }
}
