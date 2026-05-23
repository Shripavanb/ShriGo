using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriGo.Model;
using ShriGo.Pages.Helpers;

namespace ShriGo.Pages
{
    public class SignInModel : PageModel
    {
        private readonly RideDBContext _dbContext;

        public SignInModel(RideDBContext context)
        {
            _dbContext = context;
        }

        [BindProperty]
        public UserModel NewUserModel { get; set; }

        [BindProperty]
        public PassengerModel NewPassengerModel { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost(
            string InputEmail,
            string InputPswd)
        {
            var passwordHelper = new PasswordHelper();

            // ======================================================
            // DRIVER / ADMIN LOGIN
            // ======================================================

            var user = _dbContext.UserTb
                .FirstOrDefault(x =>
                    x.UserEmail == InputEmail);

            if (user != null)
            {
                bool loginSuccess = false;

                // HASHED PASSWORD
                if (passwordHelper.IsHashed(user.UserPswd))
                {
                    loginSuccess =
                        passwordHelper.VerifyPassword(
                            user.UserPswd,
                            InputPswd);
                }
                else
                {
                    // OLD PLAIN TEXT PASSWORD

                    if (user.UserPswd == InputPswd)
                    {
                        loginSuccess = true;

                        // AUTO CONVERT TO HASH

                        user.UserPswd =
                            passwordHelper.HashPassword(
                                InputPswd);

                        _dbContext.SaveChanges();
                    }
                }

                if (loginSuccess)
                {
                    HttpContext.Session.Clear();

                    HttpContext.Session.SetString(
                        "session_UserName",
                        user.UserFirstName);

                    HttpContext.Session.SetString(
                        "session_UserUniqueId",
                        user.UserUniqueId);

                    HttpContext.Session.SetString(
                        "session_UserContact",
                        user.UserContact);

                    HttpContext.Session.SetString(
                        "session_UserEmail",
                        user.UserEmail);

                    HttpContext.Session.SetString(
                        "session_UserRole",
                        user.UserRole);

                    if (user.UserRole == "Driver")
                    {
                        return RedirectToPage(
                            "/RiderProfile");
                    }

                    if (user.UserRole == "Admin")
                    {
                        return RedirectToPage(
                            "/Admin/AdminDashboard");
                    }
                }

                ViewData["Message"] =
                    "Enter the correct password";

                return Page();
            }

            // ======================================================
            // PASSENGER LOGIN
            // ======================================================

            var passenger = _dbContext.PassengerTb
                .FirstOrDefault(x =>
                    x.PassengerEmail == InputEmail);

            if (passenger != null)
            {
                bool loginSuccess = false;

                // HASHED PASSWORD
                if (passwordHelper.IsHashed(
                    passenger.PassengerPswd))
                {
                    loginSuccess =
                        passwordHelper.VerifyPassword(
                            passenger.PassengerPswd,
                            InputPswd);
                }
                else
                {
                    // OLD PLAIN TEXT PASSWORD

                    if (passenger.PassengerPswd ==
                        InputPswd)
                    {
                        loginSuccess = true;

                        // AUTO CONVERT TO HASH

                        passenger.PassengerPswd =
                            passwordHelper.HashPassword(
                                InputPswd);

                        _dbContext.SaveChanges();
                    }
                }

                if (loginSuccess)
                {
                    HttpContext.Session.Clear();

                    HttpContext.Session.SetString(
                        "session_UserName",
                        passenger.PassengerFirstName);

                    HttpContext.Session.SetString(
                        "session_UserUniqueId",
                        passenger.PassengerUniqueId);

                    HttpContext.Session.SetString(
                        "session_UserContact",
                        passenger.PassengerContact);

                    HttpContext.Session.SetString(
                        "session_UserEmail",
                        passenger.PassengerEmail);

                    HttpContext.Session.SetString(
                        "session_PassengerRole",
                        passenger.Role);

                    return RedirectToPage(
                        "/Passengers/PassengerProfile");
                }

                ViewData["Message"] =
                    "Enter the correct password";

                return Page();
            }

            // ======================================================
            // EMAIL NOT FOUND
            // ======================================================

            ViewData["Message"] =
                "Email ID doesn't exist. Please Sign Up.";

            return Page();
        }
    }
}