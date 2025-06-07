using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace SmartBusiness.ApiGateway.Controllers
{
    [Route("/health")]
    [ApiController]
    public class HealthController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public HealthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllServicesStatus()
        {
            var httpClient = _httpClientFactory.CreateClient();

            var services = new Dictionary<string, string>
            {
                ["AccountService"] = "http://account.smart-business:2100/health",
                ["SalesService"] = "http://sales.smart-business:2200/health",
                ["WriteService"] = "http://write.smart-business:2300/health",
                ["ReadService"] = "http://read.smart-business:2400/health",
                ["Frontend Web App"] = "http://frontend.smart-business:80/index.html"
            };

            var summary = new Dictionary<string, string>();
            var details = new Dictionary<string, object>();

            foreach (var service in services)
            {
                var sw = Stopwatch.StartNew();
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(service.Value);
                    sw.Stop();

                    string content = await response.Content.ReadAsStringAsync();

                    JsonDocument? jsonDoc;
                    try
                    {
                        jsonDoc = JsonDocument.Parse(content);
                    }
                    catch
                    {
                        jsonDoc = null!;
                    }

                    string displayStatus = "";
                    if (jsonDoc != null && jsonDoc.RootElement.TryGetProperty("message", out JsonElement msgElement))
                    {
                        displayStatus = msgElement.GetString() + " 🟢";
                    }
                    else
                    {
                        displayStatus = response.IsSuccessStatusCode ? "Healthy 🟢" : $"Unhealthy 🚫 ({(int)response.StatusCode})";
                    }

                    var detailEntry = new Dictionary<string, object?>
                    {
                        ["status"] = jsonDoc != null && jsonDoc.RootElement.TryGetProperty("status", out JsonElement statusElement)
                                     ? statusElement.GetString()
                                     : (response.IsSuccessStatusCode ? "Healthy" : "Unhealthy"),
                        ["totalDuration"] = sw.Elapsed.ToString()
                    };

                    if (jsonDoc != null && jsonDoc.RootElement.TryGetProperty("entries", out JsonElement entriesElement))
                    {
                        try
                        {
                            var entries = JsonSerializer.Deserialize<Dictionary<string, object>>(entriesElement.GetRawText());
                            detailEntry["entries"] = entries;
                        }
                        catch
                        {
                            detailEntry["entries"] = new Dictionary<string, object>();
                        }
                    }
                    else
                    {
                        detailEntry["entries"] = new Dictionary<string, object>();
                    }

                    summary[service.Key] = displayStatus;
                    details[service.Key] = detailEntry;
                }
                catch (Exception ex)
                {
                    sw.Stop();
                    summary[service.Key] = "Unhealthy 🚫 (Exception)";
                    details[service.Key] = new Dictionary<string, object>
                    {
                        ["error"] = ex.Message
                    };
                }
            }

            var resultObject = new
            {
                summary,
                details
            };

            return Ok(resultObject);
        }
    }
}