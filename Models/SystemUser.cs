using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class SystemUser
{
    public int UserId { get; set; }

    public int? EmployeeId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Role { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string Status { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<CleaningTask> Tasks { get; set; } = new List<CleaningTask>();
}
