using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class VwRoomsNeedingCleaning
{
    public int RoomId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public int? Floor { get; set; }

    public string? Status { get; set; }

    public int? HoursSinceCheckout { get; set; }
}
