using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShriGo.Model
{
    [Table("DriversTb")]
    public class DriverModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DriverId { get; set; }

        public string? DriverUniqueId { get; set; }

        public DateOnly? DriverRegDate { get; set; }

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

        public bool? AcceptedTerms { get; set; } = false;

        public string? PasswordResetToken { get; set; }

        public DateTime? ResetTokenExpiry { get; set; }

        public string? DriverImagePath { get; set; }
    }
}
