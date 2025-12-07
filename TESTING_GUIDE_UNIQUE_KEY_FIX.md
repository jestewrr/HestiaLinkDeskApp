# Testing Guide: UNIQUE KEY Violation Fix

## Setup
- Navigate to **Attendance Management** page (`/hr/attendance`)
- Ensure you can see employees in the table

## Test Scenario 1: Create New Shift (No Existing Record)

### Steps
1. Find an employee with `ScheduleStart` = empty ("Not Assigned")
2. Click the **?? Assign Shift** button
3. Expected: Modal opens with title "Assign Shift"

### In Modal
1. Select shift type: "Day" (5:00 AM - 5:00 PM)
2. Modal should auto-populate times
3. Click "?? Save Changes"
4. Expected: "? Shift saved successfully!"
5. Reload page
6. Expected: Employee now shows "Day Shift" badge

### ? Pass Criteria
- No UNIQUE KEY error
- Record created
- Schedule appears in table

---

## Test Scenario 2: Edit Existing Shift

### Steps
1. Find an employee with existing shift (has ScheduleStart value)
2. Click the **?? Edit Shift** button
3. Expected: Modal opens with title "Edit Attendance"

### In Modal
1. Current shift should be pre-filled
2. Change shift: Day ? Night (5:00 PM - 5:00 AM)
3. Click "?? Save Changes"
4. Expected: "? Shift saved successfully!"
5. Reload page
6. Expected: Employee now shows "Night Shift" badge

### ? Pass Criteria
- No UNIQUE KEY error
- Record updated (not duplicated)
- Shift changed successfully

---

## Test Scenario 3: Prevent Duplicate Creation (Critical Test)

### Steps
1. Create new shift for Employee A on Date X (as per Scenario 1)
2. Go back to main attendance page
3. Find the same Employee A
4. Click **?? Assign Shift** again
5. Expected: Modal opens BUT...
   - Title shows "Edit Attendance" (not "Assign Shift")
   - Record ID is > 0
   - This redirects to edit mode

### ? Pass Criteria
- System recognizes existing record
- Opens in EDIT mode, not NEW mode
- No UNIQUE KEY violation when saved

---

## Test Scenario 4: Edit Multiple Fields

### Steps
1. Find employee with existing shift
2. Click **?? Edit Shift**
3. In modal, change:
   - Shift type: Custom
   - Start time: 07:00 AM
   - End time: 04:00 PM
   - Actual Check In: 07:15 AM (if editing existing)
   - Actual Check Out: 04:00 PM (if editing existing)
   - Status: Present
   - Mark as Late: Yes
4. Click "?? Save Changes"
5. Expected: "? Shift saved successfully!"

### ? Pass Criteria
- All fields updated
- No validation errors
- Record has correct values

---

## Test Scenario 5: Console Logging Verification

### In Browser Developer Tools (F12 ? Console Tab)

#### When Creating New:
```
=== SaveScheduleChanges START ===
No existing record found, this is truly new.
>>> Creating NEW attendance record...
    EmployeeID: 2
    Date: 2025-12-07
>>> CreateAttendanceWithShiftAsync returned: true
=== SUCCESS: Shift saved! ===
```

#### When Editing Existing:
```
=== SaveScheduleChanges START ===
? Found existing record! ID: 5
   Status: Pending
   Schedule: 05:00:00 - 17:00:00
>>> Updating EXISTING attendance record ID: 5
>>> UpdateAttendanceRecordAsync returned: true
=== SUCCESS: Shift saved! ===
```

### ? Pass Criteria
- Console shows correct flow (CREATE vs UPDATE)
- No error messages
- AttendanceID is correct

---

## Regression Tests (Ensure Nothing Broke)

### Test 5.1: Check-In Still Works
1. Find employee with shift assigned
2. Click **?? Check In** button
3. Expected: Record updated, "?? Check In" button disappears
4. Expected: "?? Check Out" button appears

### Test 5.2: Check-Out Still Works
1. After checking in, click **?? Check Out** button
2. Expected: Record updated with checkout time
3. Expected: Hours calculated and displayed

### Test 5.3: History View Still Works
1. Click **?? View History** button
2. Expected: History modal opens
3. Expected: Attendance records displayed
4. Expected: Can edit records from history

### Test 5.4: Filters Still Work
1. Filter by Department
2. Filter by Status
3. Search by Name/ID
4. Change Date picker
5. Expected: Table updates correctly

---

## Error Scenarios

### ? If You See "UNIQUE KEY" Error
- This means a new record creation was attempted while one exists
- Check OpenAssignShiftModal logic
- Verify AttendanceID is being checked

### ? If Modal Opens in Wrong Mode
- Check `record.AttendanceID` value
- Should be > 0 for existing records
- Should be 0 for new records

### ? If Times Are Not Saved
- Check for "Schedule times are zero!" error
- Modal shows warning: "?? Please select a start time"
- Make sure you select Shift Type properly

---

## Success Indicators ?

| Issue | Before Fix | After Fix |
|-------|-----------|-----------|
| Try to assign shift twice | UNIQUE KEY error | Opens in edit mode |
| Change Day to Night shift | UNIQUE KEY error | Works perfectly |
| Edit existing record | May create duplicate | Updates correctly |
| New employee (no record) | Creates correctly | Creates correctly |
| Schedule times blank | NULL error | Shows validation warning |

## Database State Validation

### Check Database Directly (SQL)
```sql
-- Should show ONLY ONE record per employee per day
SELECT EmployeeID, AttendanceDate, COUNT(*) as RecordCount
FROM Attendance
WHERE AttendanceDate = '2025-12-07'
GROUP BY EmployeeID, AttendanceDate
HAVING COUNT(*) > 1;

-- Should return NO rows (no duplicates)
```

---

## Performance Checks
- Saving shift takes < 2 seconds
- History modal loads quickly
- No UI lag when switching between employees
- Page remains responsive during save

---

## Conclusion
All tests passing = UNIQUE KEY violation is fixed and no functionality is broken! ??
