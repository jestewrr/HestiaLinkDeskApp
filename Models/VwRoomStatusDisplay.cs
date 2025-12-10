using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class VwRoomStatusDisplay
{
    public long? CardNumber { get; set; }

    public string RoomInfo { get; set; } = null!;

    public string? Status { get; set; }

    public string Guest { get; set; } = null!;

    public DateOnly? CheckIn { get; set; }

    public DateOnly? CheckOut { get; set; }

    public string RoomNumber { get; set; } = null!;

    public int? Floor { get; set; }

    public string? TypeName { get; set; }
}
