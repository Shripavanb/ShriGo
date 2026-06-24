using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriGo.Model;
using ShriGo.Pages.Booking;

namespace ShriGo.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly RideDBContext _dbContext;
        private readonly EmailService _emailService;

        public ForgotPasswordModel(
    RideDBContext context,
    EmailService emailService)
        {
            _dbContext = context;
            _emailService = emailService;
        }

        [BindProperty]
        public string Email { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // ======================================================
            // FIND USER OR PASSENGER
            // ======================================================

            var user = _dbContext.DriversTb
                .FirstOrDefault(x => x.DriverEmail == Email);

            var passenger = _dbContext.PassengerTb
                .FirstOrDefault(x => x.PassengerEmail == Email);

            // ======================================================
            // IF NOT FOUND (DO NOT REVEAL INFO)
            // ======================================================

            if (user == null && passenger == null)
            {
                TempData["Message"] =
                    "If account exists, reset link sent.";

                return RedirectToPage("/SignIn");
            }

            // ======================================================
            // GENERATE TOKEN
            // ======================================================

            var token = Guid.NewGuid().ToString();

            // ======================================================
            // USER RESET SETUP
            // ======================================================

            if (user != null)
            {
                user.PasswordResetToken = token;
                user.ResetTokenExpiry = DateTime.UtcNow.AddMinutes(30);
            }

            // ======================================================
            // PASSENGER RESET SETUP
            // ======================================================

            if (passenger != null)
            {
                passenger.PasswordResetToken = token;
                passenger.ResetTokenExpiry = DateTime.UtcNow.AddMinutes(30);
            }

            await _dbContext.SaveChangesAsync();

            // ======================================================
            // RESET LINK
            // ======================================================

            var resetLink =
                $"{Request.Scheme}://{Request.Host}/Account/ResetPassword?token={token}";

            // ======================================================
            // SEND EMAIL
            // ======================================================

            await _emailService.sendEmailAsync(
                Email,
                "Reset Your ShriGo Password",
                $@"
        <h2>Password Reset</h2>

        <p>Click below to reset your password:</p>

        <p>
            <a href='{resetLink}'>Reset Password</a>
        </p>

        <p>Link expires in 30 minutes.</p>
        ");

            TempData["Message"] =
                "Reset link generated.";

            return RedirectToPage("/SignIn");
        }
    }
}