using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShriGo.Model
{
    public class RideModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RideId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? RideDate { get; set; }
        public string? RideSource { get; set; }
        public string? RideDesti { get; set; }
        public string? RideVia { get; set; }
        public string? RideTime { get; set; }
        public string? RideSeats { get; set; }
        public string? RideContact { get; set; }
    }
}
