using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class RoomType
{
    public int RoomTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public string? Description { get; set; }

    public int? MaxOccupancy { get; set; }

    public decimal BasePrice { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
