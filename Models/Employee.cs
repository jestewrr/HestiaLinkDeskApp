using System;
using System.ComponentModel.DataAnnotations;

namespace HestiaLink.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public int? PositionID { get; set; }

        public DateTime? HireDate { get; set; }

        // Use "Active" or "Archived"
        public string Status { get; set; } = "Active";
    }
}