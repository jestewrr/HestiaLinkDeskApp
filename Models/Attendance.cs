using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class Attendance
{
    public int AttendanceId { get; set; }

    public int ScheduleId { get; set; }

    public DateOnly AttendanceDate { get; set; }

    public DateTime? ActualCheckIn { get; set; }

    public DateTime? ActualCheckOut { get; set; }

    public decimal? RegularHours { get; set; }

    public decimal? OvertimeHours { get; set; }

    public string? AttendanceStatus { get; set; }

    public bool? IsListed { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();

    public virtual Schedule Schedule { get; set; } = null!;
}
