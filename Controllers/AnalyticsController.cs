using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("API working");
    }
}