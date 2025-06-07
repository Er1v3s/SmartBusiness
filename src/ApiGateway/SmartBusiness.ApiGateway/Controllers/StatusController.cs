using Microsoft.AspNetCore.Mvc;

namespace SmartBusiness.ApiGateway.Controllers
{
    [Route("/status")]
    [ApiController]
    public class StatusController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public StatusController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult GetServiceStatus() => Ok(new { status = "API Gateway is working correctly! \ud83d\udfe2" });

        [HttpGet]
        [Route("all-services")]
        public async Task<IActionResult> GetAllServicesStatus()
        {
            var client = _httpClientFactory.CreateClient();

            var services = new Dictionary<string, string>
            {
                ["AccountService"] = "http://account.smart-business:2100/status",
                ["SalesService"] = "http://sales.smart-business:2200/status",
                ["WriteService"] = "http://write.smart-business:2300/status",
                ["ReadService"] = "http://read.smart-business:2400/status",
                ["Frontend Web App"] = "http://frontend.smart-business:80/index.html",
                // Add another services
            };

            var results = new Dictionary<string, string>();

            foreach (var service in services)
            {
                try
                {
                    var response = await client.GetAsync(service.Value);
                    results[service.Key] =
                        response.IsSuccessStatusCode ? "Healthy \ud83d\udfe2" : "Unhealthy \ud83d\udd34";
                }
                catch
                {
                    results[service.Key] = "Unavailable \u274c";
                }
            }

            return Ok(results);
        }
    }
}