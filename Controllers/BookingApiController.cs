using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;

[ApiController]
[Route("api/[controller]")]
public class BookingApiController : ControllerBase
{
    private readonly RideDBContext _dbContext;

    public BookingApiController(
        RideDBContext context
    )
    {
        _dbContext = context;
    }

    //--------------------------------------------------
    // BOOK RIDE
    //--------------------------------------------------
    [HttpPost("bookride")]
    public async Task<IActionResult> BookRide( [FromBody]   BookRideRequest request)
    {
        try
        {
            //var ride = await _dbContext
            //    .Ride_DBTable
            //    .FirstOrDefaultAsync(
            //        r => r.RideId ==
            //        request.RideId
            //    );
            var ride = await _dbContext.Ride_DBTable.FirstOrDefaultAsync(e => e.RideId== request.RideId);

            if (ride == null)
            {
                return NotFound(
                    "Ride not found"
                );
            }

            int availableSeats =
                int.Parse(
                    ride.RideSeats
                );

            if (
                availableSeats <
                request.BookedSeats
            )
            {
                return BadRequest(
              new
              {
                  success = false,
                  message =
                      "Not enough seats"
              }
               );
            }

            //-----------------------------------
            // Reduce seats
            //-----------------------------------

            availableSeats -=
                request.BookedSeats;

            ride.RideSeats =
                availableSeats
                    .ToString();

            //-----------------------------------
            // Create booking
            //-----------------------------------

            var booking =
                new BookingsModel
                {
                    RideId =
                        ride.RideId
                            .ToString(),

                    RideDate =
                        ride.RideDate,

                    RideSource =
                        ride.RideSource,

                    RideDesti =
                        ride.RideDesti,

                    RideVia =
                        ride.RideVia,

                    RideTime =
                        ride.RideTime,

                    BookedSeats =
                        request.BookedSeats
                            .ToString(),

                    RidePrice =
                        (
                            int.Parse(
                                ride.RidePrice
                            ) *
                            request.BookedSeats
                        ).ToString(),

                    DriverContact =
                        ride.DriverContact,

                    DriverUniqueId =
                        ride.DriverUniqueId,

                    DriverFirstName =
                        ride.DriverFirstName,

                    PassengerFirstName =
                        request.PassengerFirstName,

                    PassengerUniqueId =
                        request.PassengerUniqueId,

                    PassengerContact =
                        request.PassengerContact,

                    PassengerEmail =
                        request.PassengerEmail
                };
            var lastBooking =

            await _dbContext
                .Bookings_DBTable
                .OrderByDescending(
                    b => b.BookingId
                )
        .FirstOrDefaultAsync();

            booking.BookingId =

                lastBooking == null
                    ? 1
                    : lastBooking
                        .BookingId + 1;
            _dbContext
                .Bookings_DBTable
                .Add(booking);

            await _dbContext
                .SaveChangesAsync();

            return Ok(
            new
            {
                success = true,
                message =
                    "Ride booked successfully"
            }
             );
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new
                {
                    success = false,
                    message =
                        ex.InnerException
                            ?.Message
                        ?? ex.Message
                }
            );
        }
    }


    //--------------------------------------------------
    // GET MY BOOKINGS
    //--------------------------------------------------

    [HttpGet("mybookings/{passengerUniqueId}")]
    public async Task<IActionResult>
    GetMyBookings(

        string passengerUniqueId

    )
    {
        try
        {
            var bookings =

                await _dbContext
                    .Bookings_DBTable
                    .Where(

                        b =>

                        b.PassengerUniqueId
                        == passengerUniqueId
                    )
                    .OrderByDescending(

                        b => b.RideDate
                    )
                    .ThenByDescending(

                        b => b.RideTime
                    )
                    .ToListAsync();

            return Ok(
                bookings
            );
        }

        catch (
            Exception ex
        )
        {
            return StatusCode(

                500,

                ex.Message
            );
        }
    }
}