using Google.Analytics.Data.V1Beta;
using Google.Apis.Auth.OAuth2;
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
            // 👇 REPLACE CLIENT CREATION HERE
            var json = Environment.GetEnvironmentVariable("GOOGLE_CREDENTIALS_JSON");
            var credential = GoogleCredential.FromJson(json);

            var client = new BetaAnalyticsDataClientBuilder
            {
                Credential = credential
            }.Build();

            var request = new RunRealtimeReportRequest
            {
                Property = "properties/502473110",
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
}