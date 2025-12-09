using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public int? RoomTypeId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public int? Floor { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<ReservedRoom> ReservedRooms { get; set; } = new List<ReservedRoom>();

    public virtual RoomType? RoomType { get; set; }

    public virtual ICollection<CleaningTask> Tasks { get; set; } = new List<CleaningTask>();
}
