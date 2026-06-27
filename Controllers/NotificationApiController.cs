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


        //---------------------------------
        //Get Notifications
        //---------------------------------
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
        //---------------------------------
        //Get Notification Count
        //---------------------------------
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

        //---------------------------------
        //Mark all as Read 
        //---------------------------------
        [HttpPost("markallread/{UserUniqueId}")]

        public async Task<IActionResult> MarkAllRead(
    string UserUniqueId)
        {
            var notifications = await _dbContext.NotificationTb
                .Where(x =>
                    x.UserUniqueId == UserUniqueId &&
                    !x.IsRead)
                .ToListAsync();

            if (!notifications.Any())
            {
                return Ok(new
                {
                    success = true,
                    message = "No unread notifications."
                });
            }

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Notifications marked as read."
            });
        }
    }
}
