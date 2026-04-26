using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;

namespace ShriGo.Pages.Admin
{
    public class AdminDashboardModel : PageModel
    {
        public List<SortedRideModel> listRideModel = new List<SortedRideModel>();
        public List<UserModel> listUserModel = new List<UserModel>();
        public List<BookingsModel> listBookingsModel = new List<BookingsModel>();

        private readonly RideDBContext _dbContext;
        //Constructor 
        public AdminDashboardModel(RideDBContext context)
        {
            _dbContext = context;
        }

        public void OnGet()
        {
            string session_UserName = HttpContext.Session.GetString("session_UserName");
            string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");


            if (session_UserName =="ShriPavan" && session_UserUniqueId =="64782")
            {
                //User List display table 
                listUserModel = _dbContext.UserTb.ToList();

                //Arrange list as per date and time 
                listRideModel = _dbContext.Ride_DBTable.OrderBy(x => x.RideDate).ThenBy(x => x.RideTime).ToList();
                //listRideModel = _dbContext.RideDBTable.ToList();
                //return RedirectToPage("/Admin/AdminDashboard");

                //Bookings Table 
                listBookingsModel = _dbContext.Bookings_DBTable.ToList();
            }
            else
            {
               RedirectToPage("/SignIn");
            }



        }
    }
}
