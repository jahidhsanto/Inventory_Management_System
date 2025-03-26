CREATE PROCEDURE UserLogin
    @Username NVARCHAR(255),
    @Password NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StoredPassword VARBINARY(64), 
            @Role NVARCHAR(50), 
            @User_ID INT,
            @Employee_ID INT,
            @EmployeeName NVARCHAR(255),
            @Designation NVARCHAR(255),
            @DepartmentName NVARCHAR(255);

    -- Retrieve the stored password hash, role, user ID, and employee details for the given username
    SELECT 
        @StoredPassword = u.Password_Hash, 
        @Role = r.Role, 
        @User_ID = u.User_ID,
        @Employee_ID = u.Employee_ID
    FROM dbo.Users u   -- Ensure the schema name (e.g., dbo) is correct
    JOIN dbo.Role r ON u.Role_ID = r.Role_ID   -- Also ensure Role table is referenced with correct schema
    WHERE u.Username = @Username;

    -- If no user found, return error
    IF @StoredPassword IS NULL
    BEGIN
        RAISERROR('Invalid username.', 16, 1);
        RETURN;
    END

    -- Compare the hashed version of the entered password with the stored hash
    IF @StoredPassword = HASHBYTES('SHA2_512', @Password)
    BEGIN
        -- Get employee details
        SELECT 
            @EmployeeName = e.Name, 
            @Designation = e.Designation,
            @DepartmentName = d.DepartmentName
        FROM dbo.Employee e    -- Ensure Employee and Department tables have correct schema names
        JOIN dbo.Department d ON e.Department_ID = d.Department_ID
        WHERE e.Employee_ID = @Employee_ID;

        PRINT 'Login successful';
        SELECT 
            @User_ID AS User_ID, 
            @Username AS Username, 
            @Role AS Role,
            @Employee_ID AS Employee_ID,  -- Adding Employee_ID to the output
            @EmployeeName AS EmployeeName, 
            @Designation AS Designation,
            @DepartmentName AS DepartmentName;
    END
    ELSE
    BEGIN
        RAISERROR('Invalid username or password.', 16, 1);
    END
END;


EXEC UserLogin 
    @Username = 'admin', 
    @Password = 'Admin@123';
