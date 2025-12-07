using HestiaLink.Data;
using HestiaLink.Models;
using Microsoft.EntityFrameworkCore;

namespace HestiaLink.Services
{
    public class AttendanceService
    {
        private readonly HestiaLinkContext _context;

        public AttendanceService(HestiaLinkContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all active employees with their schedules and attendance for a specific date
        /// </summary>
        public async Task<List<EmployeeWithScheduleDto>> GetAllEmployeesForDateAsync(DateTime date)
        {
            try
            {
                // Get ALL active employees
                var allEmployees = await _context.Employees
                    .Where(e => e.Status == "Active")
                    .Include(e => e.Position)
                    .ThenInclude(p => p!.Department)
                    .AsNoTracking()
                    .ToListAsync();

                // Get schedules for this date
                var schedules = await _context.Schedules
                    .Where(s => s.ScheduleDate.Date == date.Date && s.IsActive)
                    .AsNoTracking()
                    .ToDictionaryAsync(s => s.EmployeeID);

                // Get attendance for this date
                var attendances = await _context.Attendances
                    .Where(a => a.AttendanceDate.Date == date.Date)
                    .AsNoTracking()
                    .ToDictionaryAsync(a => a.EmployeeID);

                var result = new List<EmployeeWithScheduleDto>();

                foreach (var emp in allEmployees)
                {
                    var position = emp.Position;
                    var department = position?.Department;
                    schedules.TryGetValue(emp.EmployeeID, out var schedule);
                    attendances.TryGetValue(emp.EmployeeID, out var attendance);

                    // Calculate IsLate dynamically
                    var isLate = false;
                    if (attendance?.ActualCheckIn != null && schedule != null)
                    {
                        var graceTime = schedule.ScheduledStart.Add(TimeSpan.FromMinutes(15));
                        isLate = attendance.ActualCheckIn.Value.TimeOfDay > graceTime;
                    }

                    var dto = new EmployeeWithScheduleDto
                    {
                        EmployeeID = emp.EmployeeID,
                        FirstName = emp.FirstName,
                        LastName = emp.LastName,
                        Department = department?.DepartmentName ?? "-",
                        Position = position?.PositionTitle ?? "-",
                        AttendanceDate = date,
                        ScheduleID = schedule?.ScheduleID,
                        ScheduleStart = schedule?.ScheduledStart,
                        ScheduleEnd = schedule?.ScheduledEnd,
                        AttendanceID = attendance?.AttendanceID,
                        ActualCheckIn = attendance?.ActualCheckIn,
                        ActualCheckOut = attendance?.ActualCheckOut,
                        AttendanceStatus = attendance?.AttendanceStatus ?? (schedule != null ? "Pending" : "Not Scheduled"),
                        IsLate = isLate,
                        RegularHours = attendance?.RegularHours ?? 0,
                        OvertimeHours = attendance?.OvertimeHours ?? 0,
                        Notes = attendance?.Notes
                    };

                    result.Add(dto);
                }

                return result.OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting employees for date: {ex.Message}");
                return new List<EmployeeWithScheduleDto>();
            }
        }

        /// <summary>
        /// Gets attendance records for a specific date
        /// </summary>
        public async Task<List<AttendanceRecordDto>> GetAttendanceByDateAsync(DateTime attendanceDate, int? departmentId = null)
        {
            try
            {
                var records = await _context.Attendances
                    .Where(a => a.AttendanceDate.Date == attendanceDate.Date)
                    .Include(a => a.Schedule)
                    .AsNoTracking()
                    .ToListAsync();

                var employeeIds = records.Select(r => r.EmployeeID).Distinct().ToList();

                var employees = await _context.Employees
                    .Where(e => employeeIds.Contains(e.EmployeeID))
                    .Include(e => e.Position)
                    .ThenInclude(p => p!.Department)
                    .AsNoTracking()
                    .ToDictionaryAsync(e => e.EmployeeID);

                var attendanceList = new List<AttendanceRecordDto>();

                foreach (var record in records)
                {
                    if (!employees.TryGetValue(record.EmployeeID, out var employee))
                        continue;

                    var position = employee.Position;
                    var department = position?.Department;

                    if (departmentId.HasValue && position?.DepartmentID != departmentId)
                        continue;

                    // Calculate IsLate dynamically
                    var isLate = false;
                    if (record.ActualCheckIn != null && record.Schedule != null)
                    {
                        var graceTime = record.Schedule.ScheduledStart.Add(TimeSpan.FromMinutes(15));
                        isLate = record.ActualCheckIn.Value.TimeOfDay > graceTime;
                    }

                    attendanceList.Add(new AttendanceRecordDto
                    {
                        AttendanceID = record.AttendanceID,
                        EmployeeID = record.EmployeeID,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Department = department?.DepartmentName ?? "-",
                        Position = position?.PositionTitle ?? "-",
                        AttendanceDate = record.AttendanceDate,
                        ScheduleStart = record.Schedule?.ScheduledStart,
                        ScheduleEnd = record.Schedule?.ScheduledEnd,
                        ActualCheckIn = record.ActualCheckIn,
                        ActualCheckOut = record.ActualCheckOut,
                        RegularHours = record.RegularHours,
                        OvertimeHours = record.OvertimeHours,
                        AttendanceStatus = record.AttendanceStatus,
                        IsLate = isLate,
                        Notes = record.Notes,
                        HoursWorked = record.ActualCheckIn.HasValue && record.ActualCheckOut.HasValue
                            ? (decimal)(record.ActualCheckOut.Value - record.ActualCheckIn.Value).TotalHours
                            : 0
                    });
                }

                return attendanceList.OrderBy(a => a.FirstName).ThenBy(a => a.LastName).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting attendance records: {ex.Message}");
                return new List<AttendanceRecordDto>();
            }
        }

        /// <summary>
        /// Records employee check-in
        /// </summary>
        public async Task<bool> RecordCheckInAsync(int employeeID, DateTime attendanceDate, int? scheduleId)
        {
            try
            {
                var attendance = await _context.Attendances
                    .FirstOrDefaultAsync(a => a.EmployeeID == employeeID && a.AttendanceDate.Date == attendanceDate.Date);

                var checkInTime = DateTime.Now;

                if (attendance == null)
                {
                    var newAttendance = new Attendance
                    {
                        EmployeeID = employeeID,
                        ScheduleID = scheduleId,
                        AttendanceDate = attendanceDate,
                        ActualCheckIn = checkInTime,
                        AttendanceStatus = "Present",
                        RegularHours = 0,
                        OvertimeHours = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.Attendances.Add(newAttendance);
                }
                else
                {
                    attendance.ActualCheckIn = checkInTime;
                    attendance.AttendanceStatus = "Present";
                    attendance.UpdatedAt = DateTime.UtcNow;
                    _context.Attendances.Update(attendance);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recording check-in: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Records employee check-out and calculates hours
        /// </summary>
        public async Task<bool> RecordCheckOutAsync(int employeeID, DateTime attendanceDate)
        {
            try
            {
                var attendance = await _context.Attendances
                    .FirstOrDefaultAsync(a => a.EmployeeID == employeeID && a.AttendanceDate.Date == attendanceDate.Date);

                if (attendance == null || !attendance.ActualCheckIn.HasValue)
                {
                    return false;
                }

                var checkOutTime = DateTime.Now;
                var totalHours = (decimal)(checkOutTime - attendance.ActualCheckIn.Value).TotalHours;

                if (totalHours <= 8)
                {
                    attendance.RegularHours = totalHours;
                    attendance.OvertimeHours = 0;
                }
                else
                {
                    attendance.RegularHours = 8;
                    attendance.OvertimeHours = totalHours - 8;
                }

                attendance.ActualCheckOut = checkOutTime;
                attendance.UpdatedAt = DateTime.UtcNow;
                _context.Attendances.Update(attendance);

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recording check-out: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets attendance history for an employee
        /// </summary>
        public async Task<List<AttendanceRecordDto>> GetEmployeeAttendanceHistoryAsync(int employeeID, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                fromDate ??= DateTime.Now.AddDays(-30);
                toDate ??= DateTime.Now;

                var records = await _context.Attendances
                    .Where(a => a.EmployeeID == employeeID && a.AttendanceDate >= fromDate && a.AttendanceDate <= toDate)
                    .Include(a => a.Schedule)
                    .AsNoTracking()
                    .OrderByDescending(a => a.AttendanceDate)
                    .ToListAsync();

                return records.Select(a => {
                    // Calculate IsLate dynamically
                    var isLate = false;
                    if (a.ActualCheckIn != null && a.Schedule != null)
                    {
                        var graceTime = a.Schedule.ScheduledStart.Add(TimeSpan.FromMinutes(15));
                        isLate = a.ActualCheckIn.Value.TimeOfDay > graceTime;
                    }

                    return new AttendanceRecordDto
                    {
                        AttendanceID = a.AttendanceID,
                        EmployeeID = a.EmployeeID,
                        AttendanceDate = a.AttendanceDate,
                        ScheduleStart = a.Schedule?.ScheduledStart,
                        ScheduleEnd = a.Schedule?.ScheduledEnd,
                        ActualCheckIn = a.ActualCheckIn,
                        ActualCheckOut = a.ActualCheckOut,
                        RegularHours = a.RegularHours,
                        OvertimeHours = a.OvertimeHours,
                        AttendanceStatus = a.AttendanceStatus,
                        IsLate = isLate,
                        Notes = a.Notes,
                        HoursWorked = a.ActualCheckIn.HasValue && a.ActualCheckOut.HasValue
                            ? (decimal)(a.ActualCheckOut.Value - a.ActualCheckIn.Value).TotalHours
                            : 0
                    };
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting attendance history: {ex.Message}");
                return new List<AttendanceRecordDto>();
            }
        }

        /// <summary>
        /// Creates a new schedule for an employee on a specific date
        /// </summary>
        public async Task<bool> CreateScheduleAsync(int employeeId, DateTime scheduleDate, TimeSpan startTime, TimeSpan endTime, string? notes = null)
        {
            try
            {
                var existingSchedule = await _context.Schedules
                    .FirstOrDefaultAsync(s => s.EmployeeID == employeeId && s.ScheduleDate.Date == scheduleDate.Date);

                if (existingSchedule != null)
                {
                    existingSchedule.ScheduledStart = startTime;
                    existingSchedule.ScheduledEnd = endTime;
                    existingSchedule.Notes = notes;
                    existingSchedule.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    var newSchedule = new Schedule
                    {
                        EmployeeID = employeeId,
                        ScheduleDate = scheduleDate.Date,
                        ScheduledStart = startTime,
                        ScheduledEnd = endTime,
                        IsActive = true,
                        Notes = notes,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _context.Schedules.AddAsync(newSchedule);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating schedule: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets attendance summary for a specific date
        /// </summary>
        public async Task<AttendanceSummaryDto> GetAttendanceSummaryAsync(DateTime attendanceDate)
        {
            try
            {
                var records = await _context.Attendances
                    .Where(a => a.AttendanceDate.Date == attendanceDate.Date)
                    .Include(a => a.Schedule)
                    .AsNoTracking()
                    .ToListAsync();

                var totalActiveEmployees = await _context.Employees
                    .CountAsync(e => e.Status == "Active");

                var presentCount = records.Count(a => a.AttendanceStatus == "Present");

                // Calculate late count dynamically
                var lateCount = records.Count(a => 
                    a.ActualCheckIn != null && 
                    a.Schedule != null && 
                    a.ActualCheckIn.Value.TimeOfDay > a.Schedule.ScheduledStart.Add(TimeSpan.FromMinutes(15)));

                return new AttendanceSummaryDto
                {
                    PresentCount = presentCount,
                    AbsentCount = totalActiveEmployees - presentCount - records.Count(a => a.AttendanceStatus == "On Leave"),
                    OnLeaveCount = records.Count(a => a.AttendanceStatus == "On Leave"),
                    LateCount = lateCount,
                    TotalRecords = totalActiveEmployees
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting attendance summary: {ex.Message}");
                return new AttendanceSummaryDto();
            }
        }
    }

    // DTOs for data transfer
    public class AttendanceRecordDto
    {
        public int AttendanceID { get; set; }
        public int EmployeeID { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Department { get; set; } = "";
        public string Position { get; set; } = "";
        public DateTime AttendanceDate { get; set; }
        public TimeSpan? ScheduleStart { get; set; }
        public TimeSpan? ScheduleEnd { get; set; }
        public DateTime? ActualCheckIn { get; set; }
        public DateTime? ActualCheckOut { get; set; }
        public decimal RegularHours { get; set; }
        public decimal OvertimeHours { get; set; }
        public string AttendanceStatus { get; set; } = "";
        public bool IsLate { get; set; }
        public string? Notes { get; set; }
        public decimal HoursWorked { get; set; }
    }

    public class EmployeeWithScheduleDto
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Department { get; set; } = "";
        public string Position { get; set; } = "";
        public int? ScheduleID { get; set; }
        public TimeSpan? ScheduleStart { get; set; }
        public TimeSpan? ScheduleEnd { get; set; }
        public int? AttendanceID { get; set; }
        public DateTime? ActualCheckIn { get; set; }
        public DateTime? ActualCheckOut { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string AttendanceStatus { get; set; } = "Not Scheduled";
        public bool IsLate { get; set; }
        public decimal RegularHours { get; set; }
        public decimal OvertimeHours { get; set; }
        public string? Notes { get; set; }
    }

    public class EmployeeDto
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Department { get; set; } = "";
        public string Position { get; set; } = "";
        public string Status { get; set; } = "";
    }

    public class AttendanceSummaryDto
    {
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int OnLeaveCount { get; set; }
        public int LateCount { get; set; }
        public int TotalRecords { get; set; }
        public int NotScheduledCount { get; set; }
    }
}
