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
        private readonly IMainService _mainService;
        public MainController(IMainService mainService)
        {
            _mainService = mainService;
        }

        [HttpPost("TransferAmount")]
        public ActionResult<BillingOutput> TransferAmount(BillingInput billingInput)
        {
            return Ok(_mainService.Calculation(billingInput));
        }
        [HttpPost("TransferInput")]
        public ActionResult<Tuple<SocialSecurityRates, TaxInformation>> TransferInput(int year)
        {
            return Ok(_mainService.FetchSocialAndTaxData(year));
        }
    }
}
