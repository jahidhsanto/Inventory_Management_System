Create database STORE;

use STORE;

-- Create Employee Table
CREATE TABLE Employee (
    Employee_ID INT PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Department_ID INT,
    Designation NVARCHAR(255),
    FOREIGN KEY (Department_ID) REFERENCES Department(Department_ID)
);

-- Create Department Table
CREATE TABLE Department (
    Department_ID INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(255) NOT NULL,
    Department_Head_ID INT,
    FOREIGN KEY (Department_Head_ID) REFERENCES Employee(Employee_ID)
);

-- Create the Role table
CREATE TABLE Role (
    Role_ID INT PRIMARY KEY IDENTITY(1,1),
    Role NVARCHAR(100) NOT NULL UNIQUE
);

-- Create the Users table
CREATE TABLE Users (
    User_ID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(255) NOT NULL,
    Password_Hash VARBINARY(64) NOT NULL,
    Role_ID INT NOT NULL, 
    Employee_ID INT NOT NULL,
    CONSTRAINT FK_Users_Role FOREIGN KEY (Role_ID) REFERENCES Role(Role_ID),
    CONSTRAINT FK_Users_Employee FOREIGN KEY (Employee_ID) REFERENCES Employee(Employee_ID)
);

-- Create Com_Non_Com Table
CREATE TABLE Com_Non_Com (
    Com_Non_Com_ID INT IDENTITY(1,1) PRIMARY KEY,
    Com_Non_Com NVARCHAR(50) NOT NULL UNIQUE
);

-- Create Asset_Status Table
CREATE TABLE Asset_Status (
    Asset_Status_ID INT IDENTITY(1,1) PRIMARY KEY,
    Asset_Status NVARCHAR(50) NOT NULL UNIQUE
);

-- Create Asset_Type_Grouped Table
CREATE TABLE Asset_Type_Grouped (
    Asset_Type_Grouped_ID INT IDENTITY(1,1) PRIMARY KEY,
    Asset_Type_Grouped NVARCHAR(50) NOT NULL UNIQUE
);

-- Create Category Table
CREATE TABLE Category (
    Category_ID INT IDENTITY(1,1) PRIMARY KEY,
    Category NVARCHAR(50) NOT NULL UNIQUE
);

-- Create Sub_Category Table
CREATE TABLE Sub_Category (
    Sub_Category_ID INT IDENTITY(1,1) PRIMARY KEY,
    Sub_Category NVARCHAR(50) NOT NULL UNIQUE
);

-- Create Model Table
CREATE TABLE Model (
    Model_ID INT IDENTITY(1,1) PRIMARY KEY,
    Model NVARCHAR(50) NOT NULL UNIQUE
);

-- Create Control Table
CREATE TABLE Control (
    Control_ID INT IDENTITY(1,1) PRIMARY KEY,
    Control NVARCHAR(50) NOT NULL UNIQUE
);

--Create UoMList Table
CREATE TABLE UoM (
    UoM_ID INT IDENTITY(1,1) PRIMARY KEY,
    UoM NVARCHAR(50) NOT NULL UNIQUE
);

-- Create Requisition Table
CREATE TABLE Requisition (
    Requisition_ID INT IDENTITY(1,1) PRIMARY KEY,
    Employee_ID INT,
    Material_ID INT,
    Quantity INT NOT NULL,

	Status NVARCHAR(50) CHECK (Status IN ('Rejected', 'Pending', 'Approved')),
	Store_Status NVARCHAR(50) CHECK (Store_Status IN ('Pending', 'Out of Stock', 'Ordered', 'Delivered')),
    
	Created_Date DATETIME DEFAULT GETDATE(),
    Approved_By INT,
	Store_Status_By INT,

    Requisition_For NVARCHAR(100) CHECK (Requisition_For IN ('Employee', 'Department', 'Zone', 'Project')),
    Employee_ID_For INT,
    Department_ID_For INT,
    Zone_ID_For INT,
    Project_Code_For NVARCHAR(20),
    Requisition_Type NVARCHAR(20),

    FOREIGN KEY (Employee_ID) REFERENCES Employee(Employee_ID),
    FOREIGN KEY (Material_ID) REFERENCES Material(Material_ID),
	FOREIGN KEY (Approved_By) REFERENCES Employee(Employee_ID),
	FOREIGN KEY (Store_Status_By) REFERENCES Employee(Employee_ID),
    FOREIGN KEY (Employee_ID_For) REFERENCES Employee(Employee_ID),
    FOREIGN KEY (Department_ID_For) REFERENCES Department(Department_ID),
    FOREIGN KEY (Zone_ID_For) REFERENCES Zone(Zone_ID),
    FOREIGN KEY (Project_Code_For) REFERENCES Project(Project_Code),

    CONSTRAINT CHK_Requisition_For_Matching CHECK (
        (Requisition_For = 'Employee' AND Employee_ID_For IS NOT NULL AND Department_ID_For IS NULL AND Zone_ID_For IS NULL AND Project_Code_For IS NULL AND Requisition_Type IS NULL) OR
        (Requisition_For = 'Department' AND Department_ID_For IS NOT NULL AND Employee_ID_For IS NULL AND Zone_ID_For IS NULL AND Project_Code_For IS NULL AND Requisition_Type IS NULL) OR
        (Requisition_For = 'Zone' AND Zone_ID_For IS NOT NULL AND Employee_ID_For IS NULL AND Department_ID_For IS NULL AND Project_Code_For IS NULL AND Requisition_Type IS NULL) OR
        (Requisition_For = 'Project' AND Project_Code_For IS NOT NULL AND Requisition_Type IS NOT NULL AND Employee_ID_For IS NULL AND Department_ID_For IS NULL AND Zone_ID_For IS NULL)
    )
);

