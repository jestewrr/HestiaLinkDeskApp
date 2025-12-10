using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class Bill
{
    public int BillId { get; set; }

    public int? ReservationId { get; set; }

    public DateTime? BillDate { get; set; }

    public decimal? SubtotalAmount { get; set; }

    public decimal? GrandTotal { get; set; }

    public decimal? AmountPaid { get; set; }

    public decimal? BalanceDue { get; set; }

    public string? BillStatus { get; set; }

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Reservation? Reservation { get; set; }
}
