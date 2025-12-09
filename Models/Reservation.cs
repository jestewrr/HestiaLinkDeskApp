using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class Reservation
{
    public int ReservationId { get; set; }

    public int? GuestId { get; set; }

    public DateOnly CheckInDate { get; set; }

    public DateOnly CheckOutDate { get; set; }

    public string? Status { get; set; }

    public string? PaymentStatus { get; set; }

    public decimal? TotalPayment { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual Guest? Guest { get; set; }

    public virtual ICollection<ReservedRoom> ReservedRooms { get; set; } = new List<ReservedRoom>();

    public virtual ICollection<ServiceTransaction> ServiceTransactions { get; set; } = new List<ServiceTransaction>();
}
