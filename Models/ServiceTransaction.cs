using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class ServiceTransaction
    {
        [Key]
        public int ServiceTransactionID { get; set; }
        public int ReservationID { get; set; }
        public int ServiceID { get; set; }
        public int Quantity { get; set; } = 1;
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public string ServiceStatus { get; set; } = "Pending";

        // Navigation
        public Reservation? Reservation { get; set; }
        public Service? Service { get; set; }
    }
}