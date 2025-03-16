-- Create a New User
CREATE PROCEDURE CreateUser
    @Username NVARCHAR(255),
    @Password NVARCHAR(255),
    @Role NVARCHAR(50),
    @Employee_ID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validate role
    IF @Role NOT IN ('Admin', 'Employee', 'Manager')
    BEGIN
        RAISERROR('Invalid role. Allowed roles: Admin, Employee, Manager.', 16, 1);
        RETURN;
    END

    -- Check if username already exists
    IF EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        RAISERROR('Username already exists.', 16, 1);
        RETURN;
    END

    -- Insert user
    INSERT INTO Users (Username, Password_Hash, Role, Employee_ID)
    VALUES (@Username, HASHBYTES('SHA2_512', @Password), @Role, @Employee_ID);

    PRINT 'User created successfully.';
END;



-- Update User Role
CREATE PROCEDURE UpdateUserRole
    @Username NVARCHAR(255),
    @NewRole NVARCHAR(50),
    @AdminUsername NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AdminRole NVARCHAR(50)

    -- Check if admin has permission
    SELECT @AdminRole = Role FROM Users WHERE Username = @AdminUsername;

    IF @AdminRole <> 'Admin'
    BEGIN
        RAISERROR('Only admins can update user roles.', 16, 1);
        RETURN;
    END

    -- Validate role
    IF @NewRole NOT IN ('Admin', 'Employee', 'Manager')
    BEGIN
        RAISERROR('Invalid role. Allowed roles: Admin, Employee, Manager.', 16, 1);
        RETURN;
    END

    -- Update role
    UPDATE Users
    SET Role = @NewRole
    WHERE Username = @Username;

    PRINT 'User role updated successfully.';
END;


-- Delete User
CREATE PROCEDURE DeleteUser
    @Username NVARCHAR(255),
    @AdminUsername NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AdminRole NVARCHAR(50)

    -- Check if admin has permission
    SELECT @AdminRole = Role FROM Users WHERE Username = @AdminUsername;

    IF @AdminRole <> 'Admin'
    BEGIN
        RAISERROR('Only admins can delete users.', 16, 1);
        RETURN;
    END

    -- Check if user exists
    IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        RAISERROR('User does not exist.', 16, 1);
        RETURN;
    END

    -- Delete user
    DELETE FROM Users WHERE Username = @Username;

    PRINT 'User deleted successfully.';
END;

