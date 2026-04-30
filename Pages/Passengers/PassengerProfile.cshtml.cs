using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriGo.Model;

namespace ShriGo.Pages.Passengers
{
    public class PassengerProfileModel : PageModel
    {
        private readonly RideDBContext _dbContext;

        //Passenger
        public List<PassengerModel> listPassengerModel = new List<PassengerModel>();
        public List<PassengerModel> activePassenger = new List<PassengerModel>();

        //Bookings 
        public List<BookingsModel> list_BookingsTableModel = new List<BookingsModel>();
        public List<BookingsModel> only_PassengerBookings = new List<BookingsModel>();

        [BindProperty]
        public SortedRideModel updateRecord { get; set; }

        //Constructor
        public PassengerProfileModel(RideDBContext context)
        {
            _dbContext = context;
        }

        public void OnGet()
        {
            string session_UserName = HttpContext.Session.GetString("session_UserName");
            string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");


            //Passsenger profile display
            listPassengerModel = _dbContext.PassengerTb.ToList();
            foreach (var user in listPassengerModel)
            {
                int index = listPassengerModel.FindIndex(a => a.PassengerFirstName == session_UserName);
                if (user.PassengerFirstName ==session_UserName)
                {
                    //Major Milestone in achiving only wanted list out of selected index
                    activePassenger.Add(listPassengerModel[index]);
                }
            }

            //Passenger Booked ride list display   
            list_BookingsTableModel = _dbContext.Bookings_DBTable.ToList();
            foreach (var ride in list_BookingsTableModel)
            {

                if (ride.PassengerUniqueId ==session_UserUniqueId)
                {
                    only_PassengerBookings = list_BookingsTableModel.FindAll(a => a.PassengerUniqueId == session_UserUniqueId);
                    //Major Milestone in achiving only wanted list out of selected index
                    //only_DriverRides.Add(list_SortedRideModel[rideIndex]);
                }
            }
        }

        //Allows Passengers to Delete their Booking  of choice before 2 hours on start of ride.
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            // 1. Find the record in your database
            var rowToDelete = await _dbContext.Bookings_DBTable.FindAsync(id);

            if (rowToDelete != null)
            {
                // 2. Remove the record
                _dbContext.Bookings_DBTable.Remove(rowToDelete);

                // 3. Save changes to persist the deletion
                await _dbContext.SaveChangesAsync();
            }

            // 4. Redirect back to the current page to refresh the table
            return RedirectToPage();
        }

        public IActionResult OnPostBookRide()
        {
            return RedirectToPage("/Index");
        }
    }
}
