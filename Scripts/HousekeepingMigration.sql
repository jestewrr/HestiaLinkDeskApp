-- =============================================
-- HestiaLink Hospitality Management System
-- Database Migration Script for Housekeeping Features
-- =============================================

-- =============================================
-- 1. ADD IsAvailable COLUMN TO SystemUser TABLE
-- This column tracks if housekeeping staff is available for new task assignments
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[SystemUser]') AND name = 'IsAvailable')
BEGIN
    ALTER TABLE [dbo].[SystemUser]
    ADD [IsAvailable] BIT NOT NULL DEFAULT 1;
    
    PRINT 'Added IsAvailable column to SystemUser table';
END
ELSE
BEGIN
    PRINT 'IsAvailable column already exists in SystemUser table';
END
GO

-- =============================================
-- 2. CREATE OR MODIFY Task TABLE
-- This replaces/updates any existing CleaningTask table
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Task')
BEGIN
    CREATE TABLE [dbo].[Task] (
        [TaskID] INT IDENTITY(1,1) PRIMARY KEY,
        [RoomID] INT NOT NULL,
        [UserID] INT NULL,
        [AssignedDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [Status] VARCHAR(20) NOT NULL DEFAULT 'Assigned',
        [CompletedDate] DATETIME NULL,
        [Notes] NVARCHAR(500) NULL,
        [CompletionStatus] VARCHAR(20) NULL,
        
        -- Foreign Key Constraints
        CONSTRAINT [FK_Task_Room] FOREIGN KEY ([RoomID]) 
            REFERENCES [dbo].[Room]([RoomID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Task_SystemUser] FOREIGN KEY ([UserID]) 
            REFERENCES [dbo].[SystemUser]([UserID]) ON DELETE SET NULL
    );
    
    -- Create indexes for better query performance
    CREATE NONCLUSTERED INDEX [IX_Task_RoomID] ON [dbo].[Task]([RoomID]);
    CREATE NONCLUSTERED INDEX [IX_Task_UserID] ON [dbo].[Task]([UserID]);
    CREATE NONCLUSTERED INDEX [IX_Task_Status] ON [dbo].[Task]([Status]);
    
    PRINT 'Created Task table with indexes';
END
ELSE
BEGIN
    -- If Task table exists, ensure all required columns are present
    
    -- Add CompletionStatus column if it doesn't exist
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Task]') AND name = 'CompletionStatus')
    BEGIN
        ALTER TABLE [dbo].[Task]
        ADD [CompletionStatus] VARCHAR(20) NULL;
        
        PRINT 'Added CompletionStatus column to Task table';
    END
    
    -- Add AssignedDate column if it doesn't exist
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Task]') AND name = 'AssignedDate')
    BEGIN
        ALTER TABLE [dbo].[Task]
        ADD [AssignedDate] DATETIME NOT NULL DEFAULT GETDATE();
        
        PRINT 'Added AssignedDate column to Task table';
    END
    
    -- Add Notes column if it doesn't exist
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Task]') AND name = 'Notes')
    BEGIN
        ALTER TABLE [dbo].[Task]
        ADD [Notes] NVARCHAR(500) NULL;
        
        PRINT 'Added Notes column to Task table';
    END
    
    PRINT 'Task table already exists - verified/added required columns';
END
GO

-- =============================================
-- 3. MIGRATE DATA FROM CleaningTask TO Task (if CleaningTask exists)
-- =============================================
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'CleaningTask')
BEGIN
    -- Only migrate if Task table is empty
    IF NOT EXISTS (SELECT TOP 1 1 FROM [dbo].[Task])
    BEGIN
        SET IDENTITY_INSERT [dbo].[Task] ON;
        
        INSERT INTO [dbo].[Task] ([TaskID], [RoomID], [UserID], [AssignedDate], [Status], [CompletedDate], [Notes])
        SELECT 
            [TaskID],
            [RoomID],
            [UserID],
            ISNULL([CreatedDate], GETDATE()) AS [AssignedDate],
            CASE 
                WHEN [Status] = 'Pending' THEN 'Assigned'
                ELSE [Status]
            END AS [Status],
            [CompletedDate],
            [Description] AS [Notes]
        FROM [dbo].[CleaningTask];
        
        SET IDENTITY_INSERT [dbo].[Task] OFF;
        
        PRINT 'Migrated data from CleaningTask to Task table';
    END
    ELSE
    BEGIN
        PRINT 'Task table already has data - skipping migration';
    END
END
GO

-- =============================================
-- 4. ADD CHECK CONSTRAINTS FOR STATUS VALUES
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Task_Status')
BEGIN
    ALTER TABLE [dbo].[Task]
    ADD CONSTRAINT [CK_Task_Status] 
    CHECK ([Status] IN ('Assigned', 'In Progress', 'Completed', 'Maintenance'));
    
    PRINT 'Added status check constraint to Task table';
END
GO

IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_Task_CompletionStatus')
BEGIN
    ALTER TABLE [dbo].[Task]
    ADD CONSTRAINT [CK_Task_CompletionStatus] 
    CHECK ([CompletionStatus] IS NULL OR [CompletionStatus] IN ('Cleaned', 'Maintenance Required'));
    
    PRINT 'Added completion status check constraint to Task table';
END
GO

-- =============================================
-- 5. CREATE STORED PROCEDURES FOR HOUSEKEEPING OPERATIONS
-- =============================================

-- Procedure to assign a cleaning task
IF OBJECT_ID('[dbo].[sp_AssignCleaningTask]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_AssignCleaningTask];
GO

CREATE PROCEDURE [dbo].[sp_AssignCleaningTask]
    @RoomID INT,
    @UserID INT,
    @Notes NVARCHAR(500) = 'Standard Cleaning',
    @NewTaskID INT OUTPUT,
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validate room exists and needs cleaning
        IF NOT EXISTS (SELECT 1 FROM [dbo].[Room] WHERE [RoomID] = @RoomID AND [Status] = 'For Cleaning')
        BEGIN
            SET @NewTaskID = 0;
            SET @Message = 'Room is not marked for cleaning';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Validate user is available housekeeping staff
        IF NOT EXISTS (SELECT 1 FROM [dbo].[SystemUser] 
                       WHERE [UserID] = @UserID 
                         AND [Role] = 'Housekeeping' 
                         AND [IsAvailable] = 1 
                         AND [Status] = 'Active')
        BEGIN
            SET @NewTaskID = 0;
            SET @Message = 'User is not available for task assignment';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Check if room already has an active task
        IF EXISTS (SELECT 1 FROM [dbo].[Task] 
                   WHERE [RoomID] = @RoomID AND [Status] IN ('Assigned', 'In Progress'))
        BEGIN
            SET @NewTaskID = 0;
            SET @Message = 'Room already has an active cleaning task';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Create the task
        INSERT INTO [dbo].[Task] ([RoomID], [UserID], [AssignedDate], [Status], [Notes])
        VALUES (@RoomID, @UserID, GETDATE(), 'Assigned', @Notes);
        
        SET @NewTaskID = SCOPE_IDENTITY();
        
        -- Update user availability
        UPDATE [dbo].[SystemUser]
        SET [IsAvailable] = 0, [UpdatedAt] = GETDATE()
        WHERE [UserID] = @UserID;
        
        SET @Message = 'Task assigned successfully';
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @NewTaskID = 0;
        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO

PRINT 'Created sp_AssignCleaningTask stored procedure';
GO

-- Procedure to complete a cleaning task
IF OBJECT_ID('[dbo].[sp_CompleteCleaningTask]', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[sp_CompleteCleaningTask];
GO

CREATE PROCEDURE [dbo].[sp_CompleteCleaningTask]
    @TaskID INT,
    @UserID INT,
    @CompletionType VARCHAR(20), -- 'Cleaned' or 'Maintenance Required'
    @Success BIT OUTPUT,
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @RoomID INT;
    DECLARE @NewRoomStatus VARCHAR(20);
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validate task exists and belongs to user
        SELECT @RoomID = [RoomID]
        FROM [dbo].[Task]
        WHERE [TaskID] = @TaskID AND [UserID] = @UserID;
        
        IF @RoomID IS NULL
        BEGIN
            SET @Success = 0;
            SET @Message = 'Task not found or not assigned to this user';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Determine new room status
        SET @NewRoomStatus = CASE @CompletionType
            WHEN 'Cleaned' THEN 'Available'
            WHEN 'Maintenance Required' THEN 'Maintenance'
            ELSE 'Available'
        END;
        
        -- Update task
        UPDATE [dbo].[Task]
        SET [Status] = 'Completed',
            [CompletedDate] = GETDATE(),
            [CompletionStatus] = @CompletionType
        WHERE [TaskID] = @TaskID;
        
        -- Update room status
        UPDATE [dbo].[Room]
        SET [Status] = @NewRoomStatus
        WHERE [RoomID] = @RoomID;
        
        -- Update user availability
        UPDATE [dbo].[SystemUser]
        SET [IsAvailable] = 1, [UpdatedAt] = GETDATE()
        WHERE [UserID] = @UserID;
        
        SET @Success = 1;
        SET @Message = 'Task completed. Room status: ' + @NewRoomStatus;
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @Success = 0;
        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO

PRINT 'Created sp_CompleteCleaningTask stored procedure';
GO

-- =============================================
-- 6. CREATE VIEW FOR HOUSEKEEPING DASHBOARD
-- =============================================
IF OBJECT_ID('[dbo].[vw_HousekeepingTaskSummary]', 'V') IS NOT NULL
    DROP VIEW [dbo].[vw_HousekeepingTaskSummary];
GO

CREATE VIEW [dbo].[vw_HousekeepingTaskSummary]
AS
SELECT 
    t.[TaskID],
    t.[RoomID],
    r.[RoomNumber],
    r.[Floor],
    rt.[TypeName] AS RoomType,
    t.[UserID],
    u.[Username] AS AssignedTo,
    t.[AssignedDate],
    t.[Status],
    t.[CompletedDate],
    t.[CompletionStatus],
    t.[Notes],
    DATEDIFF(MINUTE, t.[AssignedDate], ISNULL(t.[CompletedDate], GETDATE())) AS DurationMinutes
FROM [dbo].[Task] t
INNER JOIN [dbo].[Room] r ON t.[RoomID] = r.[RoomID]
LEFT JOIN [dbo].[RoomType] rt ON r.[RoomTypeID] = rt.[RoomTypeID]
LEFT JOIN [dbo].[SystemUser] u ON t.[UserID] = u.[UserID];
GO

PRINT 'Created vw_HousekeepingTaskSummary view';
GO

-- =============================================
-- 7. CREATE VIEW FOR AVAILABLE HOUSEKEEPERS
-- =============================================
IF OBJECT_ID('[dbo].[vw_AvailableHousekeepers]', 'V') IS NOT NULL
    DROP VIEW [dbo].[vw_AvailableHousekeepers];
GO

CREATE VIEW [dbo].[vw_AvailableHousekeepers]
AS
SELECT 
    u.[UserID],
    u.[Username],
    u.[EmployeeID],
    u.[IsAvailable],
    (SELECT COUNT(*) FROM [dbo].[Task] t 
     WHERE t.[UserID] = u.[UserID] AND t.[Status] = 'Completed' 
       AND CAST(t.[CompletedDate] AS DATE) = CAST(GETDATE() AS DATE)) AS TasksCompletedToday,
    (SELECT COUNT(*) FROM [dbo].[Task] t 
     WHERE t.[UserID] = u.[UserID] AND t.[Status] IN ('Assigned', 'In Progress')) AS ActiveTasks
FROM [dbo].[SystemUser] u
WHERE u.[Role] = 'Housekeeping' AND u.[Status] = 'Active';
GO

PRINT 'Created vw_AvailableHousekeepers view';
GO

-- =============================================
-- 8. VERIFY ROOM STATUS VALUES
-- Ensure Room table supports required status values
-- =============================================
-- Note: This is informational - adjust if your Room table has a check constraint
PRINT 'Ensure Room.Status column supports: Available, Occupied, For Cleaning, Maintenance';
GO

-- =============================================
-- 9. CREATE TRIGGER FOR AUTOMATIC TASK NOTIFICATION (Optional)
-- This can be used for logging or notifications
-- =============================================
IF OBJECT_ID('[dbo].[tr_Task_StatusChange]', 'TR') IS NOT NULL
    DROP TRIGGER [dbo].[tr_Task_StatusChange];
GO

CREATE TRIGGER [dbo].[tr_Task_StatusChange]
ON [dbo].[Task]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Log task completions (you can extend this to send notifications)
    IF UPDATE([Status])
    BEGIN
        INSERT INTO [dbo].[Task] ([RoomID], [UserID], [Status], [Notes])
        SELECT 
            NULL, -- No room - this is a log entry
            NULL, -- No user
            'LOG',
            'Task ' + CAST(i.[TaskID] AS NVARCHAR(10)) + ' changed from ' + 
            ISNULL(d.[Status], 'NULL') + ' to ' + i.[Status]
        FROM inserted i
        INNER JOIN deleted d ON i.[TaskID] = d.[TaskID]
        WHERE i.[Status] <> d.[Status]
          AND 0 = 1; -- Disabled by default - remove this line to enable logging
    END
END
GO

PRINT 'Created tr_Task_StatusChange trigger (disabled by default)';
GO

-- =============================================
-- 10. GRANT PERMISSIONS (Adjust as needed for your security model)
-- =============================================
-- GRANT EXECUTE ON [dbo].[sp_AssignCleaningTask] TO [YourAppRole];
-- GRANT EXECUTE ON [dbo].[sp_CompleteCleaningTask] TO [YourAppRole];
-- GRANT SELECT ON [dbo].[vw_HousekeepingTaskSummary] TO [YourAppRole];
-- GRANT SELECT ON [dbo].[vw_AvailableHousekeepers] TO [YourAppRole];

PRINT '';
PRINT '=============================================';
PRINT 'Migration completed successfully!';
PRINT '=============================================';
PRINT '';
PRINT 'Summary of changes:';
PRINT '1. Added IsAvailable column to SystemUser table';
PRINT '2. Created/Updated Task table with CompletionStatus column';
PRINT '3. Created stored procedures for task assignment and completion';
PRINT '4. Created views for housekeeping dashboard';
PRINT '';
PRINT 'Next steps:';
PRINT '1. Review and uncomment GRANT statements if needed';
PRINT '2. Test the application with new features';
PRINT '3. Ensure Entity Framework model matches database schema';
GO
