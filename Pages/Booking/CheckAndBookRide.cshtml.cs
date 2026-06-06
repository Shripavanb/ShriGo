using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ShriGo.Pages.Booking
{
    public class CheckAndBookRideModel : PageModel
    {

        [BindProperty(SupportsGet = true)]
        public string Pickup { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Drop { get; set; }

        public double DistanceKm { get; set; }

        public decimal EstimatedFare { get; set; }

        public decimal MinEstimatedPricePP { get; set; }

        public decimal MaxEstimatedPricePP { get; set; }

        public void OnGet()
        {
            if (!string.IsNullOrEmpty(Pickup) &&
                 !string.IsNullOrEmpty(Drop))
            {
                Pickup =
                    NormalizeLocation(Pickup);

                Drop =
                    NormalizeLocation(Drop);

                if (string.IsNullOrEmpty(Pickup) ||
                    string.IsNullOrEmpty(Drop))
                {
                    TempData["RideError"] =
                        "Sorry, no ride available";

                    return;
                }
            }
        }
        public void OnPost()
        {
            // TEMP DEMO LOGIC
            // Replace with real API later

            DistanceKm = 12;

            EstimatedFare =
                Convert.ToDecimal(DistanceKm * 8);

            if (EstimatedFare < 50)
            {
                EstimatedFare = 50;
            }
        }

        private string NormalizeLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return "";

            location =
                location.Trim().ToUpper();

            // Hyderabad
            if (location.Contains("HYD") ||
                location.Contains("HYDERABAD"))
                return "HYD";

            // JBS
            if (location.Contains("JBS") ||
                location.Contains("SECUNDERABAD"))
                return "JBS";

            // Airport
            if (location.Contains("AIRPORT") ||
                location.Contains("RGIA"))
                return "AIRPORT";

            // Metpally
            if (location.Contains("MTPL") ||
                location.Contains("METPALLY"))
                return "MTPL";

            // Korutla
            if (location.Contains("KRTL") ||
                location.Contains("KORUTLA"))
                return "KRTL";

            // Ryakal
            if (location.Contains("RYKL") ||
                location.Contains("RYAKAL"))
                return "RYKL";

            // Armoor
            if (location.Contains("ARMR") ||
                location.Contains("ARMOOR"))
                return "ARMR";

            // Nizamabad
            if (location.Contains("NZB") ||
                location.Contains("NIZAMABAD"))
                return "NZB";

            //Jagtial/jagityal 
            if (location.Contains("JGT") ||
                 location.Contains("Jagtial"))
                return "JGT";

            return "";
        }
    }
}