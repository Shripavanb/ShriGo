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
        public DriverModel NewDriverModel { get; set; }

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
            var driver = _dbContext.DriversTb
                .FirstOrDefault(x =>
                    x.DriverEmail == InputEmail
                    || x.DriverContact == InputEmail);

            if (driver != null)
            {
                bool loginSuccess = false;

                // HASHED PASSWORD
                if (passwordHelper.IsHashed(driver.DriverPswd))
                {
                    loginSuccess =
                        passwordHelper.VerifyPassword(
                            driver.DriverPswd,
                            InputPswd);
                }
                else
                {
                    // OLD PLAIN TEXT PASSWORD

                    if (driver.DriverPswd == InputPswd)
                    {
                        loginSuccess = true;

                        // AUTO CONVERT TO HASH

                        driver.DriverPswd =
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
                        driver.DriverFirstName);

                    HttpContext.Session.SetString(
                        "session_DriverUniqueId",
                        driver.DriverUniqueId);

                    HttpContext.Session.SetString(
                        "session_DriverContact",
                        driver.DriverContact);

                    HttpContext.Session.SetString(
                        "session_DriverEmail",
                        driver.DriverEmail);

                    HttpContext.Session.SetString(
                        "session_DriverRole",
                        driver.DriverRole);

                    if (driver.DriverRole == "Driver")
                    {
                        return RedirectToPage(
                            "/RiderProfile");
                    }

                    if (driver.DriverRole == "Admin")
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
                        "session_DriverUniqueId",
                        passenger.PassengerUniqueId);

                    HttpContext.Session.SetString(
                        "session_DriverContact",
                        passenger.PassengerContact);

                    HttpContext.Session.SetString(
                        "session_DriverEmail",
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