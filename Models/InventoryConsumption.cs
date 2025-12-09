<<<<<<< HEAD
﻿using System;
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
=======
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class InventoryConsumption
    {
        [Key]
        public int ConsumptionID { get; set; }

        [Required]
        public int ServiceTransactionID { get; set; }

        [Required]
        public int InventoryItemID { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal QuantityConsumed { get; set; }

        public DateTime ConsumptionDate { get; set; } = DateTime.Now;

        [StringLength(10)]
        public string? RoomNumber { get; set; }

        // Navigation Properties
        [ForeignKey("ServiceTransactionID")]
        public ServiceTransaction? ServiceTransaction { get; set; }

        [ForeignKey("InventoryItemID")]
        public InventoryItem? InventoryItem { get; set; }
    }
}
>>>>>>> origin/master
