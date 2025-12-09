using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class VwRoomsBasic
{
    public string RoomNumber { get; set; } = null!;

    public int? Floor { get; set; }

    public string? Status { get; set; }

    public decimal BasePrice { get; set; }

    public string TypeName { get; set; } = null!;
}
