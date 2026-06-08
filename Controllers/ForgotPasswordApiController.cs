using Microsoft.AspNetCore.Mvc;
using ShriGo.Model;
using ShriGo.Pages.Booking;

namespace ShriGo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgotPasswordApiController : ControllerBase
    {
        private readonly RideDBContext _dbContext;
        private readonly EmailService _emailService;

        public ForgotPasswordApiController(
            RideDBContext context,
            EmailService emailService)
        {
            _dbContext = context;
            _emailService = emailService;
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(
        [FromBody] ForgotPasswordRequest request
        )
        {
            var email = request?.Email;

            if (
                string.IsNullOrWhiteSpace(email)
            )
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Email is required"
                });
            }

            //-----------------------------------
            // Find Driver
            //-----------------------------------

            var user =
                _dbContext.UserTb
                .FirstOrDefault(x =>

                    x.UserEmail ==
                    email
                );

            //-----------------------------------
            // Find Passenger
            //-----------------------------------

            var passenger =
                _dbContext.PassengerTb
                .FirstOrDefault(x =>

                    x.PassengerEmail ==
                    email
                );

            //-----------------------------------
            // Security response
            //-----------------------------------

            if (
                user == null
                &&
                passenger == null
            )
            {
                return Ok(new
                {
                    success = true,
                    message =
                        "If account exists, reset link sent."
                });
            }

            //-----------------------------------
            // Generate Token
            //-----------------------------------

            var token =
                Guid.NewGuid()
                .ToString();

            if (user != null)
            {
                user.PasswordResetToken =
                    token;

                user.ResetTokenExpiry =
                    DateTime.UtcNow
                    .AddMinutes(30);
            }

            if (passenger != null)
            {
                passenger.PasswordResetToken =
                    token;

                passenger.ResetTokenExpiry =
                    DateTime.UtcNow
                    .AddMinutes(30);
            }

            await _dbContext
                .SaveChangesAsync();

            //-----------------------------------
            // Reset Link
            //-----------------------------------

            var resetLink =
              $"{Request.Scheme}://{Request.Host}/Account/ResetPassword?token={token}";

            //-----------------------------------
            // Send Email
            //-----------------------------------

            await _emailService
                .sendEmailAsync(

                    email,

                    "Reset Your ShriGo Password",

                    $@"
                    <h2>Password Reset</h2>

                    <p>Click below to reset your password:</p>

                    <p>
                        <a href='{resetLink}'>
                            Reset Password
                        </a>
                    </p>

                    <p>
                        Link expires in
                        30 minutes.
                    </p>
                    "
                );

            return Ok(new
            {
                success = true,
                message =
                    "Reset link sent to email"
            });
        }
        public class ForgotPasswordRequest
        {
            public string? Email
            {
                get;
                set;
            }
        }
    }
}