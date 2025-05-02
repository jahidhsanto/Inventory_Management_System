INSERT INTO Material_Ledger (
    Material_ID, Challan_ID, Challan_Date, 
	Ledger_Type, In_Quantity, Out_Quantity,
    Unit_Price, Balance_After_Transaction, Valuation_After_Transaction
)
select 
	Material_ID, 
	NULL AS Challan_ID, 
	GETDATE() AS Transaction_Date, 
	'OPENING' AS Transaction_Type, 
	Stock_Quantity AS In_Quantity, 
	0 AS Out_Quantity, 
	ISNULL(Unit_Price, 0) AS Unit_Price, 
	ISNULL(Stock_Quantity, 0) AS Balance_After_Transaction, 
	ISNULL(Stock_Quantity * Unit_Price, 0) AS Valuation_After_Transaction
from Material
Where Stock_Quantity IS NOT NULL AND Stock_Quantity > 0
