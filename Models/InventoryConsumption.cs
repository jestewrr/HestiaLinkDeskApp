using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class InventoryConsumption
{
    public int ConsumptionId { get; set; }

    public int ServiceTransactionId { get; set; }

    public int InventoryItemId { get; set; }

    public decimal QuantityConsumed { get; set; }

    public DateTime? ConsumptionDate { get; set; }

    public string? RoomNumber { get; set; }

    public virtual InventoryItem InventoryItem { get; set; } = null!;

    public virtual ServiceTransaction ServiceTransaction { get; set; } = null!;
}
