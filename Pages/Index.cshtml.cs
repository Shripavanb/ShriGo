using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriGo.Model;

namespace ShriGo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly RideDBContext _dbContext;

        public List<RideModel> listRideModel = new List<RideModel>();
 
        public IndexModel(ILogger<IndexModel> logger, RideDBContext context)
        {
            _logger = logger;
            _dbContext = context;
        }

        public void OnGet()
        {
            //Creating a session variable 
            string userValue = HttpContext.Session.GetString("UserName");
            //HttpContext.Session.SetString("UserSession", "Active");

            // Define the cutoff date, date only 
            var cutoffDate = DateOnly.FromDateTime(DateTime.Today);

            TimeOnly time = TimeOnly.FromDateTime(DateTime.Now);
            string time24 = time.ToString();
            DateTime parsedTime = DateTime.Parse(time24);
            string amPmTime = parsedTime.ToString("hh:mm tt");
            // Define the cutoff date, date only 
            string cutoffTime = amPmTime;

            // Finds the old entities(as per date) to remove, 
            var oldRidesAsPerDate = _dbContext.RideDBTable.Where(r => r.RideDate < cutoffDate).ToList();
            
            // Finds the old entities(as per date) to remove,
            var oldRidesAsPerTime = _dbContext.RideDBTable.Where(r => r.RideTime.CompareTo(cutoffTime)<0).ToList();


            // Make sure every time it Removes the old entities from the DbSet
            _dbContext.RideDBTable.RemoveRange(oldRidesAsPerDate);
            //_dbContext.RideDBTable.RemoveRange(oldRidesAsPerTime);

            _dbContext.SaveChanges();
            //string time24 = "14:30";
            //DateTime parsedTime = DateTime.Parse(time24);
            //string amPmTime = parsedTime.ToString("hh:mm tt"); // Results in "02:30 PM"


            //var sortedTimes = _dbContext.RideDBTable.OrderBy(t => t.RideTime).ToList();
      
            
            //Finally Arrange list as per date and time 
            listRideModel = _dbContext.RideDBTable.OrderBy(x => x.RideDate).ThenBy(x => x.RideTime).ToList();
        }
    }
}
