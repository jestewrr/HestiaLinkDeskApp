using System.ComponentModel.DataAnnotations;

namespace HestiaLink.Models
{
    public class ServiceCategory
    {
        [Key]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        public string CategoryName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = "Active";

        public DateTime? CreatedAt { get; set; }
    }
}
