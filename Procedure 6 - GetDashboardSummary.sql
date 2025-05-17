CREATE PROCEDURE sp_GetDashboardSummary
AS
BEGIN
    -- 1st Result: Dashboard KPIs
    SELECT 
		(SELECT COUNT(*) FROM Material) AS TotalItems,
		(SELECT SUM(Quantity) FROM Stock) AS StockQuantity,
		(SELECT COUNT(*) FROM Requisition WHERE Store_Status = 'Pending') AS PendingRequisitions,
		(SELECT SUM(Quantity) FROM Stock WHERE Status = 'DEFECTIVE') AS DefectiveStock,
		(SELECT FORMAT(SUM(S.Quantity * M.Unit_Price), 'N2') AS StockValue FROM Stock S JOIN Material M ON S.Material_ID = M.Material_ID) AS StockValue,
		(SELECT COUNT(*) FROM Material WHERE Stock_Quantity < MSQ AND Stock_Quantity > 0) AS LowStock,
		(SELECT COUNT(*) FROM Material WHERE Stock_Quantity = 0) AS OutOfStock,
		(SELECT ISNULL(SUM(In_Quantity), 0) FROM Material_Transaction_Log WHERE Transaction_Type IN ('RECEIVE', 'RETURN') AND CAST(Transaction_Date AS DATE) = CAST(GETDATE() AS DATE)) AS ReceivedToday,
		(SELECT ISNULL(SUM(Out_Quantity), 0) FROM Material_Transaction_Log WHERE Transaction_Type = 'DELIVERY' AND CAST(Transaction_Date AS DATE) = CAST(GETDATE() AS DATE)) AS IssuedToday

	-- 2nd Result: Stock Trend (Last 7 Days)
	SELECT 
		FORMAT(Transaction_Date, 'ddd') AS DayName,
		CAST(Transaction_Date AS DATE) AS ExactDate,
		SUM(CASE WHEN Transaction_Type IN ('RECEIVE', 'RETURN') THEN In_Quantity ELSE 0 END) AS ReceivedQty,
		SUM(CASE WHEN Transaction_Type = 'DELIVERY' THEN Out_Quantity ELSE 0 END) AS DeliveredQty
	FROM Material_Transaction_Log
	WHERE CAST(Transaction_Date AS DATE) >= DATEADD(DAY, -6, CAST(GETDATE() AS DATE))
	GROUP BY FORMAT(Transaction_Date, 'ddd'), CAST(Transaction_Date AS DATE)
	ORDER BY CAST(Transaction_Date AS DATE)

    -- 3rd Result: Requisition Status Count
    SELECT 
        Store_Status,
        COUNT(*) AS Count
    FROM Requisition
    GROUP BY Store_Status
END



EXEC sp_GetDashboardSummary
