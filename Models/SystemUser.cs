using System;
using System.ComponentModel.DataAnnotations;

namespace HestiaLink.Models
{
    public class SystemUser
    {
        [Key]
        public int UserID { get; set; }
        public int? EmployeeID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // store hashed in production
        public string Role { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Status { get; set; } = "Active"; // Active / Archived
        
        /// <summary>
        /// Tracks if the user (especially housekeeping staff) is available for new task assignments.
        /// When a task is assigned, this is set to false. When task is completed, it's set back to true.
        /// </summary>
        public bool IsAvailable { get; set; } = true;
    }
}