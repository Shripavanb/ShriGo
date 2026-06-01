using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShriGo.Helpers;
using ShriGo.Model;

namespace ShriGo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RideApiController : ControllerBase
    {
        private readonly RideDBContext _dbContext;

        public RideApiController(
            RideDBContext context
        )
        {
            _dbContext = context;
        }

        //--------------------------------------------------
        // GET ACTIVE RIDES
        //--------------------------------------------------
        [HttpGet("active")]
        public async Task<IActionResult>
            GetActiveRides()
        {
            var now = TimeHelper.GetIndiaTime();

            var rides =
                await _dbContext
                    .Ride_DBTable
                    .ToListAsync();

            var activeRides =
                rides
                    .Where(r =>

                        r.RideDate != null &&
                        r.RideTime != null &&

                        r.RideDate.Value
                            .ToDateTime(
                                r.RideTime.Value
                            )
                            .AddHours(2)

                            >= now
                    )
                    .OrderBy(
                        r => r.RideDate
                    )
                    .ThenBy(
                        r => r.RideTime
                    )
                    .ToList();

            return Ok(
                activeRides
            );
        }

        //--------------------------------------------------
        // GET HISTORY RIDES
        //--------------------------------------------------
        [HttpGet("history")]
        public async Task<IActionResult>
            GetRideHistory()
        {
            var now = TimeHelper.GetIndiaTime();

            var rides =
                await _dbContext
                    .Ride_DBTable
                    .ToListAsync();

            var historyRides =
                rides
                    .Where(r =>

                        r.RideDate != null &&
                        r.RideTime != null &&

                        r.RideDate.Value
                            .ToDateTime(
                                r.RideTime.Value
                            )
                            .AddHours(2)

                            < now
                    )
                    .OrderByDescending(
                        r => r.RideDate
                    )
                    .ThenByDescending(
                        r => r.RideTime
                    )
                    .ToList();

            return Ok(
                historyRides
            );
        }

        //--------------------------------------------------
        // UPLOAD RIDE
        //--------------------------------------------------

        [HttpPost("upload")]
        public async Task<IActionResult>
        UploadRide(

        [FromBody]
        SortedRideModel newRide
        )
        {
            try
            {
                if (newRide == null)
                {
                    return BadRequest(
                        "Ride data missing"
                    );
                }

                //-----------------------------------
                // Remove expired rides
                //-----------------------------------
                var cutoffDate =
                    TimeHelper
                        .GetIndiaDate();
                var oldRides =
                   await _dbContext
                       .Ride_DBTable
                       .Where(r =>

                           r.RideDate != null
                           &&

                           r.RideDate
                           < cutoffDate
                       )
                       .ToListAsync();

                if (oldRides.Any())
                {
                    _dbContext
                        .Ride_DBTable
                        .RemoveRange(
                            oldRides
                        );
                }

                //-----------------------------------
                // Save new ride
                //-----------------------------------
                bool isTableEmpty =
                !_dbContext
                    .Ride_DBTable
                    .Any();

                if (isTableEmpty)
                {
                    newRide.RideId = 1;
                }
                else
                {
                    var maxRideId =
                        _dbContext
                            .Ride_DBTable
                            .Max(r => r.RideId);

                    newRide.RideId =
                        maxRideId + 1;
                }
                await _dbContext
                    .Ride_DBTable
                    .AddAsync(
                        newRide
                    );

                await _dbContext
                    .SaveChangesAsync();

                return Ok(
                    new
                    {
                        success = true,
                        message =
                            "Ride uploaded successfully"
                    }
                );
            }

            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    ex.InnerException?.Message
                    ?? ex.Message
                );
            }
        }


        //--------------------------------------------------
        // GET MY RIDES
        //--------------------------------------------------

        [HttpGet("myrides/{driverUniqueId}")]
        public async Task<IActionResult>
        GetMyRides(
            string driverUniqueId
        )
        {
            try
            {
                var rides =
                    await _dbContext
                        .Ride_DBTable
                        .Where(r =>

                            r.DriverUniqueId ==
                            driverUniqueId
                        )
                        .OrderByDescending(
                            r => r.RideDate
                        )
                        .ThenByDescending(
                            r => r.RideTime
                        )
                        .ToListAsync();

                return Ok(rides);
            }

            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    ex.Message
                );
            }
        }
    }
}