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

        public string? UserFirstName { get; set; }

        public string? UserLastName { get; set; }

        public string? UserAge { get; set; }

        public string? UserEmail { get; set; }

        public string? UserContact { get; set; }

        public string? UserPswd { get; set; }

        public string? UserRole { get; set; }

        public string? Subscription { get; set; }

        public string? VehicleRegNo { get; set; }

        public string? VehicleInsur { get; set; }

        public string? VehicleModel { get; set; }

        public bool? AcceptedTerms { get; set; }
    }
}  