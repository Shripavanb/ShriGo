namespace ShriGo.Model
{
    public class UpdateFavoriteRequest
    {
        public int FavoriteRouteId { get; set; }

        public string RouteName { get; set; }

        public string RideFrom { get; set; }

        public string RideVia { get; set; }

        public string RideTo { get; set; }

        public decimal RidePrice { get; set; }

        public int RideSeats { get; set; }

        public string RideTime { get; set; }
    }
}
