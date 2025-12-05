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
        /// Gets all active employees with their attendance status for a specific date
        /// This shows ALL employees, including those who haven't checked in yet
        /// </summary>
        public async Task<List<AttendanceRecordDto>> GetAllEmployeesWithAttendanceAsync(DateTime attendanceDate, int? departmentId = null)
        {
            try
            {
                // Get all active employees with Position and Department
                var employees = await _context.Employees
                    .Where(e => e.Status == "Active")
                    .Include(e => e.Position)
                    .ThenInclude(p => p!.Department)
                    .AsNoTracking()
                    .ToListAsync();

                // Get attendance records for the selected date
                var attendanceRecords = await _context.Attendances
                    .Where(a => a.AttendanceDate.Date == attendanceDate.Date)
                    .AsNoTracking()
                    .ToListAsync();

                var attendanceDict = attendanceRecords.ToDictionary(a => a.EmployeeID);

                var attendanceList = new List<AttendanceRecordDto>();

                foreach (var employee in employees)
                {
                    var position = employee.Position;
                    var department = position?.Department;

                    // Filter by department if specified
                    if (departmentId.HasValue && position?.DepartmentID != departmentId)
                        continue;

                    // Check if employee has attendance record for this date
                    attendanceDict.TryGetValue(employee.EmployeeID, out var attendance);

                    attendanceList.Add(new AttendanceRecordDto
                    {
                        AttendanceID = attendance?.AttendanceID ?? 0,
                        EmployeeID = employee.EmployeeID,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Department = department?.DepartmentName ?? "-",
                        Position = position?.PositionTitle ?? "-",
                        AttendanceDate = attendanceDate,
                        ScheduleStart = attendance?.ScheduleStart,
                        ScheduleEnd = attendance?.ScheduleEnd,
                        ActualCheckIn = attendance?.ActualCheckIn,
                        ActualCheckOut = attendance?.ActualCheckOut,
                        RegularHours = attendance?.RegularHours ?? 0,
                        OvertimeHours = attendance?.OvertimeHours ?? 0,
                        AttendanceStatus = attendance?.AttendanceStatus ?? "Not Checked In",
                        IsLate = attendance?.IsLate ?? false,
                        Notes = attendance?.Notes,
                        HoursWorked = attendance?.ActualCheckIn != null && attendance?.ActualCheckOut != null
                            ? (decimal)(attendance.ActualCheckOut.Value - attendance.ActualCheckIn.Value).TotalHours
                            : 0
                    });
                }

                return attendanceList.OrderBy(a => a.FirstName).ThenBy(a => a.LastName).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting employees with attendance: {ex.Message}");
                return new List<AttendanceRecordDto>();
            }
        }

        /// <summary>
        /// Gets attendance records for a specific date with employee details (only employees with records)
        /// </summary>
        public async Task<List<AttendanceRecordDto>> GetAttendanceByDateAsync(DateTime attendanceDate, int? departmentId = null)
        {
            try
            {
                var records = await _context.Attendances
                    .Where(a => a.AttendanceDate.Date == attendanceDate.Date)
                    .AsNoTracking()
                    .ToListAsync();

                // Get employee IDs from attendance records
                var employeeIds = records.Select(r => r.EmployeeID).Distinct().ToList();

                // Load employees with Position and Department
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

                    // Filter by department if specified
                    if (departmentId.HasValue && position?.DepartmentID != departmentId)
                        continue;

                    attendanceList.Add(new AttendanceRecordDto
                    {
                        AttendanceID = record.AttendanceID,
                        EmployeeID = record.EmployeeID,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Department = department?.DepartmentName ?? "-",
                        Position = position?.PositionTitle ?? "-",
                        AttendanceDate = record.AttendanceDate,
                        ScheduleStart = record.ScheduleStart,
                        ScheduleEnd = record.ScheduleEnd,
                        ActualCheckIn = record.ActualCheckIn,
                        ActualCheckOut = record.ActualCheckOut,
                        RegularHours = record.RegularHours,
                        OvertimeHours = record.OvertimeHours,
                        AttendanceStatus = record.AttendanceStatus,
                        IsLate = record.IsLate,
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
        /// Gets all active employees with Position and Department
        /// </summary>
        public async Task<List<EmployeeDto>> GetActiveEmployeesAsync(int? departmentId = null)
        {
            try
            {
                var employees = await _context.Employees
                    .Where(e => e.Status == "Active")
                    .Include(e => e.Position)
                    .ThenInclude(p => p!.Department)
                    .AsNoTracking()
                    .ToListAsync();

                var employeeList = new List<EmployeeDto>();

                foreach (var employee in employees)
                {
                    var position = employee.Position;
                    var department = position?.Department;

                    // Filter by department if specified
                    if (departmentId.HasValue && position?.DepartmentID != departmentId)
                        continue;

                    employeeList.Add(new EmployeeDto
                    {
                        EmployeeID = employee.EmployeeID,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Department = department?.DepartmentName ?? "-",
                        Position = position?.PositionTitle ?? "-",
                        Status = employee.Status
                    });
                }

                return employeeList.OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting employees: {ex.Message}");
                return new List<EmployeeDto>();
            }
        }

        /// <summary>
        /// Records employee check-in
        /// </summary>
        public async Task<bool> RecordCheckInAsync(int employeeID, DateTime attendanceDate, TimeSpan? scheduleStart)
        {
            try
            {
                var attendance = await _context.Attendances
                    .FirstOrDefaultAsync(a => a.EmployeeID == employeeID && a.AttendanceDate.Date == attendanceDate.Date);

                var checkInTime = DateTime.Now;
                var isLate = false;

                // Check if late (more than 5 minutes after scheduled start)
                if (scheduleStart.HasValue)
                {
                    var scheduledStartTime = attendanceDate.Date.Add(scheduleStart.Value);
                    isLate = checkInTime > scheduledStartTime.AddMinutes(5);
                }

                if (attendance == null)
                {
                    // Create new attendance record
                    var newAttendance = new Attendance
                    {
                        EmployeeID = employeeID,
                        AttendanceDate = attendanceDate,
                        ScheduleStart = scheduleStart,
                        ActualCheckIn = checkInTime,
                        AttendanceStatus = "Present",
                        IsLate = isLate,
                        RegularHours = 0,
                        OvertimeHours = 0,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.Attendances.Add(newAttendance);
                }
                else
                {
                    // Update existing record
                    attendance.ActualCheckIn = checkInTime;
                    attendance.AttendanceStatus = "Present";
                    attendance.IsLate = isLate;
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

                // Separate regular and overtime (8 hours regular)
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
                    .AsNoTracking()
                    .OrderByDescending(a => a.AttendanceDate)
                    .ToListAsync();

                return records.Select(a => new AttendanceRecordDto
                {
                    AttendanceID = a.AttendanceID,
                    EmployeeID = a.EmployeeID,
                    AttendanceDate = a.AttendanceDate,
                    ScheduleStart = a.ScheduleStart,
                    ScheduleEnd = a.ScheduleEnd,
                    ActualCheckIn = a.ActualCheckIn,
                    ActualCheckOut = a.ActualCheckOut,
                    RegularHours = a.RegularHours,
                    OvertimeHours = a.OvertimeHours,
                    AttendanceStatus = a.AttendanceStatus,
                    IsLate = a.IsLate,
                    Notes = a.Notes,
                    HoursWorked = a.ActualCheckIn.HasValue && a.ActualCheckOut.HasValue
                        ? (decimal)(a.ActualCheckOut.Value - a.ActualCheckIn.Value).TotalHours
                        : 0
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting attendance history: {ex.Message}");
                return new List<AttendanceRecordDto>();
            }
        }

        /// <summary>
        /// Updates schedule start and end times for an attendance record
        /// </summary>
        public async Task<bool> UpdateScheduleAsync(int attendanceID, TimeSpan scheduleStart, TimeSpan scheduleEnd)
        {
            try
            {
                var attendance = await _context.Attendances.FindAsync(attendanceID);
                if (attendance == null) return false;

                attendance.ScheduleStart = scheduleStart;
                attendance.ScheduleEnd = scheduleEnd;
                attendance.UpdatedAt = DateTime.UtcNow;

                _context.Attendances.Update(attendance);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating schedule: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Creates a new attendance record with shift assignment
        /// </summary>
        public async Task<bool> CreateAttendanceWithShiftAsync(
            int employeeID,
            DateTime attendanceDate,
            TimeSpan scheduleStart,
            TimeSpan scheduleEnd,
            string? notes = null)
        {
            try
            {
                Console.WriteLine($"=== CreateAttendanceWithShiftAsync START ===");
                Console.WriteLine($"EmployeeID: {employeeID}");
                Console.WriteLine($"AttendanceDate: {attendanceDate:yyyy-MM-dd}");
                Console.WriteLine($"ScheduleStart: {scheduleStart}");
                Console.WriteLine($"ScheduleEnd: {scheduleEnd}");
                Console.WriteLine($"Notes: {notes ?? "(null)"}");
                
                // Check if employee exists
                var employeeExists = await _context.Employees.AnyAsync(e => e.EmployeeID == employeeID);
                if (!employeeExists)
                {
                    Console.WriteLine($"ERROR: Employee with ID {employeeID} does not exist!");
                    return false;
                }
                Console.WriteLine($"Employee {employeeID} verified.");
                
                // Check if record already exists for this employee and date
                var existingRecord = await _context.Attendances
                    .FirstOrDefaultAsync(a => a.EmployeeID == employeeID && a.AttendanceDate.Date == attendanceDate.Date);

                if (existingRecord != null)
                {
                    Console.WriteLine($"Found existing record ID: {existingRecord.AttendanceID}, updating...");
                    // Update existing record with shift info
                    existingRecord.ScheduleStart = scheduleStart;
                    existingRecord.ScheduleEnd = scheduleEnd;
                    existingRecord.Notes = notes;
                    existingRecord.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    Console.WriteLine("No existing record found, creating new attendance...");
                    // Create new attendance record
                    var newAttendance = new Attendance
                    {
                        EmployeeID = employeeID,
                        AttendanceDate = attendanceDate.Date,
                        ScheduleStart = scheduleStart,
                        ScheduleEnd = scheduleEnd,
                        AttendanceStatus = "Pending",
                        IsLate = false,
                        RegularHours = 0,
                        OvertimeHours = 0,
                        Notes = notes,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    
                    Console.WriteLine($"Adding new attendance record...");
                    await _context.Attendances.AddAsync(newAttendance);
                }

                Console.WriteLine("Calling SaveChangesAsync...");
                var changes = await _context.SaveChangesAsync();
                Console.WriteLine($"SaveChangesAsync completed. Rows affected: {changes}");
                
                // Return true even if changes is 0 (might happen if no actual changes were made)
                Console.WriteLine("=== CreateAttendanceWithShiftAsync SUCCESS ===");
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"DATABASE ERROR in CreateAttendanceWithShiftAsync:");
                Console.WriteLine($"Message: {dbEx.Message}");
                Console.WriteLine($"Inner Exception: {dbEx.InnerException?.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR in CreateAttendanceWithShiftAsync: {ex.Message}");
                Console.WriteLine($"Exception Type: {ex.GetType().Name}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Updates a complete attendance record including schedule, check in/out, status, and notes
        /// </summary>
        public async Task<bool> UpdateAttendanceRecordAsync(
            int attendanceID,
            TimeSpan scheduleStart,
            TimeSpan scheduleEnd,
            DateTime? actualCheckIn,
            DateTime? actualCheckOut,
            string attendanceStatus,
            bool isLate,
            string? notes)
        {
            try
            {
                Console.WriteLine($"=== UpdateAttendanceRecordAsync START ===");
                Console.WriteLine($"AttendanceID: {attendanceID}");
                Console.WriteLine($"ScheduleStart: {scheduleStart}");
                Console.WriteLine($"ScheduleEnd: {scheduleEnd}");
                Console.WriteLine($"ActualCheckIn: {actualCheckIn}");
                Console.WriteLine($"ActualCheckOut: {actualCheckOut}");
                Console.WriteLine($"Status: {attendanceStatus}");
                Console.WriteLine($"IsLate: {isLate}");
                Console.WriteLine($"Notes: {notes ?? "(null)"}");
                
                var attendance = await _context.Attendances.FindAsync(attendanceID);
                if (attendance == null)
                {
                    Console.WriteLine($"ERROR: Attendance record with ID {attendanceID} not found!");
                    return false;
                }
                
                Console.WriteLine($"Found attendance record for Employee: {attendance.EmployeeID}, Date: {attendance.AttendanceDate:yyyy-MM-dd}");

                // Update schedule
                attendance.ScheduleStart = scheduleStart;
                attendance.ScheduleEnd = scheduleEnd;
                
                // Update actual times
                attendance.ActualCheckIn = actualCheckIn;
                attendance.ActualCheckOut = actualCheckOut;
                
                // Calculate hours if both check in and out are set
                if (actualCheckIn.HasValue && actualCheckOut.HasValue)
                {
                    var totalHours = (decimal)(actualCheckOut.Value - actualCheckIn.Value).TotalHours;
                    if (totalHours < 0)
                    {
                        // Handle overnight shifts
                        totalHours += 24;
                    }
                    
                    Console.WriteLine($"Total hours calculated: {totalHours:F2}");
                    
                    // Separate regular and overtime (8 hours regular)
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
                    
                    Console.WriteLine($"RegularHours: {attendance.RegularHours:F2}, OvertimeHours: {attendance.OvertimeHours:F2}");
                }
                else
                {
                    // Keep existing hours if no check in/out provided
                    Console.WriteLine("No check in/out times - keeping existing hours");
                }
                
                // Update status and flags
                attendance.AttendanceStatus = attendanceStatus;
                attendance.IsLate = isLate;
                attendance.Notes = notes;
                attendance.UpdatedAt = DateTime.UtcNow;

                Console.WriteLine("Calling SaveChangesAsync...");
                var changes = await _context.SaveChangesAsync();
                Console.WriteLine($"SaveChangesAsync completed. Rows affected: {changes}");
                
                Console.WriteLine("=== UpdateAttendanceRecordAsync SUCCESS ===");
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"DATABASE ERROR in UpdateAttendanceRecordAsync:");
                Console.WriteLine($"Message: {dbEx.Message}");
                Console.WriteLine($"Inner Exception: {dbEx.InnerException?.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR in UpdateAttendanceRecordAsync: {ex.Message}");
                Console.WriteLine($"Exception Type: {ex.GetType().Name}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
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
                    .AsNoTracking()
                    .ToListAsync();

                var totalActiveEmployees = await _context.Employees
                    .CountAsync(e => e.Status == "Active");

                var presentCount = records.Count(a => a.AttendanceStatus == "Present");

                return new AttendanceSummaryDto
                {
                    PresentCount = presentCount,
                    AbsentCount = totalActiveEmployees - presentCount - records.Count(a => a.AttendanceStatus == "On Leave"),
                    OnLeaveCount = records.Count(a => a.AttendanceStatus == "On Leave"),
                    LateCount = records.Count(a => a.IsLate),
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
    }
}
