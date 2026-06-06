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
        [BindProperty(SupportsGet = true)]
        public string Pickup
        {
            get;
            set;
        }

        [BindProperty(SupportsGet = true)]
        public string Drop
        {
            get;
            set;
        }

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

            if (
              !string.IsNullOrWhiteSpace(
                  Pickup
              )

              &&

              !string.IsNullOrWhiteSpace(
                  Drop
              )
          )
            {
                Pickup =
                    NormalizeLocation(
                        Pickup
                    );

                Drop =
                    NormalizeLocation(
                        Drop
                    );

                Console.WriteLine(
                    $"Normalized Pickup = {Pickup}"
                );

                Console.WriteLine(
                    $"Normalized Drop = {Drop}"
                );

                foreach (
                    var ride in
                    finalListRideModel
                )
                {
                    Console.WriteLine(
                        $"RideId = {ride.RideId}"
                    );

                    Console.WriteLine(
                        $"Source = {ride.RideSource}"
                    );

                    Console.WriteLine(
                        $"Via = {ride.RideVia}"
                    );

                    Console.WriteLine(
                        $"Destination = {ride.RideDesti}"
                    );

                    bool isMatch =

                        IsRideMatching(

                            ride,

                            Pickup,

                            Drop
                        );

                    Console.WriteLine(
                        $"Match = {isMatch}"
                    );
                }

                finalListRideModel =

                    finalListRideModel
                        .Where(

                            ride =>

                                IsRideMatching(

                                    ride,

                                    Pickup,

                                    Drop
                                )
                        )
                        .ToList();

                Console.WriteLine(
                    $"Final Count = {finalListRideModel.Count}"
                );
            }
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

        private string NormalizeLocation(
    string location
)
        {
            if (
                string.IsNullOrWhiteSpace(
                    location
                )
            )
                return "";

            location =
                location
                    .Trim()
                    .ToUpper();

            if (
                location.Contains(
                    "HYD"
                ) ||
                location.Contains(
                    "HYDERABAD"
                )
            )
                return "HYD";

            if (
                location.Contains(
                    "JBS"
                ) ||
                location.Contains(
                    "SECUNDERABAD"
                )
            )
                return "JBS";

            if (
                location.Contains(
                    "AIRPORT"
                ) ||
                location.Contains(
                    "RGIA"
                )
            )
                return "AIRPORT";

            if (
                location.Contains(
                    "MTPL"
                ) ||
                location.Contains(
                    "METPALLY"
                )
            )
                return "MTPL";

            if (
                location.Contains(
                    "KRTL"
                ) ||
                location.Contains(
                    "KORUTLA"
                )
            )
                return "KRTL";

            if (
                location.Contains(
                    "RYKL"
                ) ||
                location.Contains(
                    "RYAKAL"
                )
            )
                return "RYKL";

            if (
                location.Contains(
                    "ARMR"
                ) ||
                location.Contains(
                    "ARMOOR"
                )
            )
                return "ARMR";

            if (
                location.Contains(
                    "NZB"
                ) ||
                location.Contains(
                    "NIZAMABAD"
                )
            )
                return "NZB";

            if (
                location.Contains(
                    "JGT"
                ) ||
                location.Contains(
                    "JAGTIAL"
                )
            )
                return "JGT";

            return "";
        }
        private bool IsRideMatching(

            SortedRideModel ride,

            string pickup,

            string drop
        )
        {
            //----------------------------------
            // Normalize search
            //----------------------------------

            pickup =
                NormalizeLocation(
                    pickup
                );

            drop =
                NormalizeLocation(
                    drop
                );

            //----------------------------------
            // Source locations
            //----------------------------------

            var sourceLocations =

                string.IsNullOrWhiteSpace(
                    ride.RideSource
                )

                ?

                new List<string>()

                :

                ride.RideSource
                    .Split(',')

                    .Select(

                        x => NormalizeLocation(
                            x.Trim()
                        )
                    )
                    .ToList();

            //----------------------------------
            // Destination locations
            //----------------------------------

            var destinationLocations =

                string.IsNullOrWhiteSpace(
                    ride.RideDesti
                )

                ?

                new List<string>()

                :

                ride.RideDesti
                    .Split(',')

                    .Select(

                        x => NormalizeLocation(
                            x.Trim()
                        )
                    )
                    .ToList();

            //----------------------------------
            // Match
            //----------------------------------

            bool pickupMatch =

                sourceLocations
                    .Contains(
                        pickup
                    );

            bool dropMatch =

                destinationLocations
                    .Contains(
                        drop
                    );

            Console.WriteLine(
                $"RideId={ride.RideId}"
            );

            Console.WriteLine(
                $"Pickup={pickup}"
            );

            Console.WriteLine(
                $"Drop={drop}"
            );

            Console.WriteLine(
                $"Source={string.Join(",", sourceLocations)}"
            );

            Console.WriteLine(
                $"Destination={string.Join(",", destinationLocations)}"
            );

            Console.WriteLine(
                $"Match={pickupMatch && dropMatch}"
            );

            return

                pickupMatch
                &&
                dropMatch;
        }
    }
}