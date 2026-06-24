namespace ShriGo.Model
{
    public class SignupRequest
    {
        //----------------------------------
        // Passenger
        //----------------------------------

        public string? PassengerFirstName { get; set; }

        public string? PassengerLastName { get; set; }

        public string? PassengerAge { get; set; }

        public string? PassengerEmail { get; set; }

        public string? PassengerContact { get; set; }

        public string? PassengerPswd { get; set; }

        public string? Role { get; set; }

        //----------------------------------
        // Driver
        //----------------------------------

        public string? DriverFirstName { get; set; }

        public string? DriverLastName { get; set; }

        public string? DriverAge { get; set; }

        public string? DriverEmail { get; set; }

        public string? DriverContact { get; set; }

        public string? DriverPswd { get; set; }

        public string? DriverRole { get; set; }

        public string? Subscription { get; set; }

        public string? VehicleRegNo { get; set; }

        public string? VehicleInsur { get; set; }

        public string? VehicleModel { get; set; }

        public bool? AcceptedTerms { get; set; }
    }
}  