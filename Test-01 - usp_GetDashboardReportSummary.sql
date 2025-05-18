CREATE PROCEDURE usp_GetDashboardReportSummary
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        -- 1. Monthly Purpose-wise In/Out (example: count of requisitions)
        (SELECT COUNT(*) FROM Requisition WHERE MONTH(Created_Date) = MONTH(GETDATE()) AND YEAR(Created_Date) = YEAR(GETDATE())) AS PurposeWiseInOut,

        -- 2. Monthly Non-Moving Materials (no movement in past 30 days)
        (SELECT COUNT(*) 
         FROM Material m 
         WHERE NOT EXISTS (
             SELECT 1 FROM Material_Transaction_Log t 
             WHERE t.Material_ID = m.Material_ID 
             AND t.Transaction_Date >= DATEADD(DAY, -30, GETDATE())
         )) AS NonMoving,

        -- 3. Monthly Challan Pending
        (SELECT COUNT(*) 
         FROM Challan c 
         WHERE NOT EXISTS (
             SELECT 1 FROM Challan_Items ci WHERE ci.Challan_ID = c.Challan_ID
         )) AS ChallanPending,

        -- 4. Monthly Test Pending (example placeholder)
        (SELECT COUNT(*) FROM Requisition WHERE Store_Status = 'Pending') AS TestPending,

        -- 5. Monthly Defective Items
        (SELECT COUNT(*) FROM Stock WHERE Status = 'DEFECTIVE') AS Defective,

        -- 6. Monthly PR (Purchase Requisition) Count
        (SELECT COUNT(*) FROM Requisition WHERE Requisition_Type = 'PR') AS PRCount
END;

