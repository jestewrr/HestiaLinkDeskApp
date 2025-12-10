using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class ServiceTransaction
{
    public int ServiceTransactionId { get; set; }

    public int? ReservationId { get; set; }

    public int? ServiceId { get; set; }

    public DateTime? TransactionDateTime { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? ServiceStatus { get; set; }

    public virtual ICollection<InventoryConsumption> InventoryConsumptions { get; set; } = new List<InventoryConsumption>();

    public virtual Reservation? Reservation { get; set; }

    public virtual Service? Service { get; set; }
}
