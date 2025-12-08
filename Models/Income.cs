using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class Income
    {
        [Key]
        public int IncomeID { get; set; }

        public int PaymentID { get; set; }

        [DataType(DataType.Date)]
        public DateTime IncomeDate { get; set; } = DateTime.Today;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [StringLength(50)]
        public string IncomeType { get; set; } = "Checkout";

        [StringLength(255)]
        public string Description { get; set; } = "CheckInAndCheckout";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("PaymentID")]
        public virtual Payment Payment { get; set; }
    }
}