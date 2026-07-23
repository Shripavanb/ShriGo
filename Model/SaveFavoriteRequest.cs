namespace ShriGo.Model
{
    public class SaveFavoriteRequest
    {
        public string DriverUniqueId { get; set; }

        public string FavoriteName { get; set; }   // <-- Add this

        public string RouteName { get; set; }

        public string RideFrom { get; set; }

        public string RideVia { get; set; }

        public string RideTo { get; set; }

        public string RideTime { get; set; }

        public decimal RidePrice { get; set; }

        public int RideSeats { get; set; }
    }
}
