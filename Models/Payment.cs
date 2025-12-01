using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }
        public int? BillID { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public string PaymentMethod { get; set; } = "Cash";
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public string? ReferenceNumber { get; set; }
        public string PaymentStatus { get; set; } = "Paid";
        public int? ProcessedBy { get; set; }
    }
}