-- =============================================
-- Quick Fix: Add missing columns for Housekeeping features
-- Run this script on your IT13 database
-- =============================================

USE IT13;
GO

PRINT 'Starting database migration for Housekeeping features...';
PRINT '';

-- =============================================
-- 1. ADD IsAvailable COLUMN TO SystemUser TABLE
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemUser]') AND name = 'IsAvailable')
BEGIN
    ALTER TABLE [dbo].[SystemUser]
    ADD [IsAvailable] BIT NOT NULL CONSTRAINT DF_SystemUser_IsAvailable DEFAULT 1;
    
    PRINT '? Added IsAvailable column to SystemUser table';
END
ELSE
BEGIN
    PRINT '? IsAvailable column already exists in SystemUser table';
END
GO

-- =============================================
-- 2. ADD MISSING COLUMNS TO Task TABLE
-- =============================================

-- Add AssignedDate column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Task]') AND name = 'AssignedDate')
BEGIN
    ALTER TABLE [dbo].[Task]
    ADD [AssignedDate] DATETIME NOT NULL CONSTRAINT DF_Task_AssignedDate DEFAULT GETDATE();
    
    PRINT '? Added AssignedDate column to Task table';
END
ELSE
BEGIN
    PRINT '? AssignedDate column already exists in Task table';
END
GO

-- Add Status column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Task]') AND name = 'Status')
BEGIN
    ALTER TABLE [dbo].[Task]
    ADD [Status] VARCHAR(20) NOT NULL CONSTRAINT DF_Task_Status DEFAULT 'Assigned';
    
    PRINT '? Added Status column to Task table';
END
ELSE
BEGIN
    PRINT '? Status column already exists in Task table';
END
GO

-- Add CompletedDate column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Task]') AND name = 'CompletedDate')
BEGIN
    ALTER TABLE [dbo].[Task]
    ADD [CompletedDate] DATETIME NULL;
    
    PRINT '? Added CompletedDate column to Task table';
END
ELSE
BEGIN
    PRINT '? CompletedDate column already exists in Task table';
END
GO

-- Add Notes column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Task]') AND name = 'Notes')
BEGIN
    ALTER TABLE [dbo].[Task]
    ADD [Notes] NVARCHAR(500) NULL;
    
    PRINT '? Added Notes column to Task table';
END
ELSE
BEGIN
    PRINT '? Notes column already exists in Task table';
END
GO

-- Add CompletionStatus column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Task]') AND name = 'CompletionStatus')
BEGIN
    ALTER TABLE [dbo].[Task]
    ADD [CompletionStatus] VARCHAR(20) NULL;
    
    PRINT '? Added CompletionStatus column to Task table';
END
ELSE
BEGIN
    PRINT '? CompletionStatus column already exists in Task table';
END
GO

-- =============================================
-- 3. UPDATE EXISTING DATA
-- =============================================
PRINT '';
PRINT 'Updating existing data...';

-- Set all users as available by default
UPDATE [dbo].[SystemUser] 
SET [IsAvailable] = 1
WHERE [IsAvailable] IS NULL;

PRINT '? Updated SystemUser availability';

-- Update existing tasks to have default status
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Task]') AND name = 'Status')
BEGIN
    UPDATE [dbo].[Task] 
    SET [Status] = 'Assigned'
    WHERE [Status] IS NULL;
    
    PRINT '? Updated Task status defaults';
END
GO

-- =============================================
-- 4. VERIFY CHANGES
-- =============================================
PRINT '';
PRINT '=============================================';
PRINT 'Verifying SystemUser columns:';
PRINT '=============================================';

SELECT 
    c.name AS ColumnName,
    t.name AS DataType,
    c.is_nullable AS IsNullable,
    ISNULL(dc.definition, '') AS DefaultValue
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
LEFT JOIN sys.default_constraints dc ON c.default_object_id = dc.object_id
WHERE c.object_id = OBJECT_ID('SystemUser')
ORDER BY c.column_id;
GO

PRINT '';
PRINT '=============================================';
PRINT 'Verifying Task columns:';
PRINT '=============================================';

SELECT 
    c.name AS ColumnName,
    t.name AS DataType,
    c.is_nullable AS IsNullable,
    ISNULL(dc.definition, '') AS DefaultValue
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
LEFT JOIN sys.default_constraints dc ON c.default_object_id = dc.object_id
WHERE c.object_id = OBJECT_ID('Task')
ORDER BY c.column_id;
GO

PRINT '';
PRINT '=============================================';
PRINT 'Migration completed successfully!';
PRINT '=============================================';
PRINT '';
PRINT 'You can now restart your application.';
PRINT 'The CleaningSchedule page should work correctly.';
GO
