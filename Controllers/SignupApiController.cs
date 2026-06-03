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
            PassengerModel request
        )
        {
            try
            {
                //----------------------------------
                // Password Validation
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

                //----------------------------------
                // Duplicate Phone Check
                //----------------------------------
                var existingUser =
                    _dbContext.PassengerTb
                    .Any(x =>
                        x.PassengerContact
                        ==
                        request.PassengerContact
                    );

                if (existingUser)
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

                //----------------------------------
                // Passenger Id
                //----------------------------------
                request.PassengerId =
                    _dbContext.PassengerTb.Any()
                    ?
                    _dbContext.PassengerTb
                    .Max(r => r.PassengerId) + 1
                    :
                    1;

                //----------------------------------
                // Password Hash
                //----------------------------------
                var passwordHelper =
                    new PasswordHelper();

                request.PassengerPswd =
                    passwordHelper
                    .HashPassword(
                        request.PassengerPswd
                    );

                //----------------------------------
                // Unique Id
                //----------------------------------
                request.PassengerUniqueId =
                    _random.Next(
                        10000,
                        100000
                    ).ToString();

                //----------------------------------
                // Registration Date
                //----------------------------------
                request.PassengerRegDate =
                    TimeHelper.GetIndiaDate();

                request.Role =
                    "Passenger";

                request.AcceptedTerms =
                    true;

                //----------------------------------
                // Save
                //----------------------------------
                _dbContext.PassengerTb
                    .Add(request);

                _dbContext.SaveChanges();

                return Ok(
                    new
                    {
                        success = true,
                        message =
                            "Signup Successful"
                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new
                    {
                        success = false,
                        message = ex.Message
                    }
                );
            }
        }
    }
}

