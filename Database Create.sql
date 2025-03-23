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

-- Create Com_Non_Com Table
CREATE TABLE Com_Non_Com (
    Com_Non_Com_ID INT IDENTITY(1,1) PRIMARY KEY,
    Com_Non_Com NVARCHAR(50) NOT NULL UNIQUE
);
-- Insert allowed values into the Com_Non_Com table
INSERT INTO Com_Non_Com (Com_Non_Com)
VALUES ('NON-COMMERCIAL'), ('COMMERCIAL');

-- Create Asset_Status Table
CREATE TABLE Asset_Status (
    Asset_Status_ID INT IDENTITY(1,1) PRIMARY KEY,
    Asset_Status NVARCHAR(50) NOT NULL UNIQUE
);
-- Insert allowed values into the Asset_Status table
INSERT INTO Asset_Status (Asset_Status)
VALUES ('CLAIM ITEMS'), ('READY STOCK'), ('DLFI ITEMS'), ('PPE ITEMS'), ('TOOLS ITEMS'), ('NI INSTALLATION ITEMS'), ('DEFECTIVE ITEMS (MISC.)'), ('REPAIRING STOCK'), ('OFFICE STATIONARY'), ('STICKER ITEMS'), ('LARSEN & TOUBRO (L&T)'), ('SAFETY ITEMS'), ('COMPLETE LIFT');

-- Create Asset_Type_Grouped Table
CREATE TABLE Asset_Type_Grouped (
    Asset_Type_Grouped_ID INT IDENTITY(1,1) PRIMARY KEY,
    Asset_Type_Grouped NVARCHAR(50) NOT NULL UNIQUE
);
-- Insert allowed values into the Asset_Type_Grouped table
INSERT INTO Asset_Type_Grouped (Asset_Type_Grouped)
VALUES ('CLAIM'), ('SCHINDLER'), ('RETROFIT MATERIALS'), ('ELEVATOR -HAU'), ('COMMON S/H'), ('CONSUMABLE ITEMS'), ('DEFECTIVE/SCRAP'), ('MOD ITEMS'), ('L & T'), ('TRADING');

-- Create Category Table
CREATE TABLE Category (
    Category_ID INT IDENTITY(1,1) PRIMARY KEY,
    Category NVARCHAR(50) NOT NULL UNIQUE
);
-- Insert allowed values into the Category table
INSERT INTO Category (Category)
VALUES ('CLAIM MATERIALS'), ('ELECTRICAL'), ('MECHANICAL'), ('DLFI'), ('SAFETY ITEMS'), ('ESCALATOR'), ('REPAIRING ITEMS'), ('MODIFICATION'), ('STATIONARY'), ('STICKER'), ('PPE ITEMS'), ('SERVICING MATERIALS'), ('WORKMANSHIP ITEMS');

