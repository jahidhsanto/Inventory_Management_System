-- 1. Purpose-Wise In/Out by Month
CREATE PROCEDURE sp_GetMonthlyPurposeWiseInOut
AS
BEGIN
    SELECT 
        FORMAT(R.Created_Date, 'yyyy-MM') AS Month,
        R.Requisition_For,
        COUNT(DISTINCT R.Requisition_ID) AS RequisitionCount,
        SUM(MTL.In_Quantity) AS TotalIn,
        SUM(MTL.Out_Quantity) AS TotalOut
    FROM Requisition R
    LEFT JOIN Material_Transaction_Log MTL ON R.Requisition_ID = MTL.Requisition_ID
    GROUP BY FORMAT(R.Created_Date, 'yyyy-MM'), R.Requisition_For
    ORDER BY Month DESC
END

-- 2. Non-Moving Materials (No Transactions in Last 30 Days)
CREATE PROCEDURE sp_GetMonthlyNonMovingItems
AS
BEGIN
    SELECT M.Material_ID, M.Materials_Name, M.Stock_Quantity
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

-- 3. Pending Challans (Challan Not Fully Delivered)
CREATE PROCEDURE sp_GetPendingChallans
AS
BEGIN
    SELECT C.Challan_ID, C.Challan_Date, C.Challan_Type, CI.Material_ID, CI.Quantity
    FROM Challan C
    INNER JOIN Challan_Items CI ON C.Challan_ID = CI.Challan_ID
    WHERE C.Challan_Type = 'DELIVERY'
      AND NOT EXISTS (
          SELECT 1
          FROM Material_Transaction_Log MTL
          WHERE MTL.Challan_ID = C.Challan_ID
            AND MTL.Transaction_Type = 'DELIVERY'
      )
END

-- 4. Pending Test Report
-- Assuming there's a Requisition_Status 'TestPending'
CREATE PROCEDURE sp_GetPendingTestItems
AS
BEGIN
    SELECT R.Requisition_ID, M.Materials_Name, R.Quantity, R.Created_Date
    FROM Requisition R
    INNER JOIN Material M ON R.Material_ID = M.Material_ID
    WHERE R.Status = 'TestPending'
END

-- 5. Defective Report
CREATE PROCEDURE sp_GetDefectiveReport
AS
BEGIN
    SELECT S.Stock_ID, M.Materials_Name, S.Serial_Number, S.Quantity, S.Received_Date
    FROM Stock S
    INNER JOIN Material M ON S.Material_ID = M.Material_ID
    WHERE S.Status = 'DEFECTIVE'
END

-- 6. Monthly PR Summary
CREATE PROCEDURE sp_GetMonthlyPRSummary
AS
BEGIN
    SELECT 
        FORMAT(Created_Date, 'yyyy-MM') AS Month,
        COUNT(Requisition_ID) AS TotalPRs
    FROM Requisition
    WHERE Status = 'Approved' AND Store_Status = 'Ordered'
    GROUP BY FORMAT(Created_Date, 'yyyy-MM')
    ORDER BY Month DESC
END
