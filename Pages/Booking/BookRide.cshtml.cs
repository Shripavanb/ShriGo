using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;

namespace ShriGo.Pages.Booking
{
    public class BookRideModel : PageModel
    {
        private readonly RideDBContext _dbContext;

        public List<SortedRideModel> list_selectedRideModel = new List<SortedRideModel>();

        [BindProperty]
        public string lridetime { get; set; }

        [BindProperty]
        public SortedRideModel selectedRideModel{  get; set; }

        [BindProperty]
        public BookedRideModel bookedRideModel { get; set; }

        [BindProperty]
        public List<int> SelectedIds { get; set; } // This will hold the selected values

        [BindProperty]
        public int ItemQuantity { get; set; } = 1;

        //constructor
        public BookRideModel(RideDBContext context)
            {
            _dbContext=context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var rideSelected = await _dbContext.Ride_DBTable.FirstOrDefaultAsync(e => e.RideId==id);

            if (rideSelected.RideTime!=null)
            {
                string time24 = rideSelected.RideTime.ToString();
                DateTime parsedTime = DateTime.Parse(time24);
                string amPmTime = parsedTime.ToString("hh:mm tt");
                lridetime = amPmTime;
            }
            selectedRideModel=rideSelected;
            list_selectedRideModel.Add(selectedRideModel);
            return Page();
        }


        public async Task<IActionResult> OnPost(int id)
        {
            var rideSelected = await _dbContext.Ride_DBTable.FirstOrDefaultAsync(e => e.RideId==id);

            string session_userName = HttpContext.Session.GetString("session_UserName");
            string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");
            string session_UserContact = HttpContext.Session.GetString("session_UserContact");
            string session_UserEmail = HttpContext.Session.GetString("session_UserEmail");

            if (session_userName == "Guest"||session_userName ==null)
            {                
                Response.Redirect("/SignUp");
            }
            else
            {
                // Access the selected value via ItemQuantity
                var result = ItemQuantity;

                bookedRideModel.RideId =rideSelected.RideId;
                bookedRideModel.RideDate =rideSelected.RideDate;
                bookedRideModel.RideSource =rideSelected.RideSource;
                bookedRideModel.RideDesti =rideSelected.RideDesti;
                bookedRideModel.RideVia =rideSelected.RideVia;
                bookedRideModel.RideTime=rideSelected.RideTime;
                //Booked Seats
                bookedRideModel.BookedSeats = ItemQuantity.ToString();

                int totalbookingamount=int.Parse(rideSelected.RidePrice);
                bookedRideModel.RidePrice =(totalbookingamount*ItemQuantity).ToString();
                bookedRideModel.DriverContact =rideSelected.DriverContact;
                bookedRideModel.DriverUniqueId =rideSelected.DriverUniqueId;
                bookedRideModel.DriverFirstName =rideSelected.DriverFirstName;

                bookedRideModel.UserFirstName =session_userName;
                bookedRideModel.UserUniqueId =session_UserUniqueId;
                bookedRideModel.UserContact =session_UserContact;
                bookedRideModel.UserEmail = session_UserContact;
            }
            //store booked ride into db further use
            _dbContext.BookedRide_DBTable.Add(bookedRideModel);

            _dbContext.SaveChanges();
            return RedirectToPage("/Passengers/PassengerProfile");

        }
    }
}
