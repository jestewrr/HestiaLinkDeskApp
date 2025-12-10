using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class Payroll
{
    public int PayrollId { get; set; }

    public int AttendanceId { get; set; }

    public int? IncomeId { get; set; }

    public int? TaxId { get; set; }

    public DateOnly PeriodStart { get; set; }

    public DateOnly PeriodEnd { get; set; }

    public decimal? TotalHours { get; set; }

    public decimal? OvertimeHours { get; set; }

    public decimal? HourlyRate { get; set; }

    public decimal? RegularPay { get; set; }

    public decimal? OvertimePay { get; set; }

    public decimal? GrossPay { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal? NetPay { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Attendance Attendance { get; set; } = null!;

    public virtual TotalIncome? Income { get; set; }

    public virtual Tax? Tax { get; set; }
}
