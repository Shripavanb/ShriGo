using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;

namespace ShriGo.Pages
{
    public class SignInModel : PageModel
    {
        private readonly RideDBContext _dbContext;


        public List<UserModel> listUserModel = new List<UserModel>();

        [BindProperty]
        public UserModel NewUserModel { get; set; }


        public SignInModel(RideDBContext context)
        {
            _dbContext = context;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            //List<DriverModel> databaseList = _dBContext.DriversTb.ToList();
            listUserModel = _dbContext.UserTb.ToList();

            foreach (var user in listUserModel)
            {
                if(NewUserModel.UserEmail==user.UserEmail && NewUserModel.UserPswd == user.UserPswd)
                {
                    // Clears the session data if it holds any
                    HttpContext.Session.Clear();

                    //Session Start, Creating a session variables 
                    HttpContext.Session.SetString("session_UserName", user.UserFirstName);
                    HttpContext.Session.SetString("session_UserUniqueId", user.UserUniqueId);

                    //Signin Validated
                    return RedirectToPage("/RiderProfile");
                }
                else
                {
                    //Signin Not Validated
                }
            }
            //if (NewDriverModel.DriverEmail == listDriverM)
            //{

            //}
            //verify the login credentials and allow user to profile page 
            //Response.Redirect("~/Admin/AdminDashboard.cshtml");
            return RedirectToPage();
        }
    }
}
