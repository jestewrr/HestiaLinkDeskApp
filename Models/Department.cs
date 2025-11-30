using System.ComponentModel.DataAnnotations;

namespace HestiaLink.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "Department name is required")]
        public string DepartmentName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = "Active";
    }
}
