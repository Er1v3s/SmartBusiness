using Microsoft.AspNetCore.Mvc;

namespace ReadService.Api.Controllers
{
    [ApiController]
    [Route("status")]
    public class StatusController : Controller
    {
        [HttpGet]
        public IActionResult GetServiceStatus() => Ok(new { status = "Read Service is working correctly! \ud83d\udfe2" });
    }
}
