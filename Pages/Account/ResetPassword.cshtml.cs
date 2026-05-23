using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriGo.Model;

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

        public void OnGet(string token)
        {
            Token = token;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = _dbContext.UserTb
                .FirstOrDefault(x =>
                    x.PasswordResetToken == Token &&
                    x.ResetTokenExpiry > DateTime.Now);

            if (user == null)
            {
                TempData["Message"] =
                    "Invalid or expired token.";

                return RedirectToPage("/SignIn");
            }

            // TEMPORARY
            // Later replace with password hashing
            user.UserPswd = NewPassword;

            user.PasswordResetToken = null;
            user.ResetTokenExpiry = null;

            await _dbContext.SaveChangesAsync();

            TempData["Message"] =
                "Password reset successful.";

            return RedirectToPage("/SignIn");
        }
    }
}