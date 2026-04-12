using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShriGo.Model
{
    public class DriverModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DriverId { get; set; }


        public string? DriverUniqueId {  get; set; }

        public DateOnly? DriverRegDate {  get; set; }

        public string? DriverFirstName { get; set; }

        public string? DriverLastName { get; set; }

        public int DriverAge {  get; set; }

        public string? DriverEmail { get; set; }

        public int DriverContact { get; set; }

        public string? DriverPswd {  get; set; }
    }
}
