# Attendance Save Debugging Guide

## Issue
The "Save Changes" button in the Assign/Edit Shift modal shows error: "Failed to save shift. Check console for details."

## Solution: Step-by-Step Debugging

### Step 1: Check Console Output
1. **Open Developer Tools**: Press `F12` in your browser
2. **Go to Console tab**: Look for console.log messages from the application
3. **Check for errors**: Look for any JavaScript, network, or SQL errors

### Step 2: Test Direct Database Save
The modal now has a **"?? Test Direct Save"** button (bottom left of modal)

**What it does:**
- Creates an attendance record directly using DbContext
- Bypasses the AttendanceService layer
- Helps determine if the database connection is working

**If successful:**
- You'll get an alert: "Direct save successful! ID: [number]"
- The database connection and table are working
- The issue is likely in the AttendanceService

**If it fails:**
- You'll get an alert with the error message
- Check the console for detailed error logs
- The issue is likely a database constraint or connection problem

### Step 3: Test AttendanceService
The modal also has a **"?? Test Service"** button (bottom right of modal)

**What it does:**
- Calls `AttendanceService.CreateAttendanceWithShiftAsync()` directly
- Uses the service layer
- Helps determine if the service method is working

**If successful:**
- Result shows `true` in alert
- The service is working correctly
- The issue might be in how SaveScheduleChanges is calling it

**If it fails:**
- Result shows `false` in alert
- Check the console for service-level errors
- Look for database constraint violations

### Step 4: Check Console Logs

In the browser console, look for logs like:
```
=== CreateAttendanceWithShiftAsync ===
EmployeeID: 2
AttendanceDate: 2025-01-15
ScheduleStart: 05:00:00
ScheduleEnd: 17:00:00
Found existing record ID: 5, updating...
SaveChangesAsync completed. Rows affected: 1
=== CreateAttendanceWithShiftAsync SUCCESS ===
```

### Common Issues and Solutions

#### 1. **UNIQUE Constraint Violation**
**Error:** "Cannot insert duplicate key row in object '[IT13].[dbo].[Attendance]'"

**Cause:** A record already exists for this employee on this date

**Solution:**
- The code should detect this and UPDATE instead of INSERT
- Check if `AttendanceID` is being populated correctly
- Try clicking "Edit" instead of "Assign Shift" if the record exists

#### 2. **Foreign Key Violation**
**Error:** "The INSERT, UPDATE, or DELETE statement conflicted with a FOREIGN KEY constraint"

**Cause:** Employee ID doesn't exist in the Employee table

**Solution:**
- Verify the employee ID exists in the database
- Use the Test Direct Save button to confirm
- Check if `EditEmployeeId` is being set correctly

#### 3. **Database Connection Error**
**Error:** "Connection string validation failed" or timeout errors

**Cause:** Cannot connect to SQL Server database

**Solution:**
- Verify connection string in appsettings.json
- Ensure SQL Server is running
- Check database credentials

#### 4. **NULL Value in Required Field**
**Error:** "Cannot insert the value NULL into column '[column]'"

**Cause:** Required fields (like ScheduleStart, ScheduleEnd) are null

**Solution:**
- Ensure a shift type is selected before saving
- Verify EditScheduleStartTime and EditScheduleEndTime have values

### Step 5: Review Actual Logs

In the **Output Window** of Visual Studio (View ? Output or Ctrl+Alt+O):
1. Select "HestiaIT13Final" from the dropdown
2. Look for detailed logs from the AttendanceService
3. Example logs:

```
=== CreateAttendanceWithShiftAsync START ===
EmployeeID: 2
AttendanceDate: 2025-01-15
ScheduleStart: 05:00:00
ScheduleEnd: 17:00:00
Employee 2 verified.
Found existing record ID: 5, updating...
Calling SaveChangesAsync...
SaveChangesAsync completed. Rows affected: 1
=== CreateAttendanceWithShiftAsync SUCCESS ===
```

### Step 6: Network Inspection

If using the service test button:
1. Open **Network tab** in Developer Tools (F12)
2. Click "Test Service" button
3. Look for any failed network requests
4. Check response status codes (200 = success, 400+ = error)

## Manual Testing Sequence

1. **Preconditions:**
   - Open the Attendance page
   - Select an employee from the table
   - Click "Shift" button to open modal

2. **Test 1 - Direct Save:**
   - Select a shift type (required)
   - Click "?? Test Direct Save" button
   - Note the result and any errors

3. **Test 2 - Service Save:**
   - Select a shift type
   - Click "?? Test Service" button
   - Note the result and any errors

4. **Test 3 - Normal Save:**
   - Select a shift type
   - Click "?? Save Changes" button
   - Check if it works now

## Performance Tips

- If saving is slow, check database indexes on (EmployeeID, AttendanceDate)
- Verify network latency to database server
- Check for long-running queries blocking the insert

## Still Not Working?

1. **Collect All Debug Information:**
   - Browser console output (screenshot or copy-paste)
   - Visual Studio Output window logs
   - Network tab error responses
   - Any database error messages

2. **Check Database Directly:**
```sql
-- Check if record exists
SELECT * FROM [IT13].[dbo].[Attendance]
WHERE EmployeeID = [YourEmployeeID]
  AND AttendanceDate = CONVERT(DATE, '[YourDate]');

-- Check table structure
EXEC sp_help '[IT13].[dbo].[Attendance]';

-- Check constraints
SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE TABLE_NAME = 'Attendance' AND TABLE_SCHEMA = 'dbo';
```

3. **Verify Employee Exists:**
```sql
SELECT * FROM [IT13].[dbo].[Employee]
WHERE EmployeeID = [YourEmployeeID];
```

## Key Code Locations

- **Razor Component**: `Components/Pages/HumanResources/Attendance.razor`
- **Service**: `Components/Services/AttendanceService.cs`
- **Model**: Look for `Attendance` model in `Models/` directory
- **DbContext**: Look for `HestiaLinkContext` 

## Environment Check

```csharp
// In browser console, you can check:
// 1. Is the API reachable?
// 2. Is authentication working?
// 3. Are there CORS issues?

// The logs should show the full error chain
```

## Next Steps if Tests Pass

If both test buttons work but "Save Changes" doesn't:
1. The issue is in the `SaveScheduleChanges()` method logic
2. Check if parameters are being passed correctly
3. Verify SelectedAttendanceForEdit is not null
4. Ensure service methods are returning true

If both test buttons fail:
1. Database connection issue
2. Permission issue
3. Data type mismatch
4. Missing required fields

---
**Last Updated:** 2025-01-15
