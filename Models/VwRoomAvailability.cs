using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class VwRoomAvailability
{
    public string RoomNumber { get; set; } = null!;

    public int? Floor { get; set; }

    public string? TypeName { get; set; }

    public decimal? BasePrice { get; set; }

    public string? CurrentStatus { get; set; }

    public string CurrentGuest { get; set; } = null!;

    public DateOnly? CheckInDate { get; set; }

    public DateOnly? CheckOutDate { get; set; }

    public int? NightsRemaining { get; set; }
}
