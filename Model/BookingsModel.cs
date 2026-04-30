using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShriGo.Model
{
    public class BookingsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId {  get; set; }

        public string? RideId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateOnly? RideDate { get; set; }
        public string? RideSource { get; set; }
        public string? RideDesti { get; set; }
        public string? RideVia { get; set; }
        public TimeOnly? RideTime { get; set; }
        public string? BookedSeats { get; set; }

        public string? RidePrice { get; set; }
        public string? DriverContact { get; set; }

        //will be adding value while uploading ride
        public string? DriverUniqueId { get; set; }

        public string? DriverFirstName { get; set; }

        public string? PassengerFirstName { get; set; }

        public string? PassengerUniqueId { get; set; }
        public string? PassengerContact { get; set; }
        public string? PassengerEmail { get; set; }

  
    }
}
