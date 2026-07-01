using Microsoft.AspNetCore.Mvc;
using ShriGo.Model;

namespace ShriGo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VersionApiController : ControllerBase
    {
        [HttpGet("Latest")]
        public IActionResult GetLatestVersion()
        {
            VersionModel version = new VersionModel
            {
                LatestVersion = "20.3.0",
                MinimumVersion = "20.2.9",
                ForceUpdate = false,
                Message = "A new version of ShriGo is available.",
                PlayStoreUrl = "https://play.google.com/store/apps/details?id=in.shrigo.app"
            };

            return Ok(version);
        }
    }
}
