using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class Income
{
    public int IncomeId { get; set; }

    public int? PaymentId { get; set; }

    public DateOnly? IncomeDate { get; set; }

    public decimal? Amount { get; set; }

    public string? IncomeType { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Payment? Payment { get; set; }
}
