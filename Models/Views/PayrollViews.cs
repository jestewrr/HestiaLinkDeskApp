using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models.Views
{
    /// <summary>
    /// View model for vw_EmployeePayrollSummary database view
    /// Maps to: SELECT EmployeeID, EmployeeName, PositionTitle, DepartmentID, MonthlySalary, 
    ///          HourlyRate, TotalRegularHours, TotalOvertimeHours, TotalHours, TotalSalary, PaymentStatus
    /// Note: Configured as keyless in HestiaLinkContext.OnModelCreating
    /// </summary>
    public class EmployeePayrollSummaryView
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string PositionTitle { get; set; } = string.Empty;
        public int? DepartmentID { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlySalary { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal HourlyRate { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalRegularHours { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalOvertimeHours { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalHours { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalSalary { get; set; }
        
        public string PaymentStatus { get; set; } = "Pending";
    }

    /// <summary>
    /// View model for vw_EmployeeFullDetails database view
    /// Maps to: SELECT EmployeeID, FirstName, MiddleName, LastName, Email, ContactNumber, Address,
    ///          DateOfBirth, HireDate, EmployeeStatus, PositionID, PositionTitle, DepartmentID,
    ///          PositionLevel, PayGrade, Salary, JobDescription, PositionStatus
    /// Note: Configured as keyless in HestiaLinkContext.OnModelCreating
    /// </summary>
    public class EmployeeFullDetailsView
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? HireDate { get; set; }
        public string EmployeeStatus { get; set; } = "Active";
        public int? PositionID { get; set; }
        public string? PositionTitle { get; set; }
        public int? DepartmentID { get; set; }
        public string? PositionLevel { get; set; }
        public string? PayGrade { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
        
        public string? JobDescription { get; set; }
        public string? PositionStatus { get; set; }
    }
}
