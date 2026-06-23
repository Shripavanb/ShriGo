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
        public List<DriverModel> listDriverModel = new List<DriverModel>();
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
            string session_DriverUniqueId = HttpContext.Session.GetString("session_DriverUniqueId");


            if (session_UserName =="ShriPavan" && session_DriverUniqueId =="64782")
            {
                //User List display table 
                listDriverModel = _dbContext.DriversTb.ToList();

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
