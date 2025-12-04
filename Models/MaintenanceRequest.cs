using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class MaintenanceRequest
    {
        [Key]
        public int RequestID { get; set; }

        public int RoomID { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending"; // Pending, In Progress, Completed

        public DateTime ReportedDate { get; set; } = DateTime.Now;

        public DateTime? ResolvedDate { get; set; }

        // Navigation
        [ForeignKey("RoomID")]
        public Room? Room { get; set; }
    }
}
