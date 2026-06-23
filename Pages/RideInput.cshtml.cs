using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriGo.Helpers;
using ShriGo.Model;
using System.Collections;
using System.ComponentModel.DataAnnotations;


namespace ShriGo.Pages
{
    public class RiderInputModel : PageModel
    {
        private readonly RideDBContext _dbContext;

        [BindProperty]
        public int newRideId { get ; set ;}

        //public List<RideModel> listRideModel = new List<RideModel>();
        public List<SortedRideModel> list_SortedRideModel = new List<SortedRideModel>();
        private DateTime lastStartDate;

        [BindProperty]
        public SortedRideModel NewRideModel { get; set; }


        //Constructor 
        public RiderInputModel(RideDBContext context)
        {
            _dbContext = context;
        }

        
        public void OnGet()
        {
            string session_userName = HttpContext.Session.GetString("session_UserName");
            string session_DriverUniqueId = HttpContext.Session.GetString("session_DriverUniqueId");
        }

        public IActionResult OnPost()
        {
            var now = TimeHelper.GetIndiaTime();
            string session_userName = HttpContext.Session.GetString("session_UserName");
            string session_DriverUniqueId = HttpContext.Session.GetString("session_DriverUniqueId");

            //Add user session UniqueId into Ride table to know whats he added 
            NewRideModel.DriverUniqueId = session_DriverUniqueId;
            
            // Define the cutoff date, date only 
            var cutoffDate = TimeHelper.GetIndiaDate(); 

            TimeOnly time = TimeOnly.FromDateTime(now);
            string time24 = time.ToString();
            DateTime parsedTime = DateTime.Parse(time24);
            string amPmTime = parsedTime.ToString("hh:mm tt");
            // Define the cutoff date, date only 
            string cutoffTime = amPmTime;

            // Finds the old entities(as per date) to remove, 
            var oldRidesAsPerDate = _dbContext.Ride_DBTable.Where(r => r.RideDate < cutoffDate).ToList();
            //// Finds the old entities(as per date) to remove,
            //var oldRidesAsPerTime = _dbContext.Ride_DBTable.Where(r => r.RideTime.CompareTo(cutoffTime)<0).ToList();
            
            // Returns true if NO data exists in the table
            bool isTableEmpty = !_dbContext.Ride_DBTable.Any();

            if (isTableEmpty)
            {
                NewRideModel.RideId = 1;
                Console.WriteLine("Table is empty.");
            }
            else
            {
                // Finds the max Id number and adds +1 to it 
                var newRideId = _dbContext.Ride_DBTable.Max(r => r.RideId);
                NewRideModel.RideId = newRideId+1;
                Console.WriteLine("Table has data.");            
            }
      

            // Remove the entities from the DbSet
            _dbContext.Ride_DBTable.RemoveRange(oldRidesAsPerDate);
            //_dbContext.RideDBTable.RemoveRange(oldRidesAsPerTime);
    
            NewRideModel.DriverUniqueId =session_DriverUniqueId;
            NewRideModel.DriverFirstName =session_userName;

            _dbContext.Ride_DBTable.Add(NewRideModel);

            _dbContext.SaveChanges();

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
