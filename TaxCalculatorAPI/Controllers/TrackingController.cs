using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxCalculatorAPI.Services;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {   
        private readonly ITrackingService _trackingService;
        public TrackingController(ITrackingService trackingService)
        {
            _trackingService = trackingService;
        }

        [HttpPost("IncrementVisitorCounter")]
        public async Task<ActionResult> IncrementVisitorCounter()
        {
            return Ok(await _trackingService.IncrementVisitCounter());
        }

        [HttpGet("GetVisitorCounter")]
        public async Task<ActionResult<int>> GetVisitorCounter()
        {
            return Ok(await _trackingService.GetVisitCounter());
        }

        [HttpPost("DeleteVisitorCounter")]
        public async Task<ActionResult> DeleteVisitorCounter()
        {
            return Ok(await _trackingService.DeleteVisitCounter());
        }

    }
}
