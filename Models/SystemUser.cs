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
    }
}