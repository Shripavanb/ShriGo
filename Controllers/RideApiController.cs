using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;

namespace ShriGo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideApiController : ControllerBase
    {
        private readonly RideDBContext _dbContext;

        public RideApiController(RideDBContext context)
        {
            _dbContext = context;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveRides()
        {
            var now =
                DateTime.Now;

            var rides = await _dbContext
                .Ride_DBTable
                .Where(r =>

                    r.RideDate != null &&
                    r.RideTime != null &&

                    r.RideDate
                        .Value
                        .ToDateTime(

                            r.RideTime.Value
                        )
                        .AddHours(2)

                        >= now
                )
                .OrderBy(r => r.RideDate)
                .ThenBy(r => r.RideTime)
                .ToListAsync();

            return Ok(rides);
        }


        [HttpGet("history")]
        public async Task<IActionResult> GetRideHistory()
        {
            var now =
                DateTime.Now;

            var rides = await _dbContext
                .Ride_DBTable
                .Where(r =>

                    r.RideDate != null &&
                    r.RideTime != null &&

                    r.RideDate
                        .Value
                        .ToDateTime(

                            r.RideTime.Value
                        )
                        .AddHours(2)

                        < now
                )
                .OrderByDescending(r => r.RideDate)
                .ThenByDescending(r => r.RideTime)
                .ToListAsync();

            return Ok(rides);
        }
        [HttpGet("Expired")]
        public async Task<IActionResult> GetExpiredRides()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var oldRides = await _dbContext
                .Ride_DBTable
                .Where(r => r.RideDate < today)
                .OrderByDescending(r => r.RideDate)
                .ThenByDescending(r => r.RideTime)
                .ToListAsync();

            return Ok(oldRides);
        }
    }
}