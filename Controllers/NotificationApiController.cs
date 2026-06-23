using Microsoft.AspNetCore.Mvc;
using ShriGo.Model;
using Microsoft.EntityFrameworkCore;

namespace ShriGo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationApiController : ControllerBase
    {
        private readonly RideDBContext _dbContext;

        public NotificationApiController(RideDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{DriverUniqueId}")]
        public async Task<IActionResult> GetNotifications(
            string DriverUniqueId)
        {
            var notifications = await _dbContext.NotificationTb
                .Where(x => x.DriverUniqueId == DriverUniqueId)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return Ok(notifications);
        }
    }
}
