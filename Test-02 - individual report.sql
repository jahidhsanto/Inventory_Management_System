-- 1. Purpose-wise In/Out Report
CREATE PROCEDURE usp_GetPurposeWiseInOut
    @Month INT,
    @Year INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Requisition_For,
        COUNT(CASE WHEN t.Transaction_Type = 'RECEIVE' THEN 1 END) AS InCount,
        COUNT(CASE WHEN t.Transaction_Type = 'DELIVERY' THEN 1 END) AS OutCount
    FROM Requisition r
    LEFT JOIN Material_Transaction_Log t ON r.Requisition_ID = t.Requisition_ID
    WHERE MONTH(r.Created_Date) = @Month AND YEAR(r.Created_Date) = @Year
    GROUP BY Requisition_For
END

-- 2. Non-Moving Materials
CREATE PROCEDURE sp_GetNonMovingMaterials
AS
BEGIN
    SELECT M.Material_ID, M.Materials_Name, M.Stock_Quantity, M.Unit_Price
    FROM Material M
    WHERE NOT EXISTS (
        SELECT 1
        FROM Material_Transaction_Log L
        WHERE L.Material_ID = M.Material_ID
          AND L.Transaction_Date >= DATEADD(DAY, -30, GETDATE())
    )
	AND M.Stock_Quantity > 0
    ORDER BY M.Materials_Name;
END

-- 3. Challan Pending
CREATE PROCEDURE sp_GetPendingChallans
    @FromDate DATE,
    @ToDate DATE
AS
BEGIN
    SELECT C.Challan_ID, C.Challan_Date, C.Challan_Type, C.Remarks
    FROM Challan C
    WHERE C.Challan_Date BETWEEN @FromDate AND @ToDate
    AND NOT EXISTS (
        SELECT 1 FROM Challan_Items CI WHERE CI.Challan_ID = C.Challan_ID
    )
    ORDER BY C.Challan_Date DESC;
END

-- 4. Test Pending
CREATE PROCEDURE sp_GetTestPendingMaterials
AS
BEGIN
    SELECT S.Stock_ID, M.Materials_Name, S.Serial_Number, S.Received_Date
    FROM Stock S
    INNER JOIN Material M ON S.Material_ID = M.Material_ID
    WHERE S.Status = 'ACTIVE'
    AND S.Availability = 'AVAILABLE'
    AND NOT EXISTS (
        SELECT 1 FROM Material_Transaction_Log L
        WHERE L.Stock_ID = S.Stock_ID AND L.Transaction_Type = 'RECEIVE'
    )
    ORDER BY S.Received_Date DESC;
END

-- 5. Defective Materials
CREATE PROCEDURE sp_GetDefectiveMaterials
    @FromDate DATE,
    @ToDate DATE
AS
BEGIN
    SELECT M.Materials_Name, COUNT(*) AS Defective_Count
    FROM Stock S
    INNER JOIN Material M ON S.Material_ID = M.Material_ID
    WHERE S.Status = 'DEFECTIVE'
    AND S.Received_Date BETWEEN @FromDate AND @ToDate
    GROUP BY M.Materials_Name
    ORDER BY Defective_Count DESC;
END


-- 6. PR Summary
CREATE PROCEDURE sp_GetMonthlyPRSummary
    @FromDate DATE,
    @ToDate DATE
AS
BEGIN
    SELECT Requisition_Type, COUNT(*) AS TotalPR
    FROM Requisition
    WHERE Created_Date BETWEEN @FromDate AND @ToDate
    GROUP BY Requisition_Type
    ORDER BY TotalPR DESC;
END


