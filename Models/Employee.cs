using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public string? ContactNumber { get; set; }

    public string? Address { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public int? PositionId { get; set; }

    public DateOnly? HireDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Position? Position { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<SystemUser> SystemUsers { get; set; } = new List<SystemUser>();
}
