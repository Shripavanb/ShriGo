using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;

namespace ShriGo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileApiController
        : ControllerBase
    {
        private readonly RideDBContext
            _context;

        public ProfileApiController(

            RideDBContext context
        )
        {
            _context =
                context;
        }

        [HttpGet(
            "{DriverId}/{role}"
        )]

        public async Task<IActionResult>
            GetProfile(

            int DriverId,
            string role
        )
        {

            // ==================
            // DRIVER PROFILE
            // ==================

            if (

                role.Equals(
                    "Driver",
                    StringComparison
                        .OrdinalIgnoreCase
                )

                ||

                role.Equals(
                    "Admin",
                    StringComparison
                        .OrdinalIgnoreCase
                )
            )
            {

                var driver =
                    await _context
                    .DriversTb
                    .FirstOrDefaultAsync(

                        x =>
                        x.DriverId ==
                        DriverId
                    );

                if (
                    driver == null
                )
                {
                    return NotFound(
                        new
                        {
                            message =
                                "Driver not found"
                        }
                    );
                }

                return Ok(
                    new
                    {
                        DriverId =
                            driver.DriverId,

                        uniqueId =
                            driver.DriverUniqueId,

                        firstName =
                            driver.DriverFirstName,

                        lastName =
                            driver.DriverLastName,

                        age =
                            driver.DriverAge,

                        email =
                            driver.DriverEmail,

                        phone =
                            driver.DriverContact,

                        role =
                            driver.DriverRole,

                        imagePath =
                            driver
                                .DriverImagePath,

                        vehicleModel =
                            driver
                                .VehicleModel,

                        vehicleRegNo =
                            driver
                                .VehicleRegNo,

                        vehicleInsur =
                            driver
                                .VehicleInsur,

                        subscription =
                            driver
                                .Subscription
                    }
                );
            }

            // ==================
            // PASSENGER PROFILE
            // ==================

            else if (

                role.Equals(
                    "Passenger",
                    StringComparison
                        .OrdinalIgnoreCase
                )
            )
            {

                var passenger =
                    await _context
                    .PassengerTb
                    .FirstOrDefaultAsync(

                        x =>
                        x.PassengerId ==
                        DriverId
                    );

                if (
                    passenger ==
                    null
                )
                {
                    return NotFound(
                        new
                        {
                            message =
                                "Passenger not found"
                        }
                    );
                }

                return Ok(
                    new
                    {
                        DriverId =
                            passenger
                                .PassengerId,

                        uniqueId =
                            passenger
                                .PassengerUniqueId,

                        firstName =
                            passenger
                                .PassengerFirstName,

                        lastName =
                            passenger
                                .PassengerLastName,

                        age =
                            passenger
                                .PassengerAge,

                        email =
                            passenger
                                .PassengerEmail,

                        phone =
                            passenger
                                .PassengerContact,

                        role =
                            passenger
                                .Role,

                        imagePath =
                            passenger
                                .PassengerImagePath,

                        // Driver-only fields
                        vehicleModel =
                            (string?)null,

                        vehicleRegNo =
                            (string?)null,

                        vehicleInsur =
                            (string?)null,

                        subscription =
                            (string?)null
                    }
                );
            }

            return BadRequest(
                new
                {
                    message =
                        "Invalid role"
                }
            );
        }
    }
}