using Microsoft.AspNetCore.Mvc;

namespace AccountService.Api.Controllers
{
    [ApiController]
    [Route("status")]
    public class StatusController : Controller
    {
        [HttpGet]
        public IActionResult GetServiceStatus() => Ok(new { status = "Auth Service is working correctly! \ud83d\udfe2" });
    }
}