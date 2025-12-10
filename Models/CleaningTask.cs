using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models;

[Table("Task")]
public partial class CleaningTask
{
    [Key]
    [Column("TaskID")]
    public int TaskID { get; set; }

    [Column("RoomID")]
    public int RoomID { get; set; }

    [Column("UserID")]
    public int? UserID { get; set; }

    public DateTime AssignedDate { get; set; } = DateTime.Now;

    public DateTime? CompletedDate { get; set; }

    /// <summary>
    /// Status values: 'Assigned', 'In Progress', 'Completed', 'Maintenance'
    /// </summary>
    [MaxLength(20)]
    public string Status { get; set; } = "Assigned";

    /// <summary>
    /// Additional notes for the task
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    /// <summary>
    /// Completion status values: 'Cleaned', 'Maintenance Required'
    /// This helps categorize task history
    /// </summary>
    [NotMapped]
    public string? CompletionStatus { get; set; }

    // Navigation properties
    [ForeignKey("RoomID")]
    public virtual Room? Room { get; set; }

    [ForeignKey("UserID")]
    public virtual SystemUser? AssignedUser { get; set; }
}
