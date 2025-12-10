using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class Guest
{
    public int GuestId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public string? ContactNumber { get; set; }

    public string? Address { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
