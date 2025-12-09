using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class TotalIncome
{
    public int IncomeId { get; set; }

    public DateOnly PeriodStart { get; set; }

    public DateOnly PeriodEnd { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
}
