# Quick Reference: UNIQUE KEY Violation Fix

## TL;DR
**Problem**: User clicks "Assign Shift" twice ? UNIQUE KEY error  
**Solution**: Check if record exists before opening modal  
**Result**: System redirects to EDIT mode if record exists

## The Fix (Two Changes)

### Change 1: In `OpenAssignShiftModal()`
```csharp
// BEFORE: Always opened in "new" mode
private void OpenAssignShiftModal(AttendanceRecordDto record)
{
    IsNewAttendance = true;  // ? Wrong if record exists
    // ... open modal ...
}

// AFTER: Check if record exists first
private void OpenAssignShiftModal(AttendanceRecordDto record)
{
    if (record.AttendanceID > 0)  // ? Record exists
    {
        OpenEditShiftModal(record);  // Redirect to edit
        return;
    }
    
    IsNewAttendance = true;  // ? Only if truly new
    // ... open modal ...
}
```

### Change 2: In `SaveScheduleChanges()`
```csharp
// BEFORE: Relied only on IsNewAttendance flag
private async Task SaveScheduleChanges()
{
    // ...validation...
    
    if (IsNewAttendance || SelectedAttendanceForEdit.AttendanceID == 0)  // ? Unreliable
    {
        // Create
    }
    else
    {
        // Update
    }
}

// AFTER: Double-check in database
private async Task SaveScheduleChanges()
{
    // ...validation...
    
    // ? Check what actually exists in database
    var existingRecord = await DbContext.Attendances
        .FirstOrDefaultAsync(a => a.EmployeeID == SelectedAttendanceForEdit.EmployeeID && 
                                 a.AttendanceDate.Date == EditAttendanceDate.Date);
    
    if (existingRecord != null)
    {
        IsNewAttendance = false;  // ? Correct flag
        SelectedAttendanceForEdit.AttendanceID = existingRecord.AttendanceID;  // ? Get real ID
    }
    else
    {
        IsNewAttendance = true;
    }
    
    // Now safe to create/update
    if (IsNewAttendance || SelectedAttendanceForEdit.AttendanceID == 0)
    {
        // Create
    }
    else
    {
        // Update
    }
}
```

## Why This Works

| Scenario | Before | After |
|----------|--------|-------|
| New employee (no record) | Opens in "Assign" mode | Opens in "Assign" mode ? |
| Employee with existing record | Opens in "Assign" mode (wrong!) | Opens in "Edit" mode ? |
| Try to save new when exists | UNIQUE KEY error ? | Updates instead ? |
| Try to save existing | Works | Works ? |

## Key Insight

```
The "Assign Shift" button shouldn't show if record already exists.
But if it gets clicked anyway, we redirect to EDIT mode instead of
trying to create a duplicate.
```

## Files Changed
- `Components/Pages/HumanResources/Attendance.razor`
  - `OpenAssignShiftModal()` method
  - `SaveScheduleChanges()` method

## Service Layer Support
The service was already prepared:
- `CreateAttendanceWithShiftAsync()`: Already checks for existing
- `UpdateAttendanceRecordAsync()`: Handles TimeSpan validation

## Testing (Quick)
1. Create new shift for Employee A ? ? Works
2. Click "Assign Shift" again for same Employee A ? ? Opens in EDIT mode
3. Change Day to Night shift ? ? Updates without error
4. Check database ? ? Still only one record

## When to Use

### "Assign Shift" (New Mode)
```
Click when: Employee has NO shift yet (ScheduleStart is NULL)
Expected: Modal opens to create shift
Button shows: "?? Assign Shift"
```

### "Edit Shift" (Existing Mode)
```
Click when: Employee already has shift
Expected: Modal opens to edit shift
Button shows: "?? Edit Shift"
```

### System Handling (Auto-Redirect)
```
If you somehow click "Assign Shift" on employee with existing record:
System says: "Found existing record! Opening in EDIT mode instead"
Modal shows: "Edit Attendance" (not "Assign Shift")
Result: ? No UNIQUE KEY violation
```

## Console Verification
Open browser console (F12) and look for:

**New record creation**:
```
No existing record found, this is truly new.
>>> Creating NEW attendance record...
>>> CreateAttendanceWithShiftAsync returned: true
? SUCCESS: Shift saved!
```

**Existing record update**:
```
? Found existing record! ID: 5
>>> Updating EXISTING attendance record ID: 5
>>> UpdateAttendanceRecordAsync returned: true
? SUCCESS: Shift saved!
```

## Root Cause (Technical)
The database has:
```sql
UNIQUE INDEX IX_Attendance_EmployeeID_AttendanceDate 
ON Attendance(EmployeeID, AttendanceDate)
```

This is **correct** - you want only 1 record per employee per day.

The bug was the UI didn't respect this by trying to create when one existed.

**Fix**: UI now respects the business rule.

## Summary
? **Before**: UNIQUE KEY violation when trying to assign twice  
? **After**: System recognizes existing record and edits instead  
? **Safety**: Multi-layer protection (UI, component, service, database)  
? **Performance**: Uses indexed queries for instant checks  
? **User Experience**: Seamless assign ? edit workflow  

---
**Status**: ? FIXED - Ready for production
