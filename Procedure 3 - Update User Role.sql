CREATE PROCEDURE UpdateUserRole
    @Username NVARCHAR(255),
    @NewRole_ID INT,
    @AdminUsername NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AdminRole NVARCHAR(50);

    -- Check if admin has permission
    SELECT @AdminRole = r.Role
    FROM Users u
    JOIN Role r ON u.Role_ID = r.Role_ID
    WHERE u.Username = @AdminUsername;

    IF @AdminRole <> 'Admin'
    BEGIN
        RAISERROR('Only admins can update user roles.', 16, 1);
        RETURN;
    END

    -- Check if the NewRole_ID exists in the Role table
    IF NOT EXISTS (SELECT 1 FROM Role WHERE Role_ID = @NewRole_ID)
    BEGIN
        RAISERROR('Role not found.', 16, 1);
        RETURN;
    END

    -- Update role in Users table
    UPDATE Users
    SET Role_ID = @NewRole_ID
    WHERE Username = @Username;

    PRINT 'User role updated successfully.';
END;
