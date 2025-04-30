CREATE TRIGGER trg_AfterInsert_TransactionLog
ON Material_Transaction_Log
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Transaction_ID INT;

    SELECT @Transaction_ID = Transaction_ID FROM inserted;

    EXEC InsertMaterialLedgerEntry @Transaction_ID;
END
