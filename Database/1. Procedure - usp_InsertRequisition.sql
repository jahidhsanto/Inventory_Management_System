-- First, create a User-Defined Table Type for item details
CREATE TYPE RequisitionItemType AS TABLE
(
    Material_ID INT,
    Quantity INT
);
GO

-- Now, create the stored procedure
CREATE PROCEDURE InsertRequisitionWithItems
    @CreatedByEmployee_ID INT,
    @Dept_Status NVARCHAR(50),
    @Store_Status NVARCHAR(50),
    @Requisition_For NVARCHAR(100),
    @Employee_ID_For INT = NULL,
    @Department_ID_For INT = NULL,
    @Zone_ID_For INT = NULL,
    @Project_Code_For NVARCHAR(20) = NULL,
    @Requisition_Purpose NVARCHAR(20) = NULL,
    @ItemList RequisitionItemType READONLY,
    @NewRequisitionID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Insert into Requisition_Parent
        INSERT INTO Requisition_Parent (
            CreatedByEmployee_ID,
            Dept_Status,
            Store_Status,
            Requisition_For,
            Employee_ID_For,
            Department_ID_For,
            Zone_ID_For,
            Project_Code_For,
            Requisition_Purpose
        )
        VALUES (
            @CreatedByEmployee_ID,
            @Dept_Status,
            @Store_Status,
            @Requisition_For,
            @Employee_ID_For,
            @Department_ID_For,
            @Zone_ID_For,
            @Project_Code_For,
            @Requisition_Purpose
        );

        -- Get the new Requisition_ID
        SET @NewRequisitionID = SCOPE_IDENTITY();

        -- Insert items into Requisition_Item_Child
        INSERT INTO Requisition_Item_Child (
            Requisition_ID,
            Material_ID,
            Quantity
        )
        SELECT @NewRequisitionID, Material_ID, Quantity
        FROM @ItemList;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;

        -- Optional: Return error details
        THROW;
    END CATCH
END;
GO
