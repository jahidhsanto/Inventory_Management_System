CREATE PROCEDURE InsertUser
    @Username NVARCHAR(255),
    @Password NVARCHAR(255),
    @Role_ID INT,
    @Employee_ID INT
AS
BEGIN
    -- Declare a variable to hold error messages
    DECLARE @ErrorMessage NVARCHAR(255);
    
    -- Check if Username already exists
    IF EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        SET @ErrorMessage = 'Username already exists.';
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN;
    END
    
    -- Check if Role_ID exists
    IF NOT EXISTS (SELECT 1 FROM Role WHERE Role_ID = @Role_ID)
    BEGIN
        SET @ErrorMessage = 'Invalid Role_ID.';
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN;
    END

    -- Check if Employee_ID exists
    IF NOT EXISTS (SELECT 1 FROM Employee WHERE Employee_ID = @Employee_ID)
    BEGIN
        SET @ErrorMessage = 'Invalid Employee_ID.';
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN;
    END

    -- If validation passes, insert the new user
    BEGIN TRY
        INSERT INTO Users (Username, Password_Hash, Role_ID, Employee_ID)
        VALUES (
			@Username, 
			HASHBYTES('SHA2_512', @Password),  -- Secure password hashing
			@Role_ID, 
			@Employee_ID);
    END TRY
    BEGIN CATCH
        -- Catch any errors during insert and return an error message
        SET @ErrorMessage = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;


EXEC InsertUser 
    @Username = 'admin',
    @Password = 'Admin',
    @Role_ID = 1,
    @Employee_ID = 1;




