use STORE;

CREATE PROCEDURE GetEmployeeByUsername
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the user exists first
    IF NOT EXISTS (SELECT 1 FROM users WHERE Username = @Username)
    BEGIN
        RAISERROR('User not found', 16, 1);
        RETURN;
    END

    -- Fetch employee data linked to the username
    SELECT e.Employee_ID, u.Username, e.Name, e.Department, e.Designation, u.Role
    FROM Employee e
    INNER JOIN users u ON e.Employee_ID = u.Employee_ID
    WHERE u.Username = @Username;
END;


EXEC GetEmployeeByUsername @Username = 'nazim';
