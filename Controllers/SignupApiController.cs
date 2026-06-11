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
            try
            {
                //----------------------------------
                // DRIVER SIGNUP
                //----------------------------------

                if (request.UserRole == "Driver")
                {
                    if (
                        string.IsNullOrWhiteSpace(
                            request.UserPswd
                        )
                        ||
                        request.UserPswd.Length < 8
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
                        _dbContext.UserTb
                        .Any(x =>
                            x.UserContact ==
                            request.UserContact
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

                    var driver =
                        new UserModel
                        {
                            UserFirstName =
                                request.UserFirstName,

                            UserLastName =
                                request.UserLastName,

                            UserAge =
                                request.UserAge,

                            UserEmail =
                                request.UserEmail,

                            UserContact =
                                request.UserContact,

                            UserPswd =
                                passwordHelper
                                .HashPassword(
                                    request.UserPswd
                                ),

                            UserRole =
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

                            UserUniqueId =
                                _random.Next(
                                    10000,
                                    100000
                                ).ToString(),

                            UserRegDate =
                                TimeHelper.GetIndiaDate()
                        };

                    _dbContext.UserTb
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

                var passenger =
                    new PassengerModel
                    {
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
                            ex.Message
                    }
                );
            }
        }
    }
}

