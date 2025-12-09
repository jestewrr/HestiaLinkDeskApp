using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class ReservedRoom
{
    public int ReservedRoomId { get; set; }

    public int? ReservationId { get; set; }

    public int? RoomId { get; set; }

    public virtual Reservation? Reservation { get; set; }

    public virtual Room? Room { get; set; }
}
