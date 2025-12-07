using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HestiaLink.Models
{
    public class Payroll
    {
        [Key]
        public int PayrollID { get; set; }

        public int? AttendanceID { get; set; }

        public int? IncomeID { get; set; }

        public int? TaxID { get; set; }

        [Column("PeriodStart")]
        public DateTime PeriodStart { get; set; }

        [Column("PeriodEnd")]
        public DateTime PeriodEnd { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalHours { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal OvertimeHours { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal HourlyRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal RegularPay { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OvertimePay { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossPay { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

        [Column("NetPay", TypeName = "decimal(18,2)")]
        public decimal NetPay { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Processing, Paid, On Hold

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("AttendanceID")]
        public Attendance? Attendance { get; set; }

        [ForeignKey("TaxID")]
        public Tax? Tax { get; set; }
    }
}
