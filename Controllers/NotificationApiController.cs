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

        [HttpGet("{UserUniqueId}")]
        public async Task<IActionResult> GetNotifications(
    string UserUniqueId)
        {
            var notifications = await _dbContext.NotificationTb
                .Where(x => x.UserUniqueId == UserUniqueId)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return Ok(notifications);
        }

        [HttpGet("count/{UserUniqueId}")]
        public async Task<IActionResult> GetNotificationCount(
            string UserUniqueId)
        {
            var count = await _dbContext.NotificationTb
                .CountAsync(x =>
                    x.UserUniqueId == UserUniqueId &&
                    !x.IsRead);

            return Ok(new
            {
                count
            });
        }
    }
}
