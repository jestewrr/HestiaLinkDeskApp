using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class Position
{
    public int PositionId { get; set; }

    public int? DepartmentId { get; set; }

    public string PositionTitle { get; set; } = null!;

    public string? PositionLevel { get; set; }

    public string? PayGrade { get; set; }

    public decimal? Salary { get; set; }

    public string? JobDescription { get; set; }

    public string? Status { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
