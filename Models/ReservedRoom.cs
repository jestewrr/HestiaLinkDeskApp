using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models;

public partial class ReservedRoom
{
    public int ReservedRoomId { get; set; }

    public int? ReservationId { get; set; }

    public int? RoomId { get; set; }

    public virtual Reservation? Reservation { get; set; }

    public virtual Room? Room { get; set; }

    // Alias for different naming conventions
    [NotMapped]
    public int? ReservationID { get => ReservationId; set => ReservationId = value; }
}
