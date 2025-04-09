use STORE;

BULK INSERT Zone
FROM 'D:\SQL\CEL STORE DATABASE\Zone.csv'
WITH (
    FORMAT = 'CSV', 
    FIRSTROW = 2, -- If there's a header
    FIELDTERMINATOR = ',', 
    ROWTERMINATOR = '\n', 
    TABLOCK
);

BULK INSERT Owner
FROM 'D:\SQL\CEL STORE DATABASE\Owner.csv'
WITH (
    FORMAT = 'CSV', 
    FIRSTROW = 2, -- If there's a header
    FIELDTERMINATOR = ',', 
    ROWTERMINATOR = '\n', 
    TABLOCK
);

BULK INSERT Project
FROM 'D:\SQL\CEL STORE DATABASE\Project.csv'
WITH (
    FORMAT = 'CSV', 
    FIRSTROW = 2, -- If there's a header
    FIELDTERMINATOR = ',', 
    ROWTERMINATOR = '\n', 
    TABLOCK
);

BULK INSERT Equipment
FROM 'D:\SQL\CEL STORE DATABASE\Equipment.csv'
WITH (
    FORMAT = 'CSV', 
    FIRSTROW = 2, -- If there's a header
    FIELDTERMINATOR = ',', 
    ROWTERMINATOR = '\n', 
    TABLOCK
);

BULK INSERT Com_Non_Com
FROM 'D:\SQL\CEL STORE DATABASE\Com_Non_Com.csv'
WITH (
    FORMAT = 'CSV', 
    FIRSTROW = 2, -- If there's a header
    FIELDTERMINATOR = ',', 
    ROWTERMINATOR = '\n', 
    TABLOCK
);

BULK INSERT Asset_Status
FROM 'D:\SQL\CEL STORE DATABASE\Asset_Status.csv'
WITH (
    FORMAT = 'CSV', 
    FIRSTROW = 2, -- If there's a header
    FIELDTERMINATOR = ',', 
    ROWTERMINATOR = '\n', 
    TABLOCK
);

BULK INSERT Asset_Type_Grouped
FROM 'D:\SQL\CEL STORE DATABASE\Asset_Type_Grouped.csv'
WITH (
    FORMAT = 'CSV', 
    FIRSTROW = 2, -- If there's a header
    FIELDTERMINATOR = ',', 
    ROWTERMINATOR = '\n', 
    TABLOCK
);

BULK INSERT Category
FROM 'D:\SQL\CEL STORE DATABASE\Category.csv'
WITH (
    FORMAT = 'CSV', 
    FIRSTROW = 2, -- If there's a header
    FIELDTERMINATOR = ',', 
    ROWTERMINATOR = '\n', 
    TABLOCK
);

BULK INSERT Sub_Category
FROM 'D:\SQL\CEL STORE DATABASE\Sub_Category.csv'
WITH (
    FORMAT = 'CSV', 
    FIRSTROW = 2, -- If there's a header
    FIELDTERMINATOR = ',', 
    ROWTERMINATOR = '\n', 
    TABLOCK
);

BULK INSERT Model
FROM 'D:\SQL\CEL STORE DATABASE\Model.csv'
WITH (
    FORMAT = 'CSV', 
    FIRSTROW = 2, -- If there's a header
    FIELDTERMINATOR = ',', 
    ROWTERMINATOR = '\n', 
    TABLOCK
);

BULK INSERT Control
FROM 'D:\SQL\CEL STORE DATABASE\Control.csv'
WITH (
    FORMAT = 'CSV', 
    FIRSTROW = 2, -- If there's a header
    FIELDTERMINATOR = ',', 
    ROWTERMINATOR = '\n', 
    TABLOCK
);

BULK INSERT UoM
FROM 'D:\SQL\CEL STORE DATABASE\UoM.csv'
WITH (
    FORMAT = 'CSV', 
    FIRSTROW = 2, -- If there's a header
    FIELDTERMINATOR = ',', 
    ROWTERMINATOR = '\n', 
    TABLOCK
);

BULK INSERT Material
FROM 'D:\SQL\CEL STORE DATABASE\Material.csv'
WITH (
    FORMAT = 'CSV', 
    FIRSTROW = 2, -- If there's a header
    FIELDTERMINATOR = ',', 
    ROWTERMINATOR = '\n', 
    TABLOCK
);

BULK INSERT Department
FROM 'D:\SQL\CEL STORE DATABASE\Department.csv'
WITH (
    FORMAT = 'CSV', 
    FIRSTROW = 2, -- If there's a header
    FIELDTERMINATOR = ',', 
    ROWTERMINATOR = '\n', 
    TABLOCK
);

BULK INSERT Employee
FROM 'D:\SQL\CEL STORE DATABASE\Employee.csv'
WITH (
    FORMAT = 'CSV', 
    FIRSTROW = 2, -- If there's a header
    FIELDTERMINATOR = ',', 
    ROWTERMINATOR = '\n', 
    TABLOCK
);

BULK INSERT Stock
FROM 'D:\SQL\CEL STORE DATABASE\Stock.txt'
WITH (
    FORMAT = 'CSV', 
    FIRSTROW = 2, -- If there's a header
    FIELDTERMINATOR = '\t', 
    ROWTERMINATOR = '\n', 
    TABLOCK
);

BULK INSERT Users
FROM 'D:\SQL\CEL STORE DATABASE\Users.csv'
WITH (
    FORMAT = 'CSV', 
    FIRSTROW = 2, -- If there's a header
    FIELDTERMINATOR = ',', 
    ROWTERMINATOR = '\n', 
    TABLOCK
);
