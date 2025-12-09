-- =====================================================
-- SUPPLIER TABLE VERIFICATION AND FIX SCRIPT
-- Run this in SQL Server Management Studio
-- =====================================================

-- 1. CHECK IF TABLE EXISTS
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Supplier')
BEGIN
    PRINT 'Supplier table does not exist. Creating it now...'
    
    CREATE TABLE [dbo].[Supplier] (
        [SupplierID] INT IDENTITY(1,1) PRIMARY KEY,
        [SupplierCode] NVARCHAR(20) NOT NULL,
        [SupplierName] NVARCHAR(100) NOT NULL,
        [ContactPerson] NVARCHAR(100) NULL,
        [ContactPhone] NVARCHAR(20) NULL,
        [ContactEmail] NVARCHAR(100) NULL,
        [Address] NVARCHAR(500) NULL,
        [SupplierType] NVARCHAR(50) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [UpdatedDate] DATETIME NULL
    );
    
    -- Add unique constraint on SupplierCode
    ALTER TABLE [dbo].[Supplier]
    ADD CONSTRAINT UQ_Supplier_SupplierCode UNIQUE ([SupplierCode]);
    
    PRINT 'Supplier table created successfully!'
END
ELSE
BEGIN
    PRINT 'Supplier table exists. Checking structure...'
END

GO

-- 2. VIEW CURRENT TABLE STRUCTURE
SELECT 
    c.name AS ColumnName,
    t.name AS DataType,
    c.max_length AS MaxLength,
    c.is_nullable AS IsNullable,
    c.is_identity AS IsIdentity,
    ISNULL(dc.definition, 'NO DEFAULT') AS DefaultValue
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
LEFT JOIN sys.default_constraints dc ON c.default_object_id = dc.object_id
WHERE c.object_id = OBJECT_ID('Supplier')
ORDER BY c.column_id;

GO

-- 3. CHECK EXISTING CONSTRAINTS
SELECT 
    tc.CONSTRAINT_NAME,
    tc.CONSTRAINT_TYPE,
    cc.COLUMN_NAME
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE cc 
    ON tc.CONSTRAINT_NAME = cc.CONSTRAINT_NAME
WHERE tc.TABLE_NAME = 'Supplier';

GO

-- 4. CHECK FOR DUPLICATE SUPPLIER CODES (CASE-INSENSITIVE)
SELECT 
    LOWER(SupplierCode) as LowerCode,
    COUNT(*) AS DuplicateCount
FROM Supplier
GROUP BY LOWER(SupplierCode)
HAVING COUNT(*) > 1;

GO

-- 5. ADD MISSING DEFAULTS IF NOT EXISTS
-- Check and add IsActive default
IF NOT EXISTS (SELECT * FROM sys.default_constraints 
    WHERE parent_object_id = OBJECT_ID('Supplier') 
    AND COL_NAME(parent_object_id, parent_column_id) = 'IsActive')
BEGIN
    ALTER TABLE [dbo].[Supplier] ADD DEFAULT 1 FOR [IsActive];
    PRINT 'Added default for IsActive'
END

-- Check and add CreatedDate default
IF NOT EXISTS (SELECT * FROM sys.default_constraints 
    WHERE parent_object_id = OBJECT_ID('Supplier') 
    AND COL_NAME(parent_object_id, parent_column_id) = 'CreatedDate')
BEGIN
    ALTER TABLE [dbo].[Supplier] ADD DEFAULT GETDATE() FOR [CreatedDate];
    PRINT 'Added default for CreatedDate'
END

GO

-- 6. VIEW ALL DATA IN SUPPLIER TABLE
SELECT * FROM Supplier ORDER BY SupplierID;

GO

-- 7. TEST INSERT (Comment out after testing)
/*
INSERT INTO Supplier (SupplierCode, SupplierName, IsActive, CreatedDate)
VALUES ('TEST-001', 'Test Supplier', 1, GETDATE());

-- Delete test record
DELETE FROM Supplier WHERE SupplierCode = 'TEST-001';
*/
