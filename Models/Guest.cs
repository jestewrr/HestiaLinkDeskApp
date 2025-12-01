using System.ComponentModel.DataAnnotations;

namespace HestiaLink.Models
{
    public class Guest
    {
        [Key]
        public int GuestID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string ContactNumber { get; set; } = string.Empty;
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // convenience
        public string FullName => (FirstName + " " + LastName).Trim();
    }
}