using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = null!;

    public int? ServiceCategoryId { get; set; }

    public decimal StandardPrice { get; set; }

    public bool? IsActive { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? UsesInventory { get; set; }

    public string? InventoryNotes { get; set; }

    public virtual ServiceCategory? ServiceCategory { get; set; }

    public virtual ICollection<ServiceInventory> ServiceInventories { get; set; } = new List<ServiceInventory>();

    public virtual ICollection<ServiceTransaction> ServiceTransactions { get; set; } = new List<ServiceTransaction>();
}
