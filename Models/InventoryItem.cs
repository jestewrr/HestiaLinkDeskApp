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

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty; // FOOD, BEVERAGE, AMENITY, CLEANING, MAINTENANCE

        [Required]
        [StringLength(20)]
        public string UnitOfMeasure { get; set; } = string.Empty; // KG, L, PC, BOX, SET

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitCost { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? SellingPrice { get; set; }

        public int ReorderPoint { get; set; } = 10;

        public int CurrentStock { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}