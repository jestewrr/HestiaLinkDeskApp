-- ===================================================================
-- DATABASE VIEWS FOR HESTIALINK APPLICATION
-- Run these scripts in SQL Server Management Studio
-- ===================================================================

-- ===================================================================
-- View: vw_RoomStatusDisplay
-- Purpose: Display room status with guest and type information
-- ===================================================================
IF OBJECT_ID('dbo.vw_RoomStatusDisplay', 'V') IS NOT NULL
    DROP VIEW dbo.vw_RoomStatusDisplay;
GO

CREATE VIEW dbo.vw_RoomStatusDisplay AS
SELECT 
    r.RoomID,
    r.RoomNumber,
    r.Floor,
    ISNULL(r.Status, 'Available') AS Status,
    CONCAT(g.FirstName, ' ', g.LastName) AS Guest,
    res.CheckInDate AS CheckIn,
    res.CheckOutDate AS CheckOut,
    rt.TypeName
FROM 
    Room r
    LEFT JOIN RoomType rt ON r.RoomTypeID = rt.RoomTypeID
    LEFT JOIN ReservedRoom rr ON r.RoomID = rr.RoomID
    LEFT JOIN Reservation res ON rr.ReservationID = res.ReservationID 
        AND res.Status IN ('Checked In', 'Checked Out')
    LEFT JOIN Guest g ON res.GuestID = g.GuestID
WHERE 
    rt.Status = 'Active';

GO

-- ===================================================================
-- View: vw_BookingHistory
-- Purpose: Display complete booking history with billing information
-- ===================================================================
IF OBJECT_ID('dbo.vw_BookingHistory', 'V') IS NOT NULL
    DROP VIEW dbo.vw_BookingHistory;
GO

CREATE VIEW dbo.vw_BookingHistory AS
SELECT 
    CONCAT('BK', FORMAT(res.ReservationID, '00000')) AS BookingId,
    CONCAT(g.FirstName, ' ', g.LastName) AS GuestName,
    g.Email,
    g.ContactNumber,
    CONCAT('Room ', r.RoomNumber) AS Room,
    res.CheckInDate AS CheckIn,
    res.CheckOutDate AS CheckOut,
    DATEDIFF(DAY, res.CheckInDate, res.CheckOutDate) AS Nights,
    res.Status AS BookingStatus,
    b.BillStatus AS PaymentStatus,
    ISNULL(b.GrandTotal, 0) AS Total,
    res.CreatedDate AS BookingDate
FROM 
    Reservation res
    INNER JOIN Guest g ON res.GuestID = g.GuestID
    LEFT JOIN ReservedRoom rr ON res.ReservationID = rr.ReservationID
    LEFT JOIN Room r ON rr.RoomID = r.RoomID
    LEFT JOIN Bill b ON res.ReservationID = b.ReservationID
ORDER BY 
    res.CreatedDate DESC;

GO

-- ===================================================================
-- Optional: View for Room Availability Status
-- ===================================================================
IF OBJECT_ID('dbo.vw_RoomAvailability', 'V') IS NOT NULL
    DROP VIEW dbo.vw_RoomAvailability;
GO

CREATE VIEW dbo.vw_RoomAvailability AS
SELECT 
    r.RoomNumber,
    r.Floor,
    rt.TypeName AS RoomType,
    CASE 
        WHEN r.Status = 'Available' THEN 'Available'
        WHEN r.Status = 'Occupied' THEN 'Occupied'
        WHEN r.Status = 'For Cleaning' THEN 'Cleaning'
        WHEN r.Status = 'Maintenance' THEN 'Maintenance'
        ELSE 'Unknown'
    END AS CurrentStatus,
    rt.BasePrice AS NightlyRate,
    rt.MaxOccupancy,
    CASE 
        WHEN r.Status = 'Available' THEN 1
        ELSE 0
    END AS IsAvailable
FROM 
    Room r
    INNER JOIN RoomType rt ON r.RoomTypeID = rt.RoomTypeID
WHERE 
    rt.Status = 'Active';

GO

PRINT 'All views created successfully!';
