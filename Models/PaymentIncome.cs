using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class PaymentIncome
    {
        [Key]
        public int PaymentIncomeID { get; set; }

        [Required]
        public int PaymentID { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("PaymentID")]
        public Payment? Payment { get; set; }
    }
}
