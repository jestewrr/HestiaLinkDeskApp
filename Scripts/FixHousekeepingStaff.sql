-- =============================================
-- Script to check and fix housekeeping staff
-- =============================================

-- 1. Check what roles exist in SystemUser table
SELECT DISTINCT Role, COUNT(*) as Count
FROM SystemUser
GROUP BY Role;

-- 2. Check all active users
SELECT UserID, Username, Role, Status, 
       CASE WHEN IsAvailable IS NULL THEN 'NULL' 
            WHEN IsAvailable = 1 THEN 'Yes' 
            ELSE 'No' END as IsAvailable
FROM SystemUser
WHERE Status = 'Active';

-- 3. Check if there are any Housekeeping users
SELECT * FROM SystemUser WHERE Role = 'Housekeeping';

-- 4. Check if IsAvailable column exists
SELECT COLUMN_NAME, DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'SystemUser' AND COLUMN_NAME = 'IsAvailable';

-- =============================================
-- If no housekeeping staff exists, run this to create some:
-- =============================================

-- Add IsAvailable column if it doesn't exist
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
               WHERE TABLE_NAME = 'SystemUser' AND COLUMN_NAME = 'IsAvailable')
BEGIN
    ALTER TABLE SystemUser ADD IsAvailable BIT DEFAULT 1;
    PRINT 'Added IsAvailable column';
END

-- Set all existing users to available by default
UPDATE SystemUser SET IsAvailable = 1 WHERE IsAvailable IS NULL;

-- Insert sample housekeeping staff (only if none exist)
IF NOT EXISTS (SELECT 1 FROM SystemUser WHERE Role = 'Housekeeping')
BEGIN
    -- Insert housekeeping staff
    INSERT INTO SystemUser (Username, PasswordHash, Role, Status, IsAvailable, CreatedAt)
    VALUES 
        ('housekeeper1', 'password123', 'Housekeeping', 'Active', 1, GETDATE()),
        ('housekeeper2', 'password123', 'Housekeeping', 'Active', 1, GETDATE()),
        ('housekeeper3', 'password123', 'Housekeeping', 'Active', 1, GETDATE());
    
    PRINT 'Added 3 housekeeping staff members';
END
ELSE
BEGIN
    PRINT 'Housekeeping staff already exist';
END

-- Update existing housekeeping staff to be available
UPDATE SystemUser 
SET IsAvailable = 1 
WHERE Role = 'Housekeeping' AND Status = 'Active';

PRINT 'Updated all housekeeping staff to be available';

-- 5. Verify the fix
SELECT UserID, Username, Role, Status, IsAvailable
FROM SystemUser
WHERE Role = 'Housekeeping';
