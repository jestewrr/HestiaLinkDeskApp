using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    // Corresponds to vw_RoomsNeedingCleaning
    public class RoomNeedingCleaningView
    {
        public int RoomID { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public int Floor { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    // Corresponds to vw_ActiveCleaningTasks
    public class ActiveCleaningTaskView
    {
        public int TaskID { get; set; }
        public int RoomID { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public int? AssignedUserID { get; set; }
        public string AssignedUser { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
