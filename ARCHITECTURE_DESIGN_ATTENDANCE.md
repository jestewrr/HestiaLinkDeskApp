# Architecture & Design: Attendance Record Management

## System Overview

```
???????????????????????????????????????????????????????????????????
?                         UI Layer (Blazor)                       ?
?                    Attendance.razor Component                    ?
???????????????????????????????????????????????????????????????????
?  • OpenAssignShiftModal()     ? Checks AttendanceID > 0?        ?
?  • OpenEditShiftModal()       ? Opens in EDIT mode              ?
?  • SaveScheduleChanges()      ? Double-checks existing record   ?
????????????????????????????????????????????????????????????????????
                 ?                                ?
         CREATE NEW                       UPDATE EXISTING
         (AttendanceID=0)                 (AttendanceID>0)
                 ?                                ?
                 ?                                ?
????????????????????????????????????????????????????????????????????
?                      Service Layer (C#)                          ?
?                   AttendanceService Methods                       ?
????????????????????????????????????????????????????????????????????
?  • CreateAttendanceWithShiftAsync()                              ?
?    - Checks: FirstOrDefaultAsync(EmployeeID, Date)              ?
?    - If EXISTS: Update existing                                 ?
?    - If NOT: Create new                                         ?
?                                                                  ?
?  • UpdateAttendanceRecordAsync()                                ?
?    - Update only (never creates)                                ?
?    - Validates TimeSpan values                                  ?
????????????????????????????????????????????????????????????????????
                 ?                                ?
              EF Core                         EF Core
                 ?                                ?
                 ?                                ?
????????????????????????????????????????????????????????????????????
?                      Data Layer (SQL)                            ?
?                     Attendance Table                             ?
????????????????????????????????????????????????????????????????????
?  Constraint: UNIQUE(EmployeeID, AttendanceDate)                 ?
?  • ONE record per employee per day                              ?
?  • Prevents duplicates at database level                        ?
????????????????????????????????????????????????????????????????????
```

## Flow Diagrams

### Flow 1: First Time Assignment (New Record)

```
User clicks "?? Assign Shift"
          ?
OpenAssignShiftModal() checks: record.AttendanceID > 0?
          ?
        NO (equals 0)
          ?
Modal opens: "Assign Shift"
          ?
User selects: Day Shift (5 AM - 5 PM)
          ?
User clicks: "?? Save Changes"
          ?
SaveScheduleChanges()
    ?
    Check: Does record exist in DB?
          ?
        NO
          ?
    Call: CreateAttendanceWithShiftAsync()
          ?
          Check: FirstOrDefaultAsync(EmployeeID=2, Date=2025-12-07)
                  ?
                NO ? Create new record
                  ?
          INSERT INTO Attendance (EmployeeID=2, Date=2025-12-07, ...)
          ?
? Success: Record created with ID=5
```

### Flow 2: Editing Existing Record (Same Day)

```
User navigates to same employee, same date
          ?
Attendance table shows: Employee with existing shift
          ?
Shift column shows: "?? Day Shift"
          ?
User clicks: "?? Edit Shift"
          ?
OpenEditShiftModal() is called directly
          ?
Modal opens: "Edit Attendance"
          ?
Fields pre-populated with existing values
          ?
User changes: Day Shift ? Night Shift (5 PM - 5 AM)
          ?
User clicks: "?? Save Changes"
          ?
SaveScheduleChanges()
    ?
    Check: Does record exist in DB?
          ?
        YES (AttendanceID=5)
          ?
    Call: UpdateAttendanceRecordAsync(AttendanceID=5, ...)
          ?
          UPDATE Attendance SET ScheduleStart=17:00, ScheduleEnd=05:00, ... WHERE AttendanceID=5
          ?
? Success: Record updated, no duplicate created
```

### Flow 3: Prevention of Duplicate Creation (The Fix)

```
User clicks "?? Assign Shift" on same employee (has record)
          ?
OpenAssignShiftModal() checks: record.AttendanceID > 0?
          ?
        YES (equals 5)
          ?
System says: "Record exists! Redirect to EDIT mode"
          ?
Automatically calls: OpenEditShiftModal(record)
          ?
Modal opens: "Edit Attendance"
          ?
? PREVENTS: User would try to create duplicate
? ENSURES: User edits existing record
```

## Data State Examples

### Example 1: New Employee - No Record Yet

| EmployeeID | AttendanceID | Status | ScheduleStart |
|------------|-------------|--------|---------------|
| 2          | 0           | Pending| NULL          |

**Action**: User clicks "?? Assign Shift"
- Modal: "Assign Shift"
- Button: "?? Save Changes" (enabled)

### Example 2: After First Assignment

