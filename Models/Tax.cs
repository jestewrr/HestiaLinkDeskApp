using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class Tax
{
    public int TaxId { get; set; }

    public string TaxName { get; set; } = null!;

    public int TaxPercentage { get; set; }

    public string? TaxDescription { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();
}
