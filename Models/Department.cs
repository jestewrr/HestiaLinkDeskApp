using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();
}
