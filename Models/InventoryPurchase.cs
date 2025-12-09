using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    /// <summary>
    /// Represents a purchase order for inventory restocking.
    /// </summary>
    public class InventoryPurchase
    {
        [Key]
        public int PurchaseID { get; set; }

        [Required]
        [StringLength(50)]
        public string PurchaseNumber { get; set; } = string.Empty;

        [Required]
        public int ItemID { get; set; }

        public int? SupplierID { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalAmount { get; set; }

        public DateTime? PurchaseDate { get; set; } = DateTime.Now;

        public DateTime? ReceivedDate { get; set; }

        [StringLength(20)]
        public string PurchaseStatus { get; set; } = "PENDING"; // PENDING, RECEIVED, CANCELLED

        [StringLength(1000)]
        public string? Notes { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("ItemID")]
        public virtual InventoryItem? InventoryItem { get; set; }

        [ForeignKey("SupplierID")]
        public virtual Supplier? Supplier { get; set; }

        /// <summary>
        /// Calculates the total amount based on quantity and unit price.
        /// </summary>
        public void CalculateTotal()
        {
            TotalAmount = Quantity * UnitPrice;
        }
    }
}
