using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class Tax
    {
        [Key]
        public int TaxID { get; set; }

        [Required]
        [StringLength(100)]
        public string TaxName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(5,2)")]
        public decimal TaxPercentage { get; set; }

        [StringLength(500)]
        public string? TaxDescription { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Active"; // Active, Archived
    }
}
