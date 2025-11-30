using System.ComponentModel.DataAnnotations;

namespace HestiaLink.Models
{
    public class RoomType
    {
        [Key]
        public int RoomTypeID { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MaxOccupancy { get; set; }
        public decimal BasePrice { get; set; }
        public string Status { get; set; } = "Active";
    }
}