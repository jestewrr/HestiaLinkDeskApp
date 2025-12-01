using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationID { get; set; }
        public int GuestID { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Status { get; set; } = "Checked In";
        public string PaymentStatus { get; set; } = "Pending";
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPayment { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation
        public Guest? Guest { get; set; }
    }
}