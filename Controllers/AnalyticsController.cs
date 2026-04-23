using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Analytics.Data.V1Beta;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private BetaAnalyticsDataClient CreateClient()
    {
        var json = Environment.GetEnvironmentVariable("GOOGLE_CREDENTIALS_JSON");

        if (string.IsNullOrEmpty(json))
            throw new Exception("Google credentials not found");

        var credential = GoogleCredential.FromJson(json);

        return new BetaAnalyticsDataClientBuilder
        {
            Credential = credential
        }.Build();
    }
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
            var client = CreateClient();

            var request = new RunRealtimeReportRequest
            {
                Property = "properties/502473110", // 🔥 your GA property ID
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
            return Ok(new { error = ex.Message });
        }
    }


    //For Active user, Total views, 
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        try
        {
            var client = CreateClient();

            // 🔴 Realtime (active users)
            var realtimeRequest = new RunRealtimeReportRequest
            {
                Property = "properties/502473110",
                Metrics = { new Metric { Name = "activeUsers" } }
            };

            var realtimeResponse = await client.RunRealtimeReportAsync(realtimeRequest);

            var activeUsers = realtimeResponse.Rows.Count > 0
                ? int.Parse(realtimeResponse.Rows[0].MetricValues[0].Value)
                : 0;

            // 🔵 Standard report (page views + total users)
            var reportRequest = new RunReportRequest
            {
                Property = "properties/502473110",
                DateRanges =
            {
                new DateRange
                {
                    StartDate = "2026-04-17", // 👉 change to your launch date
                    EndDate = "today"
                }
            },
                Metrics =
            {
                new Metric { Name = "screenPageViews" },
                new Metric { Name = "totalUsers" }
            }
            };

            var reportResponse = await client.RunReportAsync(reportRequest);

            var pageViews = reportResponse.Rows.Count > 0
                ? int.Parse(reportResponse.Rows[0].MetricValues[0].Value)
                : 0;

            var totalUsers = reportResponse.Rows.Count > 0
                ? int.Parse(reportResponse.Rows[0].MetricValues[1].Value)
                : 0;

            return Ok(new
            {
                activeUsers,
                pageViews,
                totalUsers
            });
        }
        catch (Exception ex)
        {
            return Ok(new { error = ex.Message });
        }
    }
}