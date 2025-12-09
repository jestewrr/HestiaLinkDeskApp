using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class OperationalExpense
{
    public int ExpenseId { get; set; }

    public string Category { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Amount { get; set; }

    public DateTime ExpenseDate { get; set; }

    public string? ApprovedBy { get; set; }

    public string Status { get; set; } = null!;
}
