using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriGo.Model;
using ShriGo.Pages.Helpers;

namespace ShriGo.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly RideDBContext _dbContext;

        public ResetPasswordModel(RideDBContext context)
        {
            _dbContext = context;
        }

        [BindProperty]
        public string Token { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public void OnGet(string token)
        {
            Token = token;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // ======================================================
            // VALIDATIONS
            // ======================================================

            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                ViewData["Message"] =
                    "Password is required.";

                return Page();
            }

            if (NewPassword.Length < 8)
            {
                ViewData["Message"] =
                    "Password must be minimum 8 characters.";

                return Page();
            }

            if (NewPassword != ConfirmPassword)
            {
                ViewData["Message"] =
                    "Passwords do not match.";

                return Page();
            }
            // ======================================================
            // FIND USER OR PASSENGER
            // ======================================================

            var user = _dbContext.UserTb
                .FirstOrDefault(x =>
                    x.PasswordResetToken == Token &&
                    x.ResetTokenExpiry > DateTime.UtcNow);

            var passenger = _dbContext.PassengerTb
                .FirstOrDefault(x =>
                    x.PasswordResetToken == Token &&
                    x.ResetTokenExpiry > DateTime.UtcNow);

            // ======================================================
            // HASH PASSWORD
            // ======================================================

            var passwordHelper = new PasswordHelper();

            // ======================================================
            // UPDATE USER
            // ======================================================

            if (user != null)
            {
                user.UserPswd = passwordHelper.HashPassword(NewPassword);

                user.PasswordResetToken = null;
                user.ResetTokenExpiry = null;

                await _dbContext.SaveChangesAsync();

                TempData["Message"] = "Password reset successful.";

                return RedirectToPage("/SignIn");
            }

            // ======================================================
            // UPDATE PASSENGER
            // ======================================================

            if (passenger != null)
            {
                passenger.PassengerPswd = passwordHelper.HashPassword(NewPassword);

                passenger.PasswordResetToken = null;
                passenger.ResetTokenExpiry = null;

                await _dbContext.SaveChangesAsync();

                TempData["Message"] = "Password reset successful.";

                return RedirectToPage("/PassengerSignIn");
            }

            // ======================================================
            // INVALID TOKEN
            // ======================================================

            TempData["Message"] = "Invalid or expired reset link.";

            return RedirectToPage("/SignIn");
        }
    }
}