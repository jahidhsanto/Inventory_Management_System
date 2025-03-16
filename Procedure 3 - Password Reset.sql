CREATE PROCEDURE ResetPassword
    @Username NVARCHAR(255),
    @NewPassword NVARCHAR(255),
    @AdminUsername NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AdminRole NVARCHAR(50)

    -- Check if the admin user exists and has the 'Admin' role
    SELECT @AdminRole = Role FROM Users WHERE Username = @AdminUsername;

    IF @AdminRole <> 'Admin'
    BEGIN
        RAISERROR('Only admins can reset passwords.', 16, 1);
        RETURN;
    END

    -- Check if the user exists
    IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        RAISERROR('User does not exist.', 16, 1);
        RETURN;
    END

    -- Reset the password
    UPDATE Users
    SET Password_Hash = HASHBYTES('SHA2_512', @NewPassword)
    WHERE Username = @Username;

    PRINT 'Password reset successful.';
END;


EXEC ResetPassword @Username = 'jahid', @NewPassword = 'jahid@123', @AdminUsername = 'admin_user';
