ALTER PROCEDURE DeleteUser
    @Username NVARCHAR(255),
    @AdminUsername NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AdminRole NVARCHAR(50);

    -- Check if the admin has permission to delete users
    SELECT @AdminRole = r.Role
    FROM Users u
    JOIN Role r ON u.Role_ID = r.Role_ID
    WHERE u.Username = @AdminUsername;

    IF @AdminRole <> 'Admin'
    BEGIN
        RAISERROR('Only admins can delete users.', 16, 1);
        RETURN;
    END

    -- Check if the user exists in the Users table
    IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        RAISERROR('User does not exist.', 16, 1);
        RETURN;
    END

    -- Delete the user from the Users table
    DELETE FROM Users WHERE Username = @Username;

    PRINT 'User deleted successfully.';
END;
