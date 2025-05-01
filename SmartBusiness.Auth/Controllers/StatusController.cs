using Microsoft.AspNetCore.Mvc;

namespace SmartBusiness.Auth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : Controller
    {
        [HttpGet]
        public IActionResult GetServiceStatus() => Ok(new { status = "Auth Service is working correctly! \ud83d\udfe2" });
    }
}