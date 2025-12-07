-- ===================================================================
-- Migration: Add DepartmentID to Employee Table
-- Purpose: Add department assignment capability to employees
-- ===================================================================

-- Check if column exists, if not add it
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Employee' AND COLUMN_NAME = 'DepartmentID'
)
BEGIN
    ALTER TABLE Employee
    ADD DepartmentID INT NULL;
    
    -- Add foreign key constraint
    ALTER TABLE Employee
    ADD CONSTRAINT FK_Employee_Department FOREIGN KEY (DepartmentID)
    REFERENCES Department(DepartmentID)
    ON DELETE SET NULL;
    
    PRINT 'DepartmentID column added to Employee table successfully!';
END
ELSE
BEGIN
    PRINT 'DepartmentID column already exists in Employee table.';
END

GO
