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

        public DateTime AssignedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Status values: 'Assigned', 'In Progress', 'Completed', 'Maintenance'
        /// </summary>
        public string Status { get; set; } = "Assigned";

        public DateTime? CompletedDate { get; set; }

        /// <summary>
        /// Additional notes for the task
        /// </summary>
        [MaxLength(500)]
        public string? Notes { get; set; }

        /// <summary>
        /// Completion status values: 'Cleaned', 'Maintenance Required'
        /// This helps categorize task history
        /// </summary>
        [MaxLength(20)]
        public string? CompletionStatus { get; set; }

        // Navigation properties
        [ForeignKey("RoomID")]
        public Room? Room { get; set; }

        [ForeignKey("UserID")]
        public SystemUser? AssignedUser { get; set; }
    }
}
