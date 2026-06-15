namespace ShriGo.Model
{
    public class BookRideRequest
    {
        public int RideId { get; set; }

        public int BookedSeats { get; set; }

        public string?
            PassengerFirstName
        { get; set; }

        public string?
            PassengerUniqueId
        { get; set; }

        public string?
            PassengerContact
        { get; set; }

        public string?
            PassengerEmail
        { get; set; }
    }
}