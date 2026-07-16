using Microsoft.AspNetCore.Mvc;
//using ShriGo.Data;
//using ShriGo.Helper;
using ShriGo.Helpers;
using ShriGo.Model;
using ShriGo.Pages.Helpers;

namespace ShriGo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignupApiController : ControllerBase
    {
        private readonly RideDBContext _dbContext;
        private static readonly Random _random =
            new Random();

        public SignupApiController(
            RideDBContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult Signup(
            [FromBody]
            SignupRequest request
        )
        {
            Console.WriteLine("===== DRIVER REQUEST =====");
            Console.WriteLine($"DriverRole      : {request.DriverRole}");
            Console.WriteLine($"DriverFirstName : {request.DriverFirstName}");
            Console.WriteLine($"DriverContact   : {request.DriverContact}");
            Console.WriteLine($"DriverPswd      : {request.DriverPswd}");

            Console.WriteLine("===== PASSENGER REQUEST =====");
            Console.WriteLine($"Role            : {request.Role}");
            Console.WriteLine($"PassengerContact: {request.PassengerContact}");
            Console.WriteLine($"PassengerPswd   : {request.PassengerPswd}");
            try
            {
                //----------------------------------
                // DRIVER SIGNUP
                //----------------------------------

                if (request.DriverRole == "Driver")
                {
                    if (
                        string.IsNullOrWhiteSpace(
                            request.DriverPswd
                        )
                        ||
                        request.DriverPswd.Length < 8
                    )
                    {
                        return BadRequest(
                            new
                            {
                                success = false,
                                message =
                                    "Password must be minimum 8 characters"
                            }
                        );
                    }

                    var existingDriver =
                        _dbContext.DriversTb
                        .Any(x =>
                            x.DriverContact ==
                            request.DriverContact
                        );

                    if (existingDriver)
                    {
                        return BadRequest(
                            new
                            {
                                success = false,
                                message =
                                    "Phone number already registered"
                            }
                        );
                    }

                    var passwordHelper =
                        new PasswordHelper();
                    var nextDriverId =
                            _dbContext.DriversTb.Any()
                            ?
                            _dbContext.DriversTb
                                .Max(x => x.DriverId) + 1
                            :
                            1;
                    var driver =
                        new DriverModel
                        {
                            DriverId = nextDriverId,
                            DriverFirstName =
                                request.DriverFirstName,

                            DriverLastName =
                                request.DriverLastName,

                            DriverAge =
                                request.DriverAge,

                            DriverEmail =
                                request.DriverEmail,

                            DriverContact =
                                request.DriverContact,

                            DriverPswd =
                                passwordHelper
                                .HashPassword(
                                    request.DriverPswd
                                ),

                            DriverRole =
                                "Driver",

                            Subscription =
                                request.Subscription,

                            VehicleRegNo =
                                request.VehicleRegNo,

                            VehicleInsur =
                                request.VehicleInsur,

                            VehicleModel =
                                request.VehicleModel,

                            AcceptedTerms =
                                true,

                            DriverUniqueId =
                                _random.Next(
                                    10000,
                                    100000
                                ).ToString(),

                            DriverRegDate =
                                TimeHelper.GetIndiaDate()
                        };

                    _dbContext.DriversTb
                        .Add(driver);

                    _dbContext.SaveChanges();

                    return Ok(
                        new
                        {
                            success = true,
                            message =
                                "Driver Signup Successful"
                        }
                    );
                }

                //----------------------------------
                // PASSENGER SIGNUP
                //----------------------------------

                if (
                    string.IsNullOrWhiteSpace(
                        request.PassengerPswd
                    )
                    ||
                    request.PassengerPswd.Length < 8
                )
                {
                    return BadRequest(
                        new
                        {
                            success = false,
                            message =
                                "Password must be minimum 8 characters"
                        }
                    );
                }

                var existingPassenger =
                    _dbContext.PassengerTb
                    .Any(x =>
                        x.PassengerContact ==
                        request.PassengerContact
                    );

                if (existingPassenger)
                {
                    return BadRequest(
                        new
                        {
                            success = false,
                            message =
                                "Phone number already registered"
                        }
                    );
                }

                var passengerPasswordHelper =
                    new PasswordHelper();
                var nextPassengerId =
                        _dbContext.PassengerTb.Any()
                        ?
                        _dbContext.PassengerTb
                            .Max(x => x.PassengerId) + 1
                        :
                        1;
                var passenger =
                    new PassengerModel
                    {
                        PassengerId =
                            nextPassengerId,
                        PassengerFirstName =
                            request.PassengerFirstName,

                        PassengerLastName =
                            request.PassengerLastName,

                        PassengerAge =
                            request.PassengerAge,

                        PassengerEmail =
                            request.PassengerEmail,

                        PassengerContact =
                            request.PassengerContact,

                        PassengerPswd =
                            passengerPasswordHelper
                            .HashPassword(
                                request.PassengerPswd
                            ),

                        Role =
                            "Passenger",

                        AcceptedTerms =
                            true,

                        PassengerUniqueId =
                            _random.Next(
                                10000,
                                100000
                            ).ToString(),

                        PassengerRegDate =
                            TimeHelper.GetIndiaDate()
                    };

                _dbContext.PassengerTb
                    .Add(passenger);

                _dbContext.SaveChanges();

                return Ok(
                    new
                    {
                        success = true,
                        message =
                            "Passenger Signup Successful"
                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new
                    {
                        success = false,
                        message =
                            ex.InnerException?.Message
                            ?? ex.Message
                    }
                );
            }
        }
    }
}