-- Create Sub_Category Table
CREATE TABLE Sub_Category (
    Sub_Category_ID INT IDENTITY(1,1) PRIMARY KEY,
    Sub_Category NVARCHAR(50) NOT NULL UNIQUE
);
-- Insert allowed values into the Sub_Category table
INSERT INTO Sub_Category (Sub_Category)
VALUES ('SAFETY GEAR'), ('PCB'), ('LOP BASE'), ('MECHANICAL'), ('STM'), ('CABLE'), ('LOP COMPLETE'), ('VAF DRIVE'), ('ELECTRICAL'), ('SENSOR'), ('CAR PART'), ('TRACK'), ('PUSH BUTTON'), ('PIT SWITCH'), ('SWITCH'), ('BUFFER'), ('COMMON ITEMS'), ('CRANK CPL'), ('ENCODER'), ('POWER CONTROL'), ('BRAKE'), ('GLOVES'), ('TOOLS'), ('GUM'), ('MAGNETIC CONTACTOR'), ('NI MATERIALS'), ('HOLDER'), ('SKIIP'), ('ROLLER'), ('GUIDE CLIP'), ('GEARLESS MACHINE'), ('STM BELT'), ('LADDER'), ('POWER SUPPLY'), ('CONTACTOR/RELAY'), ('INVERTER'), ('GONG'), ('MOTOR CABLE'), ('IRON PLATE'), ('DISTANCE PLATE'), ('PIT RECALL'), ('KIT BOX'), ('INTERCOM PCB'), ('DOOR SHOE'), ('DISPLAY'), ('DOOR DRIVE'), ('INSPECTION BOX'), ('CABIN'), ('GUIDE SHOE'), ('LIGHT'), ('DOOR PCB'), ('MOTOR'), ('TRACTION ROPE'), ('HALF MIRROR'), ('PLASTIC COVER'), ('SPRING'), ('BUSH'), ('CONTACT BRIDGE'), ('CONTACT SWITCH'), ('PULLEY'), ('CAM SET'), ('LIP COMPLETE'), ('LIMIT SWITCH'), ('LOCK SET'), ('STEP'), ('INDICATOR'), ('IPM CARD'), ('INTERCOM SET'), ('STOP SWITCH'), ('PAD'), ('RING'), ('ROPE'), ('RECALL UNIT'), ('HARNESS'), ('RAM'), ('DOOR KEY'), ('BRAKE DISK'), ('SHAFT ACCESSORIES'), ('BANGING KIT'), ('EXTERNAL FAN'), ('CIRCUIT BREAKER'), ('DEM JEM'), ('RESERVATION PARK'), ('LAMP COVER'), ('OIL POT'), ('BELT'), ('BATTERY BOX'), ('MCB'), ('BOX'), ('RELAY'), ('STM PULLEY'), ('TEETH BELT'), ('COP COMPLETE'), ('DOOR SILL'), ('MAIN MACHINE'), ('BALANCING ROLLER'), ('ISOLATION PACKING'), ('BRACKET'), ('VENTILATOR FAN'), ('PANEL'), ('SEAL'), ('PHOTOCELL'), ('HANDRAIL BELT'), ('STEP CHAIN'), ('BRUSH'), ('YELLOW GUARD'), ('DOOR PANEL'), ('CAP'), ('ARD'), ('IPM PACK'), ('RECTIFIER'), ('IGBT'), ('PORT'), ('REVERSING CHAIN'), ('INSPECTION PANEL'), ('ARD CONVERTER'), ('Z LINE LOP'), ('FUSE'), ('DOOR WALL'), ('SIPS'), ('LUX'), ('CIRCLIP'), ('NEWEL CHAIN'), ('OSG SET'), ('LEVER SET'), ('PAPER'), ('TRAVELLING CABLE'), ('BOOK'), ('TAPE'), ('SAFETY SHOE'), ('HELMET'), ('APRON'), ('SALSIS'), ('CARD'), ('PLUG'), ('POWER ITEM'), ('DRILL BIT'), ('GREASE'), ('SHEET'), ('INTERCOM SPEAKER'), ('NUT BOLT'), ('SAFETY'), ('MRE PAPER'), ('BAG'), ('LIP BASE'), ('REVISION KEYBOARD'), ('RCB'), ('FRAME'), ('BASE'), ('HEADER'), ('CELLING'), ('MOTOR ENCODER'), ('WEIGHT ROPE'), ('INSPECTION SWITCH'), ('LUET SET'), ('PUSH BUTTON SET'), ('MCCB'), ('GLASS'), ('Tiels'), ('EYE BOLT'), ('PCB BOX'), ('STICKER'), ('BEARING'), ('LIFE LINE'), ('OIL'), ('RIBON'), ('O-RING'), ('ESE PANEL'), ('MNX'), ('INSULATOR'), ('CLEANER'), ('RUST CLEANER'), ('IC'), ('RCD'), ('GOGGLES'), ('COMPLETE AC'), ('FAN'), ('STM PULLEY COVER'), ('TRANSFORMER'), ('SCREW'), ('BATTERY'), ('INDICATOR PCB'), ('PLASTIC BUFFER'), ('BIODYN DRIVE'), ('SPEAKER'), ('ARD KIT'), ('BALANCING CHAIN'), ('VALVE OD'), ('MAGNETIC COIL'), ('SCRAP MATERIALS'), ('STEEL ROPE'), ('COUPLER'), ('ELECTRO MAGNET'), ('JACK'), ('LOP PLATE'), ('CLAMP'), ('WOOD BOARD'), ('JOIN KIT'), ('CAM BASE'), ('ACCESS CARD'), ('WINDOW NET'), ('BRAKE PAD'), ('AIR CUTTER'), ('SIM CARD'), ('DOOR TRACK'), ('KEYPAD'), ('PCB BASE'), ('CONVERTER'), ('CAPACITOR'), ('RESISTANCE'), ('THINER'), ('AIR FRESHNER'), ('AROSOL'), ('SUTLI'), ('POWDER'), ('PIPE'), ('LOIP COMPLETE'), ('KEY SWITCH'), ('REMOTE'), ('MAT'), ('CLIP'), ('BANNER'), ('TERMINAL'), ('MAGNETIC SENSOR'), ('WEIGHT'), ('PLASTIC WIPPE'), ('BELT GUIDE'), ('BELT CLAMP'), ('BRAKE RESISTOR'), ('O-RING SET'), ('RUBBER BUSH'), ('POWER MODULE'), ('BLOWER FAN'), ('KIT BASE'), ('MONITOR'), ('CHARGER'), ('PG CARD'), ('DIODE'), ('VARIODYN'), ('VSB DRIVE'), ('DOOR ITEMS'), ('ARD RELAY'), ('ENCODER ROPE'), ('TIMER'), ('SPRING COVER'), ('A5 CARD'), ('SADDLE'), ('PLASTIC'), ('SOCKET'), ('BED SWITCH'), ('COMPLETE LIFT'), ('WALL BOLT'), ('BLOWER MACHINE'), ('DOOR WIND'), ('GLUE'), ('TROLLEY'), ('MIRROR'), ('PRINT WGR'), ('STICKER PLATE'), ('TRANCTION ROPE');

