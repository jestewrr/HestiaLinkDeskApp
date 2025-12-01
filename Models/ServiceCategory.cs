using System;
using System.ComponentModel.DataAnnotations;

namespace HestiaLink.Models
{
    public class ServiceCategory
    {
        [Key]
        public int ServiceCategoryID { get; set; }
        public string ServiceCategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Status { get; set; } = "Active"; // Active / Inactive
    }
}
