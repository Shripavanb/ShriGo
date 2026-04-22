using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriGo.Model;

namespace ShriGo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly RideDBContext _dbContext;

        public List<SortedRideModel> finalListRideModel = new List<SortedRideModel>();

        //New Db table
        public List<SortedRideModel> activeTodaysRideList = new List<SortedRideModel>();
        public List<SortedRideModel> expiredRidesList = new List<SortedRideModel>();

        public List<SortedRideModel> List_SortedRideModel = new List<SortedRideModel>();

        [BindProperty]
        public SortedRideModel sortedRideModel { get; set; }



        public IndexModel(ILogger<IndexModel> logger, RideDBContext context)
        {
            _logger = logger;
            _dbContext = context;
        }

        public void OnGet()
        {
            //Creating a session variable 
            string userValue = HttpContext.Session.GetString("session_UserName");

            //HttpContext.Session.SetString("UserSession", "Active");

            // Define the cutoff date, date only 
            var cutoffDate = DateOnly.FromDateTime(DateTime.Today);
            Console.WriteLine("cutoffDate:"+cutoffDate);

            TimeOnly time = TimeOnly.FromDateTime(DateTime.Now);
    
            string time24 = time.ToString();
            DateTime parsedTime = DateTime.Parse(time24);
            string amPmTime = parsedTime.ToString("hh:mm tt");
            // Define the cutoff date, date only 
            string cutoffTime = amPmTime;
            Console.WriteLine("cutoffTime:"+cutoffTime);
            //--------------------------------------------------

            var todaysdate = DateOnly.FromDateTime(DateTime.Today);

            //foreach (var list in _dbContext.Ride_DBTable)
            //{
                //if(DateTime.Parse(list.RideTime).ToString("HH:mm")time.ToString("HH:mm"))
                //{
                //    sortedRideModel.RideDatet
                ////}
                //List<string> times = new List<string> { "14:30", "09:15", "22:00", "05:45", "13:00" };

                //// Sort ascending using OrderBy
                //var sortedTimes = times.OrderBy(t => DateTime.Parse(t)).ToList();

                //// Iterate through sorted list
                //foreach (string ltime in sortedTimes)
                //{
                //    Console.WriteLine(ltime);
                //}

                //if (list != null)
                //{
                //    sortedListRideModel.Add(list);
                //    if (list.RideDate == cutoffDate)
                //    {
                //        todaysRideList.Add(list);

                //    }
                //}
            //}

            //foreach (var newlist in todaysRideList)
            //{
            //    if (todaysRideList!=null) {

            //        string pmTime = newlist.RideTime;
            //        // Parse to DateTime, then format to 24-hour
            //        string twentyFourHourTime = DateTime.Parse(pmTime).ToString("HH:mm");

            //        Console.WriteLine("twentyFourHourTime:"+twentyFourHourTime);
            //    }
            //}
                ///var sortTodayslist = todaysRideList.OrderBy(e=>e.RideTime.CompareTo(cutoffTime)>0).ToList();
            //--------------------------------
            // Finds the old entities(as per date) to remove, 
            var oldRidesAsPerDate = _dbContext.Ride_DBTable.Where(r => r.RideDate < cutoffDate).ToList();
            Console.WriteLine("oldRidesAsPerDate:"+oldRidesAsPerDate);

            // Finds the old entities(as per time) to segregate List ,
            foreach (var list in _dbContext.Ride_DBTable)
            {
                if(list.RideDate == DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    if (list.RideTime < time)
                    {
                        expiredRidesList.Add(list);
                    }
                    else if(list.RideTime >= time)
                    {
                        List_SortedRideModel.Add(list);
                    }                    
                }
                else 
                {
                    {
                        List_SortedRideModel.Add(list);
                    }
                }
            }

            //var oldRidesAsPerTime = _dbContext.Ride_DBTable.Where(r => r.RideTime.CompareTo(cutoffTime)<0).ToList();
            //Console.WriteLine("oldRidesAsPerTime:"+oldRidesAsPerTime);

            // Make sure every time it Removes the old entities from the DbSet
            _dbContext.Ride_DBTable.RemoveRange(oldRidesAsPerDate);
            //_dbContext.RideDBTable.RemoveRange(oldRidesAsPerTime);

            _dbContext.SaveChanges();
            //string time24 = "14:30";
            //DateTime parsedTime = DateTime.Parse(time24);
            //string amPmTime = parsedTime.ToString("hh:mm tt"); // Results in "02:30 PM"

            //var sortedTimes = _dbContext.RideDBTable.OrderBy(t => t.RideTime).ToList();

            //Finally Arrange list as per date and time 
            //List_SortedRideModel = _dbContext.Ride_DBTable.OrderBy(x => x.RideDate).ThenBy(x => x.RideTime).ToList();
            finalListRideModel =List_SortedRideModel.OrderBy(x => x.RideDate).ThenBy(x => x.RideTime).ToList();
        }
    }
}
