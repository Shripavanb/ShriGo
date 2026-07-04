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
			var version = _context.VersionTb.FirstOrDefault();

			if (version == null)
				return NotFound();

			return Ok(version);
		}
    }
}
