using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class VwBookingHistory
{
    public string? BookingId { get; set; }

    public string GuestName { get; set; } = null!;

    public string Room { get; set; } = null!;

    public DateOnly CheckIn { get; set; }

    public DateOnly CheckOut { get; set; }

    public string? BookingStatus { get; set; }

    public string? PaymentStatus { get; set; }

    public decimal? Total { get; set; }

    public DateTime? BookingDate { get; set; }

    public int? Nights { get; set; }

    public string? Email { get; set; }

    public string? ContactNumber { get; set; }
}
