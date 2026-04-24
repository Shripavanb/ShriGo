using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Analytics.Data.V1Beta;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private static int _cachedUsers = 0;
    private static DateTime _lastFetch = DateTime.MinValue;
    private BetaAnalyticsDataClient CreateClient()
    {
        var json = Environment.GetEnvironmentVariable("GOOGLE_CREDENTIALS_JSON");

        if (string.IsNullOrEmpty(json))
            throw new Exception("Google credentials not found");

        var credential = GoogleCredential
            .FromJson(json)
            .CreateScoped("https://www.googleapis.com/auth/analytics.readonly");

        return new BetaAnalyticsDataClientBuilder
        {
            Credential = credential
        }.Build();
    }

    //private BetaAnalyticsDataClient CreateClient()
    //{
    //    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "google-credentials1.json");

    //    var credential = GoogleCredential
    //        .FromFile(path)
    //        .CreateScoped("https://www.googleapis.com/auth/analytics.readonly");

    //    return new BetaAnalyticsDataClientBuilder
    //    {
    //        Credential = credential
    //    }.Build();
    //}

    //private static string _cachedUsers = "0";
    //private static DateTime _lastFetch = DateTime.MinValue;

    //[HttpGet("active-users")]
    //public async Task<IActionResult> GetActiveUsers()
    //{
    //    if ((DateTime.UtcNow - _lastFetch).TotalSeconds < 30)
    //        return Ok(new { activeUsers = _cachedUsers });

    //    var client = CreateClient();

    //    var response = await client.RunRealtimeReportAsync(new RunRealtimeReportRequest
    //    {
    //        Property = "properties/502473110",
    //        Metrics = { new Metric { Name = "activeUsers" } }
    //    });

    //    var users = response.Rows.Count > 0
    //        ? response.Rows[0].MetricValues[0].Value
    //        : "0";

    //    _cachedUsers = users;
    //    _lastFetch = DateTime.UtcNow;

    //    return Ok(new { activeUsers = users });
    //}




    //For Active users online only-Working Code both on locall and on server 
    [HttpGet("active-users")]
    public async Task<IActionResult> GetActiveUsers()
    {
        try
        {
            // ✅ Cache check (FIRST)
            if ((DateTime.UtcNow - _lastFetch).TotalSeconds < 15)
            {
                return Ok(new { activeUsers = _cachedUsers });
            }

            var client = CreateClient();

            var response = await client.RunRealtimeReportAsync(new RunRealtimeReportRequest
            {
                Property = "properties/502473110",
                Metrics = { new Metric { Name = "activeUsers" } }
            });

            var value = response.Rows.FirstOrDefault()?.MetricValues.FirstOrDefault()?.Value;

            int.TryParse(value, out int users);

            // ✅ Update cache
            _cachedUsers = users;
            _lastFetch = DateTime.UtcNow;

            return Ok(new { activeUsers = users });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("API is working");
    }

    //For Active user, Total views, 
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        try
        {
            var client = CreateClient();

            // Realtime
            var realtime = await client.RunRealtimeReportAsync(new RunRealtimeReportRequest
            {
                Property = "properties/502473110",
                Metrics = { new Metric { Name = "activeUsers" } }
            });

            int.TryParse(
                realtime.Rows.FirstOrDefault()?.MetricValues.FirstOrDefault()?.Value,
                out int activeUsers
            );

            // Report
            var report = await client.RunReportAsync(new RunReportRequest
            {
                Property = "properties/502473110",
                DateRanges =
            {
                new DateRange { StartDate = "7daysAgo", EndDate = "today" }
            },
                Metrics =
            {
                new Metric { Name = "screenPageViews" },
                new Metric { Name = "totalUsers" }
            }
            });

            int.TryParse(report.Rows.FirstOrDefault()?.MetricValues[0]?.Value, out int pageViews);
            int.TryParse(report.Rows.FirstOrDefault()?.MetricValues[1]?.Value, out int totalUsers);

            return Ok(new
            {
                activeUsers,
                pageViews,
                totalUsers
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}