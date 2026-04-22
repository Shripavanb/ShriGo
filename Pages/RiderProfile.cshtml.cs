using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;
using System.Collections;
using System.Data;

namespace ShriGo.Pages
{
    public class RiderProfileModel : PageModel
    {
        private readonly RideDBContext _dbContext;
        public List<UserModel> listUserModel = new List<UserModel>();
        public List<UserModel> activeUser = new List<UserModel>();

        public List<SortedRideModel> list_SortedRideModel = new List<SortedRideModel>();
        public List<SortedRideModel> only_DriverRides = new List<SortedRideModel>();

        [BindProperty]
        public SortedRideModel updateRecord { get; set; }

        //Constructor
        public RiderProfileModel(RideDBContext context)
        {
            _dbContext = context;
        }

        public void OnGet()
        {
            string session_UserName = HttpContext.Session.GetString("session_UserName");
            string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");
            string session_UserRole = HttpContext.Session.GetString("session_UserRole");
            //User profile display
            listUserModel = _dbContext.UserTb.ToList();
            foreach(var user in listUserModel)
            {
                int index = listUserModel.FindIndex(a => a.UserFirstName == session_UserName);
                if (user.UserFirstName ==session_UserName)
                {
                    //Major Milestone in achiving only wanted list out of selected index
                    activeUser.Add(listUserModel[index]);
                }
             
            }

            //Driver ride list display   
            list_SortedRideModel = _dbContext.Ride_DBTable.ToList();
            foreach (var ride in list_SortedRideModel)
            {
               
                if (ride.DriverUniqueId ==session_UserUniqueId)
                {
                    only_DriverRides = list_SortedRideModel.FindAll(a => a.DriverUniqueId == session_UserUniqueId);
                    //Major Milestone in achiving only wanted list out of selected index
                    //only_DriverRides.Add(list_SortedRideModel[rideIndex]);
                }

            }

        }

   

        //Allows Drivers to Delete their ride of choice.
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            // 1. Find the record in your database
            var rowToDelete = await _dbContext.Ride_DBTable.FindAsync(id);

            if (rowToDelete != null)
            {
                // 2. Remove the record
                _dbContext.Ride_DBTable.Remove(rowToDelete);

                // 3. Save changes to persist the deletion
                await _dbContext.SaveChangesAsync();
            }

            // 4. Redirect back to the current page to refresh the table
            return RedirectToPage();
        }
    }
}
