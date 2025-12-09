using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class BillDetail
{
    public int BillDetailId { get; set; }

    public int? BillId { get; set; }

    public string? Description { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? TransactionDate { get; set; }

    public virtual Bill? Bill { get; set; }
}
