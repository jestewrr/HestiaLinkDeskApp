using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class InventoryItem
{
    public int ItemId { get; set; }

    public string ItemCode { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string UnitOfMeasure { get; set; } = null!;

    public decimal UnitCost { get; set; }

    public int? CurrentStock { get; set; }

    public int? ReorderPoint { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<InventoryConsumption> InventoryConsumptions { get; set; } = new List<InventoryConsumption>();

    public virtual ICollection<ServiceInventory> ServiceInventories { get; set; } = new List<ServiceInventory>();
}