-- Create Model Table
CREATE TABLE Model (
    Model_ID INT IDENTITY(1,1) PRIMARY KEY,
    Model NVARCHAR(50) NOT NULL UNIQUE
);
-- Insert allowed values into the Model table
INSERT INTO Model (Model)
VALUES ('COMMON MODEL'), ('5500'), ('7000'), ('3300'), ('HAUSHAHN'), ('5400'), ('S100'), ('DOOR ITEMS'), ('LIP'), ('CONTROLER'), ('CAR SILL'), ('9300'), ('2600'), ('3000'), ('COMMON'), ('3100'), ('COMMON ITEMS'), ('ELEGANT'), ('COMMOM MODEL'), ('COMMIN MODEL'), ('S001'), ('ARKEL'), ('SMART MRL'), ('S-3000'), ('Suzuki'), ('2600 (HYDROLIC)');

-- Create Control Table
CREATE TABLE Control (
    Control_ID INT IDENTITY(1,1) PRIMARY KEY,
    Control NVARCHAR(50) NOT NULL UNIQUE
);
-- Insert allowed values into the Control table
INSERT INTO Control (Control)
VALUES ('COMMON'), ('BIONIC-7'), ('MIC TX-GC'), ('SCALE ABLE'), ('BIONIC-5'), ('CUSTOM TX-GC'), ('MC2000'), ('MIC MX-GC'), ('MICONIC-SX'), ('WITTUR (SELCOM)'), ('V-30'), ('QKS-9/10'), ('SEMATIC'), ('QKS-11'), ('V-10'), ('V-35'), ('SEMATIC / V-35'), ('V-35/ FERMATOR'), ('SCALE ABLE / F3'), ('HYDRAULIC'), ('QKS-11/V-10'), ('QKS-09/10/11'), ('DO DSF9'), ('MELLER'), ('FERMATOR'), ('FERMATOR/V-35'), ('LOGOS'), ('NICE'), ('V-35 / SELCOM'), ('V-15 / V-35'), ('KST'), ('WITTUR (AGUSTA)'), ('QKS-8/9/10'), ('COMMON ITEMS'), ('MIC CX-GC'), ('V-15'), ('SEMATIC (WITTUR)'), ('WITTUR AGUSTA'), ('Miconic E'), ('SELCOM'), ('MILLER'), ('BIONIC-3'), ('SUZUKI'), ('ARL200S'), ('WITTUR (SELCOM) / V-35'), ('ARL300S'), ('V-50'), ('QSK-11'), ('BIONIC'), ('LINOVETRO'), ('FST'), ('QKS-9/10 VF / V-30'), ('QKS-11/QKS-9/10'), ('V-30/V-70'), ('FERMATOR / V-15'), ('FERMATOR / V-10'), ('LOGOS / V-35'), ('SEMATIC/SELCOM/LOGOS'), ('QKS-11/V-30/V-10/Fermator'), ('WITTUR (HYDRA PLUS)');
--Create UoMList Table
CREATE TABLE UoM (
    UoM_ID INT IDENTITY(1,1) PRIMARY KEY,
    UoM NVARCHAR(50) NOT NULL UNIQUE
);
-- Insert allowed values into the UoM table
INSERT INTO UoM (UoM)
VALUES ('set'), ('pcs'), ('pair'), ('ft'), ('m'), ('box'), ('ream'), ('pkt'), ('kg'), ('ltr'), ('coil'), ('pallet'), ('job'), ('can');

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
    Unit_Price DECIMAL(10,2),
    Stock_Quantity DECIMAL(10,2),
    UoM INT NOT NULL,

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

