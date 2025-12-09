using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class CleaningTask
{
    public int TaskId { get; set; }

    public int RoomId { get; set; }

    public int UserId { get; set; }

    public virtual Room Room { get; set; } = null!;

    public virtual SystemUser User { get; set; } = null!;
}
