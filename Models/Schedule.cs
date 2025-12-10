using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public int EmployeeId { get; set; }

    public DateOnly ScheduleDate { get; set; }

    public TimeOnly ScheduledStart { get; set; }

    public TimeOnly ScheduledEnd { get; set; }

    public bool? IsActive { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Employee Employee { get; set; } = null!;
}
