using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class ServiceInventory
    {
        [Key]
        public int ServiceInventoryId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int InventoryItemId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal QuantityRequired { get; set; } = 1.0m;

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("ServiceId")]
        public Service? Service { get; set; }

        [ForeignKey("InventoryItemId")]
        public InventoryItem? InventoryItem { get; set; }
    }
}
