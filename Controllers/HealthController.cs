using Microsoft.AspNetCore.Mvc;

namespace ApigeeExplorer.ApiV2.Controllers
{
    /// <summary>
    /// Controller for health check operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Health check endpoint
        /// </summary>
        /// <returns>Health status information</returns>
        [HttpGet]
        public ActionResult<object> GetHealth()
        {
            try
            {
                var healthInfo = new
                {
                    status = "OK",
                    timestamp = DateTime.UtcNow,
                    message = "Apigee Explorer API V2 is running (.NET)",
                    version = "2.0.0",
                    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
                };

                _logger.LogInformation("Health check requested");
                return Ok(healthInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return StatusCode(500, new { 
                    status = "ERROR", 
                    timestamp = DateTime.UtcNow,
                    message = "Health check failed" 
                });
            }
        }
    }
}