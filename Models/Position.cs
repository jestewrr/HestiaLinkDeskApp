using System.ComponentModel.DataAnnotations;

namespace HestiaLink.Models
{
    public class Position
    {
        public int PositionID { get; set; }

        public int DepartmentID { get; set; }

        // Navigation property
        public Department? Department { get; set; }

        [Required]
        public string PositionTitle { get; set; } = string.Empty;

        public string PositionLevel { get; set; } = string.Empty;

        public string PayGrade { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public string JobDescription { get; set; } = string.Empty;

        public string Status { get; set; } = "Active";
    }
}