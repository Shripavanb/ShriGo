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
        public UserModel NewDriverModel { get; set; }


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
            //if (NewDriverModel.DriverEmail == listDriverM)
            //{

            //}
            //verify the login credentials and allow user to profile page 
            //Response.Redirect("~/Admin/AdminDashboard.cshtml");
            return RedirectToPage();
        }
    }
}
