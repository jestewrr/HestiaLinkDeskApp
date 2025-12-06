-- ===================================================================
-- Attendance Table Creation Script
-- Purpose: Track employee attendance, check-ins, and scheduled hours
-- ===================================================================

IF OBJECT_ID('dbo.Attendance', 'U') IS NOT NULL
    DROP TABLE dbo.Attendance;
GO

CREATE TABLE dbo.Attendance (
    AttendanceID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeID INT NOT NULL,
    AttendanceDate DATE NOT NULL,
    ScheduleStart TIME NULL,                    -- Scheduled start time
    ScheduleEnd TIME NULL,                      -- Scheduled end time
    ActualCheckIn DATETIME2 NULL,               -- Actual check-in timestamp
    ActualCheckOut DATETIME2 NULL,              -- Actual check-out timestamp
    RegularHours DECIMAL(5,2) DEFAULT 0,        -- Regular hours worked (8 hours max)
    OvertimeHours DECIMAL(5,2) DEFAULT 0,       -- Overtime hours
    AttendanceStatus NVARCHAR(20) DEFAULT 'Absent', -- Present, Absent, On Leave, Half Day
    IsLate BIT DEFAULT 0,                       -- Flag if check-in was late
    Notes NVARCHAR(500) NULL,                   -- Additional notes
    CreatedAt DATETIME2 DEFAULT GETUTCNOW(),
    UpdatedAt DATETIME2 DEFAULT GETUTCNOW(),
    
    -- Foreign key constraint
    CONSTRAINT FK_Attendance_Employee FOREIGN KEY (EmployeeID)
        REFERENCES Employee(EmployeeID)
        ON DELETE CASCADE,
    
    -- Unique constraint to prevent duplicate attendance records per day
    CONSTRAINT UQ_Attendance_EmployeeDate UNIQUE (EmployeeID, AttendanceDate),
    
    -- Index for common queries
    INDEX IX_Attendance_Date ON Attendance(AttendanceDate),
    INDEX IX_Attendance_EmployeeDate ON Attendance(EmployeeID, AttendanceDate),
    INDEX IX_Attendance_Status ON Attendance(AttendanceStatus)
);

GO

-- ===================================================================
-- View: vw_AttendanceSummary
-- Purpose: Provides daily attendance summary by status
-- ===================================================================

IF OBJECT_ID('dbo.vw_AttendanceSummary', 'V') IS NOT NULL
    DROP VIEW dbo.vw_AttendanceSummary;
GO

CREATE VIEW dbo.vw_AttendanceSummary AS
SELECT
    CAST(a.AttendanceDate AS DATE) AS AttendanceDate,
    SUM(CASE WHEN a.AttendanceStatus = 'Present' THEN 1 ELSE 0 END) AS PresentCount,
    SUM(CASE WHEN a.AttendanceStatus = 'Absent' THEN 1 ELSE 0 END) AS AbsentCount,
    SUM(CASE WHEN a.AttendanceStatus = 'On Leave' THEN 1 ELSE 0 END) AS OnLeaveCount,
    SUM(CASE WHEN a.IsLate = 1 THEN 1 ELSE 0 END) AS LateCount,
    COUNT(*) AS TotalRecords
FROM Attendance a
GROUP BY CAST(a.AttendanceDate AS DATE);

GO

-- ===================================================================
-- Stored Procedure: sp_GetAttendanceByDate
-- Purpose: Fetch attendance records with employee details for a given date
-- ===================================================================

