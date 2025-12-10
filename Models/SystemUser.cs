using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models;

public partial class SystemUser
{
    [Key]
    [Column("UserID")]
    public int UserID { get; set; }
    
    [Column("EmployeeID")]
    public int? EmployeeID { get; set; }
    
    public string Username { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public string Role { get; set; } = string.Empty;
    
    public DateTime? CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public string Status { get; set; } = "Active";

    /// <summary>
    /// Tracks if the user (especially housekeeping staff) is available for new task assignments.
    /// Note: This is NOT mapped to the database - used only in memory for application logic.
    /// </summary>
    [NotMapped]
    public bool IsAvailable { get; set; } = true;

    // Aliases for different naming conventions used in the codebase
    [NotMapped]
    public int UserId { get => UserID; set => UserID = value; }
    
    [NotMapped]
    public int? EmployeeId { get => EmployeeID; set => EmployeeID = value; }
}
