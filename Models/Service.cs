using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public partial class Service
    {
        [Key]
        public int ServiceId { get; set; }

        [Required]
        [StringLength(100)]
        public string ServiceName { get; set; } = string.Empty;

        public int? ServiceCategoryId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal StandardPrice { get; set; }

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

        // Navigation Properties
        [ForeignKey("ServiceCategoryId")]
        public virtual ServiceCategory? ServiceCategory { get; set; }

        public virtual ICollection<ServiceInventory> ServiceInventories { get; set; } = new List<ServiceInventory>();

        public virtual ICollection<ServiceTransaction> ServiceTransactions { get; set; } = new List<ServiceTransaction>();
    }
}
