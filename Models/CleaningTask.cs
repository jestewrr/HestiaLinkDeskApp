using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

        public DateTime AssignedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Status values: 'Assigned', 'In Progress', 'Completed', 'Maintenance'
        /// </summary>
        public string Status { get; set; } = "Assigned";

    public int UserId { get; set; }

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

    public virtual SystemUser User { get; set; } = null!;
}
