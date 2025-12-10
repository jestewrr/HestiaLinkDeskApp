using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class InventoryConsumption
    {
        [Key]
        public int ConsumptionId { get; set; }

        [Required]
        public int ServiceTransactionId { get; set; }

        [Required]
        public int InventoryItemId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal QuantityConsumed { get; set; }

        public DateTime ConsumptionDate { get; set; } = DateTime.Now;

        [StringLength(10)]
        public string? RoomNumber { get; set; }

        // Navigation Properties
        [ForeignKey("ServiceTransactionId")]
        public ServiceTransaction? ServiceTransaction { get; set; }

        [ForeignKey("InventoryItemId")]
        public InventoryItem? InventoryItem { get; set; }
    }
}
