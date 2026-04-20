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

        private readonly RideDBContext _dbContext;
        //Constructor 
        public AdminDashboardModel(RideDBContext context)
        {
            _dbContext = context;
        }

        public void OnGet()
        {
            //Arrange list as per date and time 
            listRideModel = _dbContext.Ride_DBTable.OrderBy(x => x.RideDate).ThenBy(x => x.RideTime).ToList();
            //listRideModel = _dbContext.RideDBTable.ToList();

            //User List display table 
            listUserModel = _dbContext.UserTb.ToList();
        }
    }
}
