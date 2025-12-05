using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceID { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        public DateTime AttendanceDate { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? ScheduleStart { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? ScheduleEnd { get; set; }

        public DateTime? ActualCheckIn { get; set; }

        public DateTime? ActualCheckOut { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal RegularHours { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal OvertimeHours { get; set; } = 0;

        [StringLength(20)]
        public string AttendanceStatus { get; set; } = "Absent"; // Present, Absent, On Leave, Half Day

        public bool IsLate { get; set; } = false;

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        [ForeignKey("EmployeeID")]
        public Employee? Employee { get; set; }
    }
}
