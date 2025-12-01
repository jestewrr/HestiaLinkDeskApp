using System.ComponentModel.DataAnnotations;

namespace HestiaLink.Models
{
    public class Service
    {
        [Key]
        public int ServiceID { get; set; }

        [Required(ErrorMessage = "Service name is required")]
        public string ServiceName { get; set; } = string.Empty;

        public int? CategoryID { get; set; }

        // Navigation property
        public ServiceCategory? ServiceCategory { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = "Active";

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