IF OBJECT_ID('dbo.sp_GetAttendanceByDate', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetAttendanceByDate;
GO

CREATE PROCEDURE sp_GetAttendanceByDate
    @AttendanceDate DATE,
    @DepartmentID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT
        a.AttendanceID,
        a.EmployeeID,
        e.FirstName,
        e.LastName,
        e.EmployeeID AS EmpID,
        e.DepartmentID,
        d.DepartmentName,
        p.PositionTitle,
        e.Status AS EmployeeStatus,
        a.AttendanceDate,
        a.ScheduleStart,
        a.ScheduleEnd,
        a.ActualCheckIn,
        a.ActualCheckOut,
        a.RegularHours,
        a.OvertimeHours,
        a.AttendanceStatus,
        a.IsLate,
        a.Notes,
        DATEDIFF(HOUR, a.ActualCheckIn, a.ActualCheckOut) AS HoursWorked
    FROM Attendance a
    INNER JOIN Employee e ON a.EmployeeID = e.EmployeeID
    LEFT JOIN Department d ON e.DepartmentID = d.DepartmentID
    LEFT JOIN Position p ON e.PositionID = p.PositionID
    WHERE a.AttendanceDate = @AttendanceDate
        AND (@DepartmentID IS NULL OR e.DepartmentID = @DepartmentID)
        AND e.Status = 'Active'
    ORDER BY e.FirstName, e.LastName;
END;

GO

-- ===================================================================
-- Stored Procedure: sp_RecordCheckIn
-- Purpose: Record an employee's check-in time
-- ===================================================================

IF OBJECT_ID('dbo.sp_RecordCheckIn', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_RecordCheckIn;
GO

CREATE PROCEDURE sp_RecordCheckIn
    @EmployeeID INT,
    @AttendanceDate DATE,
    @ActualCheckIn DATETIME2,
    @ScheduleStart TIME
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @IsLate BIT = 0;
    DECLARE @ExistingRecordID INT;
    
    -- Check if late (after scheduled start time by 5 minutes)
    IF CAST(@ActualCheckIn AS TIME) > DATEADD(MINUTE, 5, @ScheduleStart)
        SET @IsLate = 1;
    
    -- Check if attendance record exists
    SELECT @ExistingRecordID = AttendanceID 
    FROM Attendance 
    WHERE EmployeeID = @EmployeeID AND AttendanceDate = @AttendanceDate;
    
    IF @ExistingRecordID IS NULL
    BEGIN
        -- Create new attendance record
        INSERT INTO Attendance (EmployeeID, AttendanceDate, ScheduleStart, ActualCheckIn, AttendanceStatus, IsLate)
        VALUES (@EmployeeID, @AttendanceDate, @ScheduleStart, @ActualCheckIn, 'Present', @IsLate);
    END
    ELSE
    BEGIN
        -- Update existing record with check-in
        UPDATE Attendance
        SET 
            ActualCheckIn = @ActualCheckIn,
            IsLate = @IsLate,
            AttendanceStatus = 'Present',
            UpdatedAt = GETUTCNOW()
        WHERE AttendanceID = @ExistingRecordID;
    END
END;

GO

-- ===================================================================
-- Stored Procedure: sp_RecordCheckOut
-- Purpose: Record an employee's check-out time and calculate hours
-- ===================================================================

IF OBJECT_ID('dbo.sp_RecordCheckOut', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_RecordCheckOut;
GO

CREATE PROCEDURE sp_RecordCheckOut
    @EmployeeID INT,
    @AttendanceDate DATE,
    @ActualCheckOut DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @ActualCheckIn DATETIME2;
    DECLARE @RegularHours DECIMAL(5,2);
    DECLARE @OvertimeHours DECIMAL(5,2);
    DECLARE @TotalHours DECIMAL(5,2);
    
    -- Get check-in time
    SELECT @ActualCheckIn = ActualCheckIn 
    FROM Attendance 
    WHERE EmployeeID = @EmployeeID AND AttendanceDate = @AttendanceDate;
    
    IF @ActualCheckIn IS NOT NULL
    BEGIN
        -- Calculate total hours
        SET @TotalHours = CAST(DATEDIFF(MINUTE, @ActualCheckIn, @ActualCheckOut) AS DECIMAL) / 60;
        
        -- Separate regular and overtime (8 hours regular)
        IF @TotalHours <= 8
            SET @RegularHours = @TotalHours;
        ELSE
        BEGIN
            SET @RegularHours = 8;
            SET @OvertimeHours = @TotalHours - 8;
        END
        
        -- Update attendance record
        UPDATE Attendance
        SET 
            ActualCheckOut = @ActualCheckOut,
            RegularHours = @RegularHours,
            OvertimeHours = @OvertimeHours,
            UpdatedAt = GETUTCNOW()
        WHERE EmployeeID = @EmployeeID AND AttendanceDate = @AttendanceDate;
    END
END;

GO

-- ===================================================================
-- Stored Procedure: sp_GetEmployeeAttendanceHistory
-- Purpose: Get attendance history for an employee with filtering options
-- ===================================================================

IF OBJECT_ID('dbo.sp_GetEmployeeAttendanceHistory', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetEmployeeAttendanceHistory;
GO

CREATE PROCEDURE sp_GetEmployeeAttendanceHistory
    @EmployeeID INT,
    @FromDate DATE = NULL,
    @ToDate DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Default to last 30 days if not specified
    IF @FromDate IS NULL SET @FromDate = DATEADD(DAY, -30, CAST(GETDATE() AS DATE));
    IF @ToDate IS NULL SET @ToDate = CAST(GETDATE() AS DATE);
    
    SELECT
        a.AttendanceID,
        a.AttendanceDate,
        a.ScheduleStart,
        a.ScheduleEnd,
        a.ActualCheckIn,
        a.ActualCheckOut,
        a.RegularHours,
        a.OvertimeHours,
        a.AttendanceStatus,
        a.IsLate,
        a.Notes,
        DATEDIFF(HOUR, a.ActualCheckIn, a.ActualCheckOut) AS HoursWorked
    FROM Attendance a
    WHERE a.EmployeeID = @EmployeeID
        AND a.AttendanceDate BETWEEN @FromDate AND @ToDate
    ORDER BY a.AttendanceDate DESC;
END;

GO

PRINT 'Attendance table and related objects created successfully!';
