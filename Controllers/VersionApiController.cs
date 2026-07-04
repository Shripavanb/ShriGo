using Microsoft.AspNetCore.Mvc;
using ShriGo.Model;

namespace ShriGo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VersionApiController : ControllerBase
    {
        private readonly RideDBContext _dBContext;

        public VersionApiController(
            RideDBContext context
        )
        {
            _dBContext = context;
        }

        [HttpGet("Latest")]
        public IActionResult GetLatestVersion()
        {
            var configuration = _dBContext.AppConfigurationTb
                .FirstOrDefault(x => x.IsActive);

            if (configuration == null)
                return NotFound(new
                {
                    Success = false,
                    Message = "Application configuration not found."
                });

            return Ok(configuration);
        }
    }
}