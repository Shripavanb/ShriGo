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
            // FIND USER
            // ======================================================

            var user = _dbContext.UserTb
                .FirstOrDefault(x =>
                    x.PasswordResetToken == Token &&
                    x.ResetTokenExpiry > DateTime.Now);

            if (user == null)
            {
                TempData["Message"] =
                    "Invalid or expired reset link.";

                return RedirectToPage("/SignIn");
            }

            // ======================================================
            // HASH PASSWORD
            // ======================================================

            var passwordHelper =
                new PasswordHelper();

            user.UserPswd =
                passwordHelper.HashPassword(
                    NewPassword);

            // ======================================================
            // CLEAR RESET TOKEN
            // ======================================================

            user.PasswordResetToken = null;

            user.ResetTokenExpiry = null;

            await _dbContext.SaveChangesAsync();

            // ======================================================
            // SUCCESS
            // ======================================================

            TempData["Message"] =
                "Password reset successful.";

            return RedirectToPage("/SignIn");
        }
    }
}