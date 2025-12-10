using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class VwEmployeeFullDetail
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public string? ContactNumber { get; set; }

    public string? Address { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public DateOnly? HireDate { get; set; }

    public string EmployeeStatus { get; set; } = null!;

    public int PositionId { get; set; }

    public string PositionTitle { get; set; } = null!;

    public int? DepartmentId { get; set; }

    public string? PositionLevel { get; set; }

    public string? PayGrade { get; set; }

    public decimal? Salary { get; set; }

    public string? JobDescription { get; set; }

    public string? PositionStatus { get; set; }
}
