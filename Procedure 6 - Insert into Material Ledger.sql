CREATE PROCEDURE InsertMaterialLedgerEntry
    @Transaction_ID INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Material_ID INT,
            @Stock_ID INT,
            @Serial_Number NVARCHAR(255),
            @Transaction_Type NVARCHAR(50),
            @In_Quantity DECIMAL(18,2),
            @Out_Quantity DECIMAL(18,2),
            @Transaction_Date DATETIME,
            @Unit_Price DECIMAL(18,2),
            @Previous_Balance DECIMAL(18,2),
            @Previous_Valuation DECIMAL(18,2),
            @New_Balance DECIMAL(18,2),
            @New_Valuation DECIMAL(18,2);

    -- Get transaction log details
    SELECT 
        @Material_ID = Material_ID,
        @Stock_ID = Stock_ID,
        @Serial_Number = Serial_Number,
        @Transaction_Type = Transaction_Type,
        @In_Quantity = In_Quantity,
        @Out_Quantity = Out_Quantity,
        @Transaction_Date = Transaction_Date
    FROM Material_Transaction_Log
    WHERE Transaction_ID = @Transaction_ID;

    -- Get unit price from Material table
    SELECT @Unit_Price = Unit_Price FROM Material WHERE Material_ID = @Material_ID;

    -- Get last balance and valuation
    SELECT TOP 1
        @Previous_Balance = Balance_After_Transaction,
        @Previous_Valuation = Valuation_After_Transaction
    FROM Material_Ledger
    WHERE Material_ID = @Material_ID
    ORDER BY Transaction_Date DESC, Ledger_ID DESC;

    SET @Previous_Balance = ISNULL(@Previous_Balance, 0);
    SET @Previous_Valuation = ISNULL(@Previous_Valuation, 0);

    -- Compute new balance and valuation
    SET @New_Balance = @Previous_Balance + @In_Quantity - @Out_Quantity;
    SET @New_Valuation = @Previous_Valuation + (@In_Quantity * @Unit_Price) - (@Out_Quantity * @Unit_Price);

    -- Insert into ledger
    INSERT INTO Material_Ledger (
        Material_ID, Stock_ID, Serial_Number, Transaction_ID,
        Transaction_Date, Transaction_Type, In_Quantity, Out_Quantity,
        Unit_Price, Balance_After_Transaction, Valuation_After_Transaction
    )
    VALUES (
        @Material_ID, @Stock_ID, @Serial_Number, @Transaction_ID,
        @Transaction_Date, @Transaction_Type, @In_Quantity, @Out_Quantity,
        @Unit_Price, @New_Balance, @New_Valuation
    );
END