-- Create Material Table
CREATE TABLE Material (
    Material_ID INT IDENTITY(1,1) PRIMARY KEY,
    Part_Id NVARCHAR(255) NOT NULL,
    Com_Non_Com_ID INT NOT NULL,
    Asset_Status_ID INT NOT NULL,
    Asset_Type_Grouped_ID INT NOT NULL,
    Category_ID INT NOT NULL,
    Sub_Category_ID INT NOT NULL,
    Model_ID INT NOT NULL,
    Control_ID INT NOT NULL,
    Materials_Name NVARCHAR(255) NOT NULL,
    Unit_Price DECIMAL(10,2) NOT NULL DEFAULT 0,
    Stock_Quantity DECIMAL(10,2) NOT NULL DEFAULT 0,
    UoM INT NOT NULL,
	MSQ DECIMAL(10, 2) NOT NULL,
	Requires_Serial_Number NVARCHAR(3) NOT NULL CHECK (Requires_Serial_Number IN ('Yes', 'No')),
	Material_Image_Path VARCHAR(500),
	Warranty_Period_Months INT DEFAULT 0,

    -- Add UNIQUE constraint on Part_Id to ensure it is unique
    CONSTRAINT UQ_Part_Id UNIQUE (Part_Id),
    
    -- Foreign Key Constraints
    FOREIGN KEY (Com_Non_Com_ID) REFERENCES Com_Non_Com(Com_Non_Com_ID),
    FOREIGN KEY (Asset_Status_ID) REFERENCES Asset_Status(Asset_Status_ID),
    FOREIGN KEY (Asset_Type_Grouped_ID) REFERENCES Asset_Type_Grouped(Asset_Type_Grouped_ID),
    FOREIGN KEY (Category_ID) REFERENCES Category(Category_ID),
    FOREIGN KEY (Sub_Category_ID) REFERENCES Sub_Category(Sub_Category_ID),
    FOREIGN KEY (Model_ID) REFERENCES Model(Model_ID),
    FOREIGN KEY (Control_ID) REFERENCES Control(Control_ID),
	FOREIGN KEY (UoM) REFERENCES UoM(UoM_ID)
);

-- Create Stock Table 
CREATE TABLE Stock (
    Stock_ID INT IDENTITY(1,1) PRIMARY KEY,
    Material_ID INT NOT NULL,
    Serial_Number NVARCHAR(255),
	Rack_Number NVARCHAR(50) NOT NULL,
    Shelf_Number NVARCHAR(50) NOT NULL,
	Status NVARCHAR(50) NOT NULL CHECK (Status IN ('ACTIVE', 'DEFECTIVE')),
	Availability NVARCHAR(50) NOT NULL CHECK (Availability IN('AVAILABLE', 'DELIVERED')),
	Quantity DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    Received_Date DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (Material_ID) REFERENCES Material(Material_ID)
);
-- Create a filtered unique index for Serial_Number to allow multiple NULLs
CREATE UNIQUE INDEX UQ_Serial_Number 
ON Stock (Serial_Number)
WHERE Serial_Number IS NOT NULL;

-- Create Challan Table
CREATE TABLE Challan (
    Challan_ID INT IDENTITY(1,1) PRIMARY KEY,
    Challan_Date DATETIME DEFAULT GETDATE(),
    Challan_Type NVARCHAR(50) CHECK (Challan_Type IN ('DELIVERY', 'RETURN', 'RECEIVE')),
	Reference_Challan_ID INT NULL,
    Remarks NVARCHAR(1000),
	FOREIGN KEY (Reference_Challan_ID) REFERENCES Challan(Challan_ID)
);

