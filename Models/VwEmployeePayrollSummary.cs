using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class VwEmployeePayrollSummary
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string PositionTitle { get; set; } = null!;

    public int? DepartmentId { get; set; }

    public decimal? MonthlySalary { get; set; }

    public decimal? HourlyRate { get; set; }

    public decimal? TotalRegularHours { get; set; }

    public decimal? TotalOvertimeHours { get; set; }

    public decimal? TotalHours { get; set; }

    public decimal? TotalSalary { get; set; }

    public string PaymentStatus { get; set; } = null!;
}
