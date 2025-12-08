using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class Service
    {
        [Key]
        public int ServiceID { get; set; }

        [Required]
        [StringLength(100)]
        public string ServiceName { get; set; } = string.Empty;

        [Required]
        public int ServiceCategoryID { get; set; }

        [ForeignKey("ServiceCategoryID")]
        public ServiceCategory? Category { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal StandardPrice { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(20)]
        public string Status { get; set; } = "Active";

        [Display(Name = "Uses Inventory")]
        public bool UsesInventory { get; set; } = false;

        [Display(Name = "Inventory Notes")]
        [StringLength(500)]
        public string? InventoryNotes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}