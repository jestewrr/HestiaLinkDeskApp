using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class ServiceInventory
{
    public int ServiceInventoryId { get; set; }

    public int ServiceId { get; set; }

    public int InventoryItemId { get; set; }

    public decimal QuantityRequired { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual InventoryItem InventoryItem { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
