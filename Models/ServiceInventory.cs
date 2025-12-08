using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class ServiceInventory
    {
        [Key]
        public int ServiceInventoryID { get; set; }

        [Required]
        public int ServiceID { get; set; }

        [Required]
        public int InventoryItemID { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal QuantityRequired { get; set; } = 1.0m;

        public bool IsOptional { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("ServiceID")]
        public Service? Service { get; set; }

        [ForeignKey("InventoryItemID")]
        public InventoryItem? InventoryItem { get; set; }
    }
}