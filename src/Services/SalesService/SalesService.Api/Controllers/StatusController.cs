using Microsoft.AspNetCore.Mvc;

namespace SalesService.Api.Controllers
{
    [ApiController]
    [Route("status")]
    public class StatusController : Controller
    {
        [HttpGet]
        public IActionResult GetServiceStatus() => Ok(new { status = "Sales Service is working correctly! \ud83d\udfe2" });
    }
}
