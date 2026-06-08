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
        public async Task<IActionResult> ForgotPassword(
            [FromBody] ForgotPasswordRequest request)
        {
            try
            {
                var user = _dbContext.UserTb
                    .FirstOrDefault(x =>
                        x.UserEmail == request.Email);

                var passenger = _dbContext.PassengerTb
                    .FirstOrDefault(x =>
                        x.PassengerEmail == request.Email);

                if (user == null &&
                    passenger == null)
                {
                    return Ok(new
                    {
                        success = true,
                        message =
                        "If account exists, reset link sent."
                    });
                }

                var token =
                    Guid.NewGuid().ToString();

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

                var resetLink =
                    $"{Request.Scheme}://{Request.Host}/Account/ResetPassword?token={token}";

                await _emailService.sendEmailAsync(
                    request.Email,
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
                        Link expires in 30 minutes.
                    </p>");

                return Ok(new
                {
                    success = true,
                    message =
                    "Reset link sent to email"
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
    }
}