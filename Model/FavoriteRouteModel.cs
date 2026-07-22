namespace ShriGo.Model
{
    public class FavoriteRouteModel
    {
        public int Id { get; set; }

        public string FavoriteName { get; set; }

        public string DriverUniqueId { get; set; }

        public string RouteName { get; set; }

        public string RideFrom { get; set; }

        public string RideVia { get; set; }

        public string RideTo { get; set; }

        public string RideTime { get; set; }

        public decimal RidePrice { get; set; }

        public int RideSeats { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

        // Reserved for future analytics and frequently-used route sorting
        public int UsageCount { get; set; }
    }
}
