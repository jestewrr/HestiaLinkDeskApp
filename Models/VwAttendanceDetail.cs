using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class VwAttendanceDetail
{
    public int AttendanceId { get; set; }

    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public DateOnly AttendanceDate { get; set; }

    public TimeOnly ScheduledStart { get; set; }

    public TimeOnly ScheduledEnd { get; set; }

    public DateTime? ActualCheckIn { get; set; }

    public DateTime? ActualCheckOut { get; set; }

    public decimal? RegularHours { get; set; }

    public decimal? OvertimeHours { get; set; }

    public string? AttendanceStatus { get; set; }

    public bool? IsListed { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int ScheduledD { get; set; }
}
