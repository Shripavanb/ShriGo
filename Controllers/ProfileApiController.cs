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
            "{userId}/{role}"
        )]

        public async Task<IActionResult>
            GetProfile(

            int userId,
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
            )
            {

                var driver =
                    await _context
                    .UserTb
                    .FirstOrDefaultAsync(

                        x =>
                        x.UserId ==
                        userId
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
                        userId =
                            driver.UserId,

                        uniqueId =
                            driver.UserUniqueId,

                        firstName =
                            driver.UserFirstName,

                        lastName =
                            driver.UserLastName,

                        age =
                            driver.UserAge,

                        email =
                            driver.UserEmail,

                        phone =
                            driver.UserContact,

                        role =
                            driver.UserRole,

                        imagePath =
                            driver
                                .UserImagePath,

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
                        userId
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
                        userId =
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