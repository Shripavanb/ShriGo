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
            var user = _dbContext.UserTb
                .FirstOrDefault(x => x.UserEmail == Email);

            if (user == null)
            {
                TempData["Message"] =
                    "If account exists, reset link sent.";

                return RedirectToPage("/SignIn");
            }

            // Generate secure token
            var token = Guid.NewGuid().ToString();

            user.PasswordResetToken = token;

            user.ResetTokenExpiry =
                DateTime.Now.AddMinutes(30);

            await _dbContext.SaveChangesAsync();

            var resetLink =
                $"{Request.Scheme}://{Request.Host}/Account/ResetPassword?token={token}";

            // TODO:
            // Send email here
            await _emailService.sendEmailAsync(
                                Email,
                                "Reset Your ShriGo Password",
                                $@"
                    <h2>Password Reset</h2>

                    <p>
                        Click below to reset your password:
                    </p>

                    <p>
                        <a href='{resetLink}'>
                            Reset Password
                        </a>
                    </p>

                    <p>
                        Link expires in 30 minutes.
                    </p>
                    ");

            TempData["Message"] =
                "Reset link generated.";

            return RedirectToPage("/SignIn");
        }
    }
}