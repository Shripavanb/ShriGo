using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;
using ShriGo.Pages.Helpers;

namespace ShriGo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginApiController : ControllerBase
    {
        private readonly RideDBContext _context;

        public LoginApiController(
            RideDBContext context
        )
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request
        )
        {
            var passwordHelper =
                new PasswordHelper();

            // ==========================
            // DRIVER LOGIN
            // ==========================

            var user = await _context.UserTb
                .FirstOrDefaultAsync(x =>

                    x.UserEmail ==
                    request.EmailOrPhone

                    ||

                    x.UserContact ==
                    request.EmailOrPhone
                );

            if (user != null)
            {
                bool loginSuccess = false;

                // HASHED PASSWORD
                if (
                    passwordHelper.IsHashed(
                        user.UserPswd
                    )
                )
                {
                    loginSuccess =
                        passwordHelper
                        .VerifyPassword(
                            user.UserPswd,
                            request.Password
                        );
                }
                else
                {
                    // OLD PASSWORD SUPPORT
                    loginSuccess =
                        user.UserPswd ==
                        request.Password;
                }

                if (loginSuccess)
                {
                    return Ok(new
                    {
                        success = true,
                        loginType = "Driver",

                        userId =
                            user.UserId,

                        uniqueId =
                            user.UserUniqueId,

                        firstName =
                            user.UserFirstName,

                        lastName =
                            user.UserLastName,

                        phone =
                            user.UserContact,

                        email =
                            user.UserEmail,

                        role =
                            user.UserRole
                    });
                }
            }

            // ==========================
            // PASSENGER LOGIN
            // ==========================

            var passenger =
                await _context.PassengerTb
                .FirstOrDefaultAsync(x =>

                    x.PassengerEmail ==
                    request.EmailOrPhone

                    ||

                    x.PassengerContact ==
                    request.EmailOrPhone
                );

            if (passenger != null)
            {
                bool loginSuccess = false;

                if (
                    passwordHelper.IsHashed(
                        passenger.PassengerPswd
                    )
                )
                {
                    loginSuccess =
                        passwordHelper
                        .VerifyPassword(
                            passenger.PassengerPswd,
                            request.Password
                        );
                }
                else
                {
                    loginSuccess =
                        passenger.PassengerPswd ==
                        request.Password;
                }

                if (loginSuccess)
                {
                    return Ok(new
                    {
                        success = true,
                        loginType =
                            "Passenger",

                        userId =
                            passenger.PassengerId,

                        uniqueId =
                            passenger.PassengerUniqueId,

                        firstName =
                            passenger.PassengerFirstName,

                        lastName =
                            passenger.PassengerLastName,

                        phone =
                            passenger.PassengerContact,

                        email =
                            passenger.PassengerEmail,

                        role =
                            passenger.Role
                    });
                }
            }

            return Unauthorized(new
            {
                success = false,
                message =
                    "Invalid credentials"
            });
        }

    }

    public class LoginRequest
    {
        public string?
            EmailOrPhone
        { get; set; }

        public string?
            Password
        { get; set; }
    }
}