| EmployeeID | AttendanceID | Status | ScheduleStart |
|------------|-------------|--------|---------------|
| 2          | 5           | Pending| 05:00:00      |

**Action**: User clicks "?? Edit Shift"
- Modal: "Edit Attendance"
- AttendanceID: 5 (pre-filled)
- ScheduleStart: 05:00:00 (pre-filled)

### Example 3: After Editing (Day ? Night)

| EmployeeID | AttendanceID | Status | ScheduleStart |
|------------|-------------|--------|---------------|
| 2          | 5           | Pending| 17:00:00      |

**State**: Still ONE record (ID=5), just updated
- No duplicate created
- No UNIQUE KEY violation

## Three-Layer Validation

### Layer 1: UI Logic (Blazor Component)
```csharp
// OpenAssignShiftModal()
if (record.AttendanceID > 0)  // ? Check here
{
    OpenEditShiftModal(record);  // Redirect to edit
    return;
}
```
? Prevents wrong modal from opening

### Layer 2: Save Logic (Component)
```csharp
// SaveScheduleChanges()
var existingRecord = await DbContext.Attendances
    .FirstOrDefaultAsync(a => a.EmployeeID == ... && 
                             a.AttendanceDate.Date == ...);
if (existingRecord != null)  // ? Check here
{
    IsNewAttendance = false;
    SelectedAttendanceForEdit.AttendanceID = existingRecord.AttendanceID;
}
```
? Double-checks before save

### Layer 3: Service Logic (AttendanceService)
```csharp
// CreateAttendanceWithShiftAsync()
var existing = await _context.Attendances
    .FirstOrDefaultAsync(a => a.EmployeeID == ... && 
                             a.AttendanceDate.Date == ...);
if (existing != null)  // ? Final check here
{
    // Update instead of create
    existing.ScheduleStart = scheduleStart;
    existing.UpdatedAt = DateTime.UtcNow;
}
else
{
    // Create new
}
```
? Last line of defense

### Layer 4: Database Constraint (SQL)
```sql
CREATE UNIQUE INDEX IX_Attendance_EmployeeID_AttendanceDate
ON Attendance(EmployeeID, AttendanceDate);
```
? Absolute protection against duplicates

## Key Principles

### 1. Single Record Per Employee Per Day
- Business Rule: One schedule per employee per day
- Enforced by: UNIQUE constraint
- Validated by: Service + UI logic

### 2. Idempotent Operations
- Create is idempotent: Calling twice = one record (second updates)
- Update is idempotent: Calling twice = same result
- Safe to retry if network fails

### 3. Defense in Depth
- UI prevents wrong mode
- Component double-checks
- Service triple-checks
- Database enforces via constraint

## Performance Considerations

### Query Optimization
```csharp
// FirstOrDefaultAsync performs ONE query to DB
var existing = await _context.Attendances
    .FirstOrDefaultAsync(a => 
        a.EmployeeID == employeeID && 
        a.AttendanceDate.Date == attendanceDate.Date);
```
- Index on (EmployeeID, AttendanceDate) makes this fast
- Returns immediately when found
- No N+1 queries

### Update vs Create Performance
- **Create**: Insert new row (~2-5ms)
- **Update**: Modify existing row (~1-2ms)
- Both are instant for user

## Error Scenarios & Recovery

### Scenario: Network Fails During Save
1. User clicks "?? Save Changes"
2. Network disconnects
3. Request doesn't reach server
4. Modal stays open
5. User retries (service checks for existing)
6. **Result**: Record created or updated safely (no duplicate)

### Scenario: Concurrent Edit by Two Users
1. User A opens modal for Employee 2
2. User B opens modal for same Employee 2
3. User A saves first (record created/updated)
4. User B saves after
5. **Result**: User B's update overwrites User A's (acceptable for attendance)

### Scenario: User Navigates Away During Save
1. Modal is closing
2. Save is in progress
3. User navigates to another page
4. **Result**: Save completes in background, next load shows correct data

## Testing Strategy

| Test Type | What's Tested | How |
|-----------|---------------|-----|
| Unit | Service logic | Direct method calls |
| Integration | UI + Service | User interactions |
| Database | Constraint | SQL queries |
| Regression | Existing features | Full workflow |

## Migration Path (If Database Grows)

For large attendance tables:
1. Add composite index: `CREATE INDEX idx_emp_date ON Attendance(EmployeeID, AttendanceDate)`
2. Partition table by year: `Attendance_2024, Attendance_2025, ...`
3. Archive old records: Move completed years to archive table

## Summary

? **Before Fix**: Risk of UNIQUE KEY violation
? **After Fix**: Multiple layers prevent any duplicate creation
? **User Experience**: Seamless switching between assign/edit modes
? **Data Integrity**: Guaranteed one record per employee per day
? **Performance**: Instant operations with indexed queries
