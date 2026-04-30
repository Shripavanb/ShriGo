using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShriGo.Model
{
    public class PassengerModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PassengerId { get; set; }
        public string? PassengerUniqueId { get; set; }

        public DateOnly? PassengerRegDate { get; set; }
        public string? PassengerFirstName { get; set; }
        public string? PassengerLastName { get; set; }
        public string? PassengerAge { get; set; }
        public string? PassengerEmail { get; set; }
        public string? PassengerContact { get; set; }
        public string? PassengerPswd { get; set; }
    }
}