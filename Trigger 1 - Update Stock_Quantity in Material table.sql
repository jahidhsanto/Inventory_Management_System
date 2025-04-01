-- Trigger to update Stock_Quantity in Material table after Stock record is inserted
CREATE TRIGGER trg_UpdateStockQuantityAfterInsert
ON Stock
AFTER INSERT
AS
BEGIN
    UPDATE m
    SET m.Stock_Quantity = (
        SELECT SUM(s.Quantity)
        FROM Stock s
        WHERE s.Material_ID = m.Material_ID
    )
    FROM Material m
    INNER JOIN inserted i ON m.Material_ID = i.Material_ID;
END;
GO

-- Trigger to update Stock_Quantity in Material table after Stock record is deleted
CREATE TRIGGER trg_UpdateStockQuantityAfterDelete
ON Stock
AFTER DELETE
AS
BEGIN
    UPDATE m
    SET m.Stock_Quantity = (
        SELECT SUM(s.Quantity)
        FROM Stock s
        WHERE s.Material_ID = m.Material_ID
    )
    FROM Material m
    INNER JOIN deleted d ON m.Material_ID = d.Material_ID;
END;
GO

-- Trigger to update Stock_Quantity in Material table after Stock record is updated
CREATE TRIGGER trg_UpdateStockQuantityAfterUpdate
ON Stock
AFTER UPDATE
AS
BEGIN
    UPDATE m
    SET m.Stock_Quantity = (
        SELECT SUM(s.Quantity)
        FROM Stock s
        WHERE s.Material_ID = m.Material_ID
    )
    FROM Material m
    INNER JOIN inserted i ON m.Material_ID = i.Material_ID;
END;
GO


UPDATE Material
SET Stock_Quantity = (
    SELECT SUM(Quantity)
    FROM Stock
    WHERE Material_ID = Material.Material_ID
);