-- Create Requisition Table
CREATE TABLE Requisition (
    Requisition_ID INT IDENTITY(1,1) PRIMARY KEY,
    Employee_ID INT,
    Material_ID INT,
    Department_ID INT,
    Quantity INT NOT NULL,
    Status NVARCHAR(50) CHECK (Status IN ('Pending', 'Approved', 'Rejected', 'Delivered', 'Not Available')),
	Store_Status NVARCHAR(50) CHECK (Store_Status IN ('Delivered', 'Out of Stock', 'Ordered')),
    Created_Date DATETIME DEFAULT GETDATE(),
    Approved_By INT,
    FOREIGN KEY (Employee_ID) REFERENCES Employee(Employee_ID),
    FOREIGN KEY (Material_ID) REFERENCES Material(Material_ID),
    FOREIGN KEY (Department_ID) REFERENCES Department(Department_ID),
    FOREIGN KEY (Approved_By) REFERENCES Employee(Employee_ID)
);

-- Create Stock Table
CREATE TABLE Stock (
    Stock_ID INT IDENTITY(1,1) PRIMARY KEY,
    Material_ID INT NOT NULL,
    Serial_Number NVARCHAR(255) UNIQUE,
	Rack_Number NVARCHAR(50) NOT NULL,
    Shelf_Number NVARCHAR(50) NOT NULL,
	Status NVARCHAR(50) CHECK (Status IN ('Available', 'Reserved', 'Delivered', 'Warranty')) NOT NULL,
	Quantity DECIMAL(8,2) NOT NULL DEFAULT 0.00;,
    Received_Date DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (Material_ID) REFERENCES Material(Material_ID)
);

-- Create Challan Table
CREATE TABLE Challan (
    Challan_ID INT IDENTITY(1,1) PRIMARY KEY,
    Requisition_ID INT,
    Employee_ID INT,
    Delivery_Date DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(50) CHECK (Status IN ('Dispatched', 'Delivered', 'Rejected')),
    Remarks NVARCHAR(1000),
    FOREIGN KEY (Requisition_ID) REFERENCES Requisition(Requisition_ID),
    FOREIGN KEY (Employee_ID) REFERENCES Employee(Employee_ID)
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
    Total_Cost DECIMAL(18,2),
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
CREATE TABLE Return_Table (
    Return_ID INT IDENTITY(1,1) PRIMARY KEY,
    Employee_ID INT,
    Material_ID INT,
    Serial_Number NVARCHAR(255),
    Reason NVARCHAR(1000),
    Status NVARCHAR(50) CHECK (Status IN ('Received', 'Not Accepted')),
    Return_Date DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (Employee_ID) REFERENCES Employee(Employee_ID),
    FOREIGN KEY (Material_ID) REFERENCES Material(Material_ID),
    FOREIGN KEY (Serial_Number) REFERENCES Stock(Serial_Number)
);

-- Create Warranty Table
CREATE TABLE Warranty (
    Warranty_ID INT IDENTITY(1,1) PRIMARY KEY,
    Material_ID INT,
    Serial_Number NVARCHAR(255),
    Start_Date DATE,
    End_Date DATE,
    Status NVARCHAR(50) CHECK (Status IN ('Under Warranty', 'Expired')),
    FOREIGN KEY (Material_ID) REFERENCES Material(Material_ID),
    FOREIGN KEY (Serial_Number) REFERENCES Stock(Serial_Number)
);
