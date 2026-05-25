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
                string[] local = {"RYKL", "KRTL", "MTPL","RYKL,KRTL,MTPL" };
                string[] city = { "JBS", "HYD", "JBS,HYD"};
                bool haslocalpickup = local.Any(any=> any == Pickup);
                bool haslocaldrop = local.Any(any=>any == Drop);
                bool hascitypickup = city.Any(any => any == Pickup);
                bool hascitydrop = city.Any(any => any == Drop);


                if ((haslocalpickup && hascitydrop)||
                                   (hascitypickup  && haslocaldrop))
                {
                    DistanceKm = 214;

                    EstimatedFare =
                        Convert.ToDecimal(DistanceKm * 12);

                    MinEstimatedPricePP = 600;
                    MaxEstimatedPricePP = 800;

                    if (EstimatedFare < 50)
                    {
                        EstimatedFare = 50;
                    }
                }
                else if((haslocalpickup && Drop.Contains("Airport"))||
                                   (Pickup.Contains("Airport") && haslocaldrop))
                {
                    DistanceKm = 247;

                    EstimatedFare =
                        Convert.ToDecimal(DistanceKm * 12);

                    if (EstimatedFare < 50)
                    {
                        EstimatedFare = 50;
                    }
                    MinEstimatedPricePP = 750;
                    MaxEstimatedPricePP = 950;
                }

                else if ((Pickup.Contains("ARMR") && hascitydrop)||
                         (hascitypickup && Drop.Contains("ARMR")))
                {
                    DistanceKm = 187;

                    EstimatedFare =
                        Convert.ToDecimal(DistanceKm * 12);

                    if (EstimatedFare < 50)
                    {
                        EstimatedFare = 50;
                    }
                    MinEstimatedPricePP = 450;
                    MaxEstimatedPricePP = 550;
                }
                else if ((Pickup.Contains("ARMR") && Drop.Contains("Airport"))||
                (Pickup.Contains("Airport") && Drop.Contains("ARMR")))
                {
                    DistanceKm = 224;

                    EstimatedFare =
                        Convert.ToDecimal(DistanceKm * 12);

                    if (EstimatedFare < 50)
                    {
                        EstimatedFare = 50;
                    }
                    MinEstimatedPricePP = 600;
                    MaxEstimatedPricePP = 800;
                }
                else if ((Pickup.Contains("NZB") && hascitydrop)||
                   (hascitypickup && Drop.Contains("NZB")))
                {
                    DistanceKm = 178;

                    EstimatedFare =
                        Convert.ToDecimal(DistanceKm * 12);

                    if (EstimatedFare < 50)
                    {
                        EstimatedFare = 50;
                    }
                    MinEstimatedPricePP = 450;
                    MaxEstimatedPricePP = 550;
                }
                else if ((Pickup.Contains("NZB") && Drop.Contains("Airport"))||
                  (Pickup.Contains("Airport") && Drop.Contains("NZB")))
                {
                    DistanceKm = 217;

                    EstimatedFare =
                        Convert.ToDecimal(DistanceKm * 12);

                    if (EstimatedFare < 50)
                    {
                        EstimatedFare = 50;
                    }
                    MinEstimatedPricePP = 600;
                    MaxEstimatedPricePP = 800;
                }

                // later:
                // fetch matching rides here
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
    }
}