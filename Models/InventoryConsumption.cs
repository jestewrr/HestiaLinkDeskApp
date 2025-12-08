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