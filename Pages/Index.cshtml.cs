using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShriGo.Helpers;
using ShriGo.Model;

namespace ShriGo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly RideDBContext _dbContext;

        public List<SortedRideModel> finalListRideModel =
            new List<SortedRideModel>();

        public List<SortedRideModel> activeTodaysRideList =
            new List<SortedRideModel>();

        public List<SortedRideModel> expiredRidesList =
            new List<SortedRideModel>();

        public List<SortedRideModel> List_SortedRideModel =
            new List<SortedRideModel>();

        [BindProperty]
        public SortedRideModel sortedRideModel
        {
            get;
            set;
        }

        public IndexModel(
            ILogger<IndexModel> logger,
            RideDBContext context
        )
        {
            _logger = logger;
            _dbContext = context;
        }

        public void OnGet()
        {
            //----------------------------------
            // India Time
            //----------------------------------
            var now =
                TimeHelper.GetIndiaTime();

            var todaysdate =
                TimeHelper.GetIndiaDate();

            //----------------------------------
            // Session
            //----------------------------------
            HttpContext.Session.SetString(
                "session_Guest",
                "Guest_session"
            );

            //----------------------------------
            // Remove old rides (past date)
            //----------------------------------
            var cutoffDate =
                todaysdate;

            Console.WriteLine(
                "cutoffDate: " +
                cutoffDate
            );

            var oldRidesAsPerDate =
                _dbContext
                    .Ride_DBTable
                    .Where(
                        r =>
                            r.RideDate
                            < cutoffDate
                    )
                    .ToList();

            Console.WriteLine(
                "oldRidesAsPerDate: " +
                oldRidesAsPerDate.Count
            );

            //----------------------------------
            // Separate active vs expired rides
            //----------------------------------
            foreach (
                var list in
                _dbContext.Ride_DBTable
            )
            {
                // Today's rides
                if (
                    list.RideDate ==
                    todaysdate
                )
                {
                    if (
                        list.RideTime !=
                        null
                    )
                    {
                        var rideDateTime =

                            list.RideDate
                                .Value
                                .ToDateTime(
                                    list
                                        .RideTime
                                        .Value
                                );

                        // Ride expires
                        // after 2 hours
                        var rideExpiryTime =

                            rideDateTime
                                .AddHours(
                                    2
                                );

                        // Expired ride
                        if (
                            rideExpiryTime
                            < now
                        )
                        {
                            expiredRidesList
                                .Add(
                                    list
                                );
                        }

                        // Active ride
                        else
                        {
                            List_SortedRideModel
                                .Add(
                                    list
                                );
                        }
                    }
                }

                // Future rides
                else
                {
                    List_SortedRideModel
                        .Add(
                            list
                        );
                }
            }

            //----------------------------------
            // Remove expired old date rides
            //----------------------------------
            _dbContext
                .Ride_DBTable
                .RemoveRange(
                    oldRidesAsPerDate
                );

            _dbContext
                .SaveChanges();

            //----------------------------------
            // Final sorting
            //----------------------------------
            finalListRideModel =

                List_SortedRideModel
                    .OrderBy(
                        x => x.RideDate
                    )
                    .ThenBy(
                        x => x.RideTime
                    )
                    .ToList();
        }

        public string GetWhatsAppShareText(
            SortedRideModel item
        )
        {
            //----------------------------------
            // Safe time handling
            //----------------------------------
            string time24 =

                item.RideTime?
                    .ToString()

                ?? "00:00";

            DateTime parsedTime =
                DateTime.Parse(
                    time24
                );

            string amPmTime =

                parsedTime
                    .ToString(
                        "hh:mm tt"
                    );

            string add2hourstoamPmTime =

                parsedTime
                    .AddHours(2)
                    .ToString(
                        "hh:mm tt"
                    );

            //----------------------------------
            // Today / Tomorrow
            //----------------------------------
            string date = "";

            var today =
                TimeHelper
                    .GetIndiaDate();

            if (
                item.RideDate ==
                today
            )
            {
                date = "Today";
            }

            else if (
                item.RideDate ==
                today.AddDays(1)
            )
            {
                date = "Tomorrow";
            }

            else
            {
                date =
                    item.RideDate
                        .ToString();
            }

            //----------------------------------
            // WhatsApp Share Text
            //----------------------------------
            string message =

                "🚗 *Ride Available on ShriGo.in!* \n" +
                "┌─────────────┐\n" +
                "│ 📅 Date    : " +
                date +
                " (" +
                item.RideDate +
                ")\n" +

                "│ 📍 From     : " +
                item.RideSource +
                "\n" +

                "│ 📍 To       : " +
                item.RideDesti +
                "\n" +

                "│ ⏰ Time    : " +
                amPmTime +
                "-" +
                add2hourstoamPmTime +
                "\n" +

                "│ 💺 Seats    : " +
                item.RideSeats +
                "\n" +

                "│ 💰 Price    : ₹" +
                item.RidePrice +
                "/p\n" +

                "│ 👤 Driver   : " +
                item.DriverFirstName +
                "\n" +

                "│ 📞 Contact  : " +
                item.DriverContact +
                "\n" +

                "└─────────────┘\n" +

                "⚡ Book your seat now before it fills!\n" +
                "🌐 Book now on https://shrigo.in";

            return Uri.EscapeDataString(
                message
            );
        }
    }
}
