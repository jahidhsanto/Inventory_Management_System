-- 1. Create the Users Table
CREATE TABLE Users (
    User_ID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(255) UNIQUE NOT NULL,
    Password_Hash VARBINARY(64) NOT NULL,
    Role NVARCHAR(50) NOT NULL CHECK (Role IN ('Admin', 'Employee', 'Manager')),
    Employee_ID INT UNIQUE NOT NULL,
    FOREIGN KEY (Employee_ID) REFERENCES Employee(Employee_ID)
);

-- 2. Procedure to insert data
CREATE PROCEDURE InsertUser
    @Username NVARCHAR(255),
    @Password NVARCHAR(255),
    @Role NVARCHAR(50),
    @Employee_ID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validate role input
    IF @Role NOT IN ('Admin', 'Employee', 'Manager')
    BEGIN
        RAISERROR('Invalid role. Allowed values are Admin, Employee, or Manager.', 16, 1);
        RETURN;
    END

    -- Check if username already exists
    IF EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
    BEGIN
        RAISERROR('Username already exists. Choose a different username.', 16, 1);
        RETURN;
    END

    -- Insert new user with hashed password
    INSERT INTO Users (Username, Password_Hash, Role, Employee_ID)
    VALUES (
        @Username,
        HASHBYTES('SHA2_512', @Password),  -- Secure password hashing
        @Role,
        @Employee_ID
    );

    PRINT 'User successfully inserted.';
END;

-- 3. Insert Data
EXEC InsertUser 
    @Username = 'admin', 
    @Password = 'Admin@123', 
    @Role = 'Admin', 
    @Employee_ID = 1;

EXEC InsertUser 
    @Username = 'jahid', 
    @Password = 'jahid@123', 
    @Role = 'Employee', 
    @Employee_ID = 3981;

EXEC InsertUser 
    @Username = 'nazim', 
    @Password = 'Nazim@123', 
    @Role = 'Manager', 
    @Employee_ID = 2335;

DROP TABLE Users;
drop procedure InsertUser;