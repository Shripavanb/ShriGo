using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShriGo.Model;
using System.ComponentModel.DataAnnotations;

namespace ShriGo.Pages.Drivers
{
    public class EditModel : PageModel
    {
        private readonly RideDBContext _dbContext;

        [BindProperty]
        public string lridetime { get; set; }

        private DateTime lastStartDate;

        public EditModel(RideDBContext context)
        {
            _dbContext=context;

        }

        [BindProperty]
        public SortedRideModel Rides {  get; set; }

        //public void OnGet()
        //{

        //}

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == 0 || _dbContext.Ride_DBTable==null)
            {
                return NotFound();
            }

            var rideToEdit = await _dbContext.Ride_DBTable.FirstOrDefaultAsync(e=>e.RideId==id);
            if (rideToEdit == null)
            {
                return NotFound();
            }
            if(rideToEdit.RideTime!=null){

                string time24 = rideToEdit.RideTime.ToString();
                DateTime parsedTime = DateTime.Parse(time24);
                string amPmTime = parsedTime.ToString("hh:mm tt");
                lridetime = amPmTime;
            }
 
      
            Rides= rideToEdit;

            return Page();
        }

        //Allows Drivers to Edit their ride of choice.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            string session_UserName = HttpContext.Session.GetString("session_UserName");
            string session_UserUniqueId = HttpContext.Session.GetString("session_UserUniqueId");
            Rides.RideId = id;
            Rides.DriverUniqueId =session_UserUniqueId;
            Rides.DriverFirstName =session_UserName;
            if (Rides != null)
            {
                // 2. Update the record in table 
                _dbContext.Ride_DBTable.Update(Rides);

                // 3. Save changes to persist the update
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }

            // 4. Redirect back to the current page to refresh the table
            return RedirectToPage("/RiderProfile");
        }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime LastStartDate
        {
            get { return lastStartDate; }
            set { lastStartDate = value; }
        }
    }
}
