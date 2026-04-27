using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShriGo.Model
{
    public class UserModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public string? UserUniqueId {  get; set; }

        public DateOnly? UserRegDate {  get; set; }

        public string? UserFirstName { get; set; }

        public string? UserLastName { get; set; }

        public string? UserAge {  get; set; }

        public string? UserEmail { get; set; }

        public string? UserContact { get; set; }

        public string? UserPswd {  get; set; }
        public string? UserRole { get; set; }
        public string? Subscription { get; set; }
        public string? VehicleRegNo { get; set; }
        public string? VehicleInsur { get; set; }
        public string? VehicleModel { get; set; }
    }
}
