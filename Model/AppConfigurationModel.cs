using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShriGo.Model
{
    public class AppConfigurationModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //------------------------------------------------------
        // Version Information
        //------------------------------------------------------
        public string LatestVersion { get; set; }

        public string MinimumVersion { get; set; }

        public bool ForceUpdate { get; set; }

        //------------------------------------------------------
        // Update Dialog
        //------------------------------------------------------
        public string UpdateTitle { get; set; }

        public string UpdateMessage { get; set; }

        public string ReleaseNotes { get; set; }

        public string PlayStoreUrl { get; set; }

        //------------------------------------------------------
        // Maintenance
        //------------------------------------------------------
        public bool MaintenanceMode { get; set; }

        public string MaintenanceTitle { get; set; }

        public string MaintenanceMessage { get; set; }

        //------------------------------------------------------
        // Feature Flags
        //------------------------------------------------------
        public bool EnablePassengerBooking { get; set; }

        public bool EnableDriverPosting { get; set; }

        //------------------------------------------------------
        // General
        //------------------------------------------------------
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
