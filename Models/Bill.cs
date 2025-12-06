using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class Bill
    {
        [Key]
        public int BillID { get; set; }
        public int ReservationID { get; set; }
        public DateTime BillDate { get; set; } = DateTime.Now;
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubtotalAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal GrandTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceDue { get; set; }
        public string BillStatus { get; set; } = "Pending";

        // Navigation
        [ForeignKey("ReservationID")]
        public Reservation? Reservation { get; set; }
    }
}
