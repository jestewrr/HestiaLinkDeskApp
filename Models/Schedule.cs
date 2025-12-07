using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleID { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        public DateTime ScheduleDate { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan ScheduledStart { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeSpan ScheduledEnd { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("EmployeeID")]
        public Employee? Employee { get; set; }
    }
}
