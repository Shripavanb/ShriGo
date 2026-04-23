using Google.Analytics.Data.V1Beta;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly BetaAnalyticsDataClient _client;

    public AnalyticsController()
    {
        _client = BetaAnalyticsDataClient.Create();
    }

    [HttpGet("active-users")]
    public async Task<IActionResult> GetActiveUsers()
    {
        try
        {
            var path = Path.Combine(AppContext.BaseDirectory, "google-credentials.json");

            if (!System.IO.File.Exists(path))
            {
                return Ok(new { error = "Credentials file NOT found", path });
            }

            var client = BetaAnalyticsDataClient.Create();

            var request = new RunRealtimeReportRequest
            {
                Property = "properties/502473110", // 🔥 replace with REAL property ID
                Metrics = { new Metric { Name = "activeUsers" } }
            };

            var response = await client.RunRealtimeReportAsync(request);

            var users = response.Rows.Count > 0
                ? response.Rows[0].MetricValues[0].Value
                : "0";

            return Ok(new { activeUsers = users });
        }
        catch (Exception ex)
        {
            return Ok(new
            {
                error = ex.Message,
                inner = ex.InnerException?.Message,
                stack = ex.StackTrace
            });
        }
    }
}