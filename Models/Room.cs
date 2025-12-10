using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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

    // Aliases for different naming conventions
    [NotMapped]
    public int RoomID { get => RoomId; set => RoomId = value; }
    
    [NotMapped]
    public int? RoomTypeID { get => RoomTypeId; set => RoomTypeId = value; }
}
