using System.ComponentModel.DataAnnotations;

namespace HestiaLink.Models
{
    public class ReservedRoom
    {
        [Key]
        public int ReservedRoomID { get; set; }
        public int ReservationID { get; set; }
        public int RoomID { get; set; }

        // Navigation
        public Reservation? Reservation { get; set; }
        public Room? Room { get; set; }
    }
}