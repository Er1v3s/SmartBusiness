using Microsoft.AspNetCore.Mvc;

namespace WriteService.Api.Controllers
{
    [ApiController]
    [Route("status")]
    public class StatusController : Controller
    {
        [HttpGet]
        public IActionResult GetServiceStatus() => Ok(new { status = "Write Service is working correctly! \ud83d\udfe2" });
    }
}
