using System;
using System.Collections.Generic;

namespace HestiaLink.Models;

public partial class ServiceCategory
{
    public int ServiceCategoryId { get; set; }

    public string ServiceCategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Status { get; set; }

    public string? Function { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
