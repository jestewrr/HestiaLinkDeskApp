using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class Room
    {
        [Key]
        public int RoomID { get; set; }
        public int? RoomTypeID { get; set; }

        public string RoomNumber { get; set; } = string.Empty;

        public int Floor { get; set; }
        public string Status { get; set; } = "Available";

        // UI-only fields (not present in DB table shown)
        [NotMapped]
        public int MaxOccupancy { get; set; }
        [NotMapped]
        public decimal Price { get; set; }
        [NotMapped]
        public string Description { get; set; } = string.Empty;

        // Navigation
        public RoomType? RoomType { get; set; }
    }
}