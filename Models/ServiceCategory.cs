using System.ComponentModel.DataAnnotations;

namespace HestiaLink.Models
{
    public class ServiceCategory
    {
        [Key]
        public int ServiceCategoryID { get; set; }

        [Required]
        [StringLength(100)]
        public string ServiceCategoryName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Function { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [StringLength(20)]
        public string Status { get; set; } = "Active";

        // Navigation property
        public ICollection<Service>? Services { get; set; }
    }
}