-- Create Challan_Items Table
CREATE TABLE Challan_Items (
    Challan_Item_ID INT IDENTITY(1,1) PRIMARY KEY,
    Challan_ID INT NOT NULL,
    Requisition_ID INT NOT,
    Material_ID INT NOT NULL,
    Stock_ID INT NOT NULL,
	Serial_Number NVARCHAR(50) NULL,
    Quantity DECIMAL(10,2) NOT NULL CHECK (Quantity > 0),
    FOREIGN KEY (Challan_ID) REFERENCES Challan(Challan_ID),
    FOREIGN KEY (Stock_ID) REFERENCES Stock(Stock_ID),
    FOREIGN KEY (Material_ID) REFERENCES Material(Material_ID),
	FOREIGN KEY (Requisition_ID) REFERENCES Requisition(Requisition_ID)
);

-- Create Transaction Log Table
CREATE TABLE Material_Transaction_Log (
    Transaction_ID INT IDENTITY(1,1) PRIMARY KEY,
    Material_ID INT NOT NULL,
    Stock_ID INT NULL, -- If tracked by stock item
    Serial_Number NVARCHAR(255) NULL,
	Material_Status NVARCHAR(50) NOT NULL CHECK (Material_Status IN ('ACTIVE', 'DEFECTIVE')),
    
    Transaction_Type NVARCHAR(50) NOT NULL CHECK (Transaction_Type IN ('DELIVERY', 'RETURN', 'RECEIVE')),
    Transaction_Date DATETIME DEFAULT GETDATE(),

    In_Quantity DECIMAL(10,2) NOT NULL DEFAULT 0,
    Out_Quantity DECIMAL(10,2) NOT NULL DEFAULT 0,
    Challan_ID INT NULL, -- Can be linked to Challan
    Requisition_ID INT NOT NULL, -- Optional
    ReceivedBy_Employee_ID INT NULL, -- Who received/delivered/returned
    Remarks NVARCHAR(500) NULL,
    CreatedBy_Employee_ID INT NOT NULL,

    FOREIGN KEY (Material_ID) REFERENCES Material(Material_ID),
    FOREIGN KEY (Stock_ID) REFERENCES Stock(Stock_ID),
    FOREIGN KEY (Challan_ID) REFERENCES Challan(Challan_ID),
    FOREIGN KEY (Requisition_ID) REFERENCES Requisition(Requisition_ID),
    FOREIGN KEY (ReceivedBy_Employee_ID) REFERENCES Employee(Employee_ID),
    FOREIGN KEY (CreatedBy_Employee_ID) REFERENCES Employee(Employee_ID)
);

-- Create Material Ledger Table
CREATE TABLE Material_Ledger (
    Ledger_ID INT IDENTITY(1,1) PRIMARY KEY,
    Material_ID INT NOT NULL,
    Challan_ID INT NULL,
    Challan_Date DATETIME NOT NULL,
    Ledger_Type NVARCHAR(50) NOT NULL CHECK (Ledger_Type IN ('OPENING', 'RECEIVE', 'DELIVERY', 'RETURN')),
    
    In_Quantity DECIMAL(10, 2) DEFAULT 0,
    Out_Quantity DECIMAL(10, 2) DEFAULT 0,
    Unit_Price DECIMAL(10, 2) NOT NULL,
    Total_Valuation AS ((In_Quantity - Out_Quantity) * Unit_Price) PERSISTED,

    Balance_After_Transaction DECIMAL(10, 2) NOT NULL,
    Valuation_After_Transaction DECIMAL(10, 2) NOT NULL,

    FOREIGN KEY (Material_ID) REFERENCES Material(Material_ID),
    FOREIGN KEY (Challan_ID) REFERENCES Challan(Challan_ID)
);

-- Create Purchase_Request Table
CREATE TABLE Purchase_Request (
    Purchase_Request_ID INT IDENTITY(1,1) PRIMARY KEY,
    Store_Person_ID INT,
    Material_ID INT,
    Quantity INT NOT NULL,
    Status NVARCHAR(50) CHECK (Status IN ('Pending', 'Approved', 'Ordered', 'Received')),
    Request_Date DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (Store_Person_ID) REFERENCES Employee(Employee_ID),
    FOREIGN KEY (Material_ID) REFERENCES Material(Material_ID)
);

-- Create Vendor Table
CREATE TABLE Vendor (
    Vendor_ID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Contact NVARCHAR(255),
    Address NVARCHAR(1000)
);

