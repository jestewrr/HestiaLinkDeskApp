using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class CleaningTask
    {
        [Key]
        public int TaskID { get; set; }

        public int RoomID { get; set; }

        public int? UserID { get; set; } // Assigned Staff

        [Required]
        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending"; // Pending, In Progress, Completed

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? CompletedDate { get; set; }

        // Navigation
        [ForeignKey("RoomID")]
        public Room? Room { get; set; }

        [ForeignKey("UserID")]
        public SystemUser? AssignedUser { get; set; }
    }
}
