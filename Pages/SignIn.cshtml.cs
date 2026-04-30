using Google.Api;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;

namespace ShriGo.Pages
{
    public class SignInModel : PageModel
    {
        private readonly RideDBContext _dbContext;

        private RedirectToPageResult x;

        public List<UserModel> listUserModel = new List<UserModel>();
        [BindProperty]
        public UserModel NewUserModel { get; set; }

        public List<PassengerModel> listPassengerModel = new List<PassengerModel>();
        [BindProperty]
        public PassengerModel NewPassengerModel { get; set; }


        public SignInModel(RideDBContext context)
        {
            _dbContext = context;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost(string InputEmail, string InputPswd)
        {
            //List<DriverModel> databaseList = _dBContext.DriversTb.ToList();
            listUserModel = _dbContext.UserTb.ToList();
            listPassengerModel = _dbContext.PassengerTb.ToList();


            foreach (var user in listUserModel)
            {
                if (InputEmail==user.UserEmail && InputPswd == user.UserPswd)
                {
                    // Clears the session data if it holds any
                    HttpContext.Session.Clear();

                    //Session Start, Creating a session variables 
                    HttpContext.Session.SetString("session_UserName", user.UserFirstName);
                    HttpContext.Session.SetString("session_UserUniqueId", user.UserUniqueId);
                    HttpContext.Session.SetString("session_UserContact", user.UserContact);
                    HttpContext.Session.SetString("session_UserEmail", user.UserEmail);
                    HttpContext.Session.SetString("session_UserRole", user.UserRole);

                    if (user.UserRole=="Driver")
                    {
                        //Signin Validated
                        x= RedirectToPage("/RiderProfile");
                    }
                    else if (user.UserRole=="Admin")
                    {
                        x= RedirectToPage("/Admin/AdminDashboard");
                    }
                    return x;
                }

                //if (NewDriverModel.DriverEmail == listDriverM)
                //{

                //}
                //verify the login credentials and allow user to profile page 
                //Response.Redirect("~/Admin/AdminDashboard.cshtml");

            }
            foreach (var user in listPassengerModel)
            {
                if (InputEmail==user.PassengerEmail && InputPswd == user.PassengerPswd)
                {
                    // Clears the session data if it holds any
                    HttpContext.Session.Clear();

                    //Session Start, Creating a session variables 
                    HttpContext.Session.SetString("session_UserName", user.PassengerFirstName);
                    HttpContext.Session.SetString("session_UserUniqueId", user.PassengerUniqueId);
                    HttpContext.Session.SetString("session_UserContact", user.PassengerContact);
                    HttpContext.Session.SetString("session_UserEmail", user.PassengerEmail);

                    return RedirectToPage("/Passengers/PassengerProfile");
                }
            }
            return RedirectToPage();
        }

        
        //----------Future use code ------------------------------------------

        //    // User navigation property
        //     public User User { get; set; }
        //public async Task<bool> VerifyUserAsync(string email, string passwordHash)
        //{
        //    // Joining Users and Profiles to verify credentials [4, 6]
        //    var userExists = await _dbContext.UserTb
        //        .Join(_dbContext.PassengerTb,
        //            user => user.UserId,
        //            profile => profile.UserId,
        //            (user, profile) => new { user, profile })
        //        .AnyAsync(joined =>
        //            joined.user.Email == email &&
        //            joined.user.PasswordHash == passwordHash &&
        //            joined.user.IsActive);

        //    return userExists;
        //}
         
    }
}
