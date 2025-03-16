CREATE PROCEDURE UserLogin
    @Username NVARCHAR(255),
    @Password NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StoredPassword VARBINARY(64), 
			@Role NVARCHAR(50), 
			@User_ID INT

    -- Retrieve the stored password hash and role for the given username
    SELECT @StoredPassword = Password_Hash, @Role = Role, @User_ID = User_ID
    FROM Users
    WHERE Username = @Username;

    -- If no user found, return error
    IF @StoredPassword IS NULL
    BEGIN
        RAISERROR('Invalid username.', 16, 1);
        RETURN;
    END

    -- Compare the hashed version of the entered password with the stored hash
    IF @StoredPassword = HASHBYTES('SHA2_512', @Password)
    BEGIN
        PRINT 'Login successful';
        SELECT @User_ID AS User_ID, @Username AS Username, @Role AS Role;
    END
    ELSE
    BEGIN
        RAISERROR('Invalid username or password.', 16, 1);
    END
END;


EXEC UserLogin 
    @Username = 'jahid', 
    @Password = 'jahid@123';
