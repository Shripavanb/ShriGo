using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShriGo.Model
{
    public class NotificationModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }

        public string? DriverUniqueId { get; set; }

        public string? Title { get; set; }

        public string? Message { get; set; }

        public string? NotificationType { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedDate { get; set; }
    }
}