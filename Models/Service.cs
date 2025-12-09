using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = null!;

<<<<<<< HEAD
    public int? ServiceCategoryId { get; set; }
=======
        public int? ServiceCategoryID { get; set; }
>>>>>>> origin/master

    public decimal StandardPrice { get; set; }

    public bool? IsActive { get; set; }

<<<<<<< HEAD
    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? UsesInventory { get; set; }

    public string? InventoryNotes { get; set; }

    public virtual ServiceCategory? ServiceCategory { get; set; }

    public virtual ICollection<ServiceInventory> ServiceInventories { get; set; } = new List<ServiceInventory>();

    public virtual ICollection<ServiceTransaction> ServiceTransactions { get; set; } = new List<ServiceTransaction>();
}
=======
        public bool? IsActive { get; set; } = true;

        [StringLength(50)]
        public string? Status { get; set; } = "Active";

        [Display(Name = "Uses Inventory")]
        public bool? UsesInventory { get; set; } = false;

        [Display(Name = "Inventory Notes")]
        [StringLength(500)]
        public string? InventoryNotes { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
>>>>>>> origin/master
