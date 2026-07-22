using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;

namespace ShriGo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteRouteApiController : ControllerBase
    {
        private readonly RideDBContext _dBContext;

        public FavoriteRouteApiController(RideDBContext context)
        {
            _dBContext = context;
        }

        // Save Favorite
        [HttpPost("SaveFavorite")]
        public async Task<IActionResult> SaveFavorite(
          SaveFavoriteRequest request)
        {
            try
            {
                var exists = await _dBContext.FavoriteRoutesTb.AnyAsync(x =>
                    x.DriverUniqueId == request.DriverUniqueId &&
                    x.RouteName == request.RouteName &&
                    x.IsActive);
                if (exists)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Favorite Route already exists."
                    });
                }
                FavoriteRouteModel favorite =
                        new FavoriteRouteModel
                        {
                            DriverUniqueId = request.DriverUniqueId,
                            RouteName = request.RouteName,
                            RideFrom = request.RideFrom,
                            RideVia = request.RideVia,
                            RideTo = request.RideTo,
                            RideTime = request.RideTime,
                            RidePrice = request.RidePrice,
                            RideSeats = request.RideSeats,
                            IsActive = true,
                            CreatedDate = DateTime.Now
                        };

                    _dBContext.FavoriteRoutesTb.Add(favorite);

                    await _dBContext.SaveChangesAsync();

                    return Ok(new
                    {
                        success = true,
                        message = "Favorite Route saved successfully."
                    });
                
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // Get Driver Favorites
        [HttpGet("GetFavorites/{driverUniqueId}")]
        public async Task<IActionResult> GetFavorites(string driverUniqueId)
        {
            try
            {
                var favorites =
                    await _dBContext.FavoriteRoutesTb
                        .Where(r =>
                                r.DriverUniqueId == driverUniqueId &&
                                r.IsActive)
                      .OrderByDescending(r => r.CreatedDate)
                      .ThenByDescending(r => r.RideTime)
                      .ToListAsync();

                return Ok(favorites);
            }

            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    ex.Message
                );
            }
        }

        // Delete Favorite
        [HttpDelete("DeleteFavorite/{id}")]
        public async Task<IActionResult> DeleteFavorite(int id)
        {
            try
                {
                    var fav =

                        await _dBContext.FavoriteRoutesTb
                            .FirstOrDefaultAsync(x =>
                                x.Id ==
                                id
                            );

                    if (
                        fav == null
                    )
                    {
                       return NotFound(new
                                    {
                                        success = false,
                                        message = "Favorite Route not found."
                                    });
                    }

                    _dBContext.FavoriteRoutesTb
                         .Remove( fav);

                    await _dBContext
                        .SaveChangesAsync();

                    return Ok(

                        new
                        {
                            success = true,
                            message =
                                "Favorite deleted successfully"
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


        // Update Favorite
        [HttpPut("UpdateFavorite/{favId}")]
        public async Task<IActionResult> UpdateFavorite(
        int favId,
        [FromBody] FavoriteRouteModel request)

        {
            var fav = await _dBContext.FavoriteRoutesTb
             .FirstOrDefaultAsync(r => r.Id == favId);

           

               if (fav == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Favorite Route not found."
                    });
                }

                var exists =
               await _dBContext.FavoriteRoutesTb.AnyAsync(x =>

                   x.DriverUniqueId == fav.DriverUniqueId &&
                   x.RouteName == request.RouteName &&
                   x.Id != favId &&
                   x.IsActive);
            if (exists)
            {
                return Ok(new
                {
                    success = false,
                    message = "Favorite Route already exists."
                });
            }
           
                fav.RouteName = request.RouteName;
                fav.RideFrom = request.RideFrom;
                fav.RideVia = request.RideVia;
                fav.RideTo = request.RideTo;
                fav.RideTime = request.RideTime;
                fav.RideSeats = request.RideSeats;
                fav.RidePrice = request.RidePrice;
                fav.ModifiedDate = DateTime.Now;

            // Don't change DriverUniqueId
            // Don't change CreatedDate

            await _dBContext.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Favorite Route updated successfully."
                });
            
        }
    }
}