-- Create Procurement Table
CREATE TABLE Procurement (
    Procurement_ID INT IDENTITY(1,1) PRIMARY KEY,
    Purchase_Request_ID INT,
    Vendor_ID INT,
    Purchase_Date DATETIME DEFAULT GETDATE(),
    Total_Cost DECIMAL(10,2),
    FOREIGN KEY (Purchase_Request_ID) REFERENCES Purchase_Request(Purchase_Request_ID),
    FOREIGN KEY (Vendor_ID) REFERENCES Vendor(Vendor_ID)
);

-- Create Material_Receipt Table
CREATE TABLE Material_Receipt (
    Receipt_ID INT IDENTITY(1,1) PRIMARY KEY,
    Procurement_ID INT,
    Material_ID INT,
    Quantity INT NOT NULL,
    Received_Date DATETIME DEFAULT GETDATE(),
    Serial_Numbers NVARCHAR(MAX),
    FOREIGN KEY (Procurement_ID) REFERENCES Procurement(Procurement_ID),
    FOREIGN KEY (Material_ID) REFERENCES Material(Material_ID)
);

-- Create Return Table
CREATE TABLE Return_Tracking (
    Return_ID INT IDENTITY(1,1) PRIMARY KEY,
    Return_Challan_ID INT, 
    Original_Challan_Item_ID INT, 
    Return_Date DATETIME DEFAULT GETDATE(),
    Return_Status NVARCHAR(50) CHECK (Return_Status IN ('RECEIVED', 'REJECTED')),
    Is_Warranty BIT,
    Replacement_Challan_ID INT NULL,
    FOREIGN KEY (Return_Challan_ID) REFERENCES Challan(Challan_ID),
    FOREIGN KEY (Original_Challan_Item_ID) REFERENCES Challan_Items(Challan_Item_ID),
    FOREIGN KEY (Replacement_Challan_ID) REFERENCES Challan(Challan_ID)
);

-- Create Warranty Table
CREATE TABLE Warranty (
    Warranty_ID INT IDENTITY(1,1) PRIMARY KEY,
    Material_ID INT NOT NULL,
    Serial_Number NVARCHAR(255) NOT NULL,
    Start_Date DATE NOT NULL,
    End_Date DATE NOT NULL,
    Status NVARCHAR(50) CHECK (Status IN ('Under Warranty', 'Expired', 'Replaced')),
    FOREIGN KEY (Material_ID) REFERENCES Material(Material_ID),
    FOREIGN KEY (Serial_Number) REFERENCES Stock(Serial_Number)
);

CREATE TABLE Temp_Delivery (
    Temp_ID INT IDENTITY(1,1) PRIMARY KEY, 
    Stock_ID INT NOT NULL, 
    Material_ID INT NOT NULL, 
    Requisition_ID INT NULL,
	Delivered_Quantity DECIMAL(10,2) NOT NULL CHECK (Delivered_Quantity > 0), 
    Session_ID NVARCHAR(100) NOT NULL,  -- To track user session
    FOREIGN KEY (Stock_ID) REFERENCES Stock(Stock_ID),
    FOREIGN KEY (Material_ID) REFERENCES Material(Material_ID),
	FOREIGN KEY (Requisition_ID) REFERENCES Requisition(Requisition_ID)
);

CREATE TABLE Temp_Receiving (
    Temp_ID INT IDENTITY(1,1) PRIMARY KEY,
    Challan_ID INT NULL, -- Only for returns
    Material_ID INT NOT NULL,
    Stock_ID INT NULL, 
    Serial_Number NVARCHAR(100) NULL,
	Quantity DECIMAL(10,2) NOT NULL, 
    Rack_Number NVARCHAR(100),
    Shelf_Number NVARCHAR(100),
    Receive_Type NVARCHAR(50) NOT NULL, -- NewReceive, ReturnActiveReceive, ReturnDefectiveReceive
    Session_ID NVARCHAR(100) NOT NULL,
);

CREATE TABLE Owner (
    Owner_ID INT IDENTITY(1,1) PRIMARY KEY,
    Owner_Name NVARCHAR(255) NOT NULL,
);

CREATE TABLE Zone (
    Zone_ID INT IDENTITY(1,1) PRIMARY KEY,
    Zone_Name NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Project (
    Project_Code NVARCHAR(20) PRIMARY KEY, 
    Project_Name NVARCHAR(255) NOT NULL,
    Owner_ID INT NULL,
    Zone_ID INT, 
	Department NVARCHAR(50),
    FOREIGN KEY (Owner_ID) REFERENCES Owner(Owner_ID),
	FOREIGN KEY (Zone_ID) REFERENCES Zone(Zone_ID)
);
