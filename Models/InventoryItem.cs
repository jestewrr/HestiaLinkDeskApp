<<<<<<< HEAD
﻿using System;
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
=======
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class InventoryItem
    {
        [Key]
        public int ItemID { get; set; }

        [Required]
        [StringLength(50)]
        public string ItemCode { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty; // FOOD, BEVERAGE, AMENITY, CLEANING, MAINTENANCE

        [Required]
        [StringLength(20)]
        public string UnitOfMeasure { get; set; } = string.Empty; // KG, L, PC, BOX, SET

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitCost { get; set; }

        public int? CurrentStock { get; set; } = 0;

        public int? ReorderPoint { get; set; } = 10;

        public bool? IsActive { get; set; } = true;

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        // Supplier relationship
        public int? SupplierID { get; set; }

        [ForeignKey("SupplierID")]
        public virtual Supplier? Supplier { get; set; }
    }
}
>>>>>>> origin/master
