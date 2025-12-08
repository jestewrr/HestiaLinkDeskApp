using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    /// <summary>
    /// Represents a supplier/vendor for inventory items.
    /// </summary>
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; }

        [Required]
        [StringLength(20)]
        public string SupplierCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string SupplierName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ContactPerson { get; set; }

        [StringLength(20)]
        public string? ContactPhone { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string? ContactEmail { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(50)]
        public string? SupplierType { get; set; } // FOOD, BEVERAGE, AMENITIES, CLEANING, LINEN, OTHER

        public bool? IsActive { get; set; } = true;

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        public virtual ICollection<InventoryItem>? InventoryItems { get; set; }

        public virtual ICollection<InventoryPurchase>? InventoryPurchases { get; set; }
    }
}
