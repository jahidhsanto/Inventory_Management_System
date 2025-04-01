USE STORE;

DELETE FROM Challan_Items;
DBCC CHECKIDENT ('Challan_Items', RESEED, 0);

DELETE FROM Challan;
DBCC CHECKIDENT ('Stock', RESEED, 0);

DELETE from Requisition;
DBCC CHECKIDENT ('Requisition', RESEED, 0);

DELETE FROM Temp_Delivery;
DBCC CHECKIDENT ('Temp_Delivery', RESEED, 0);

DELETE FROM Stock;
DBCC CHECKIDENT ('Stock', RESEED, 0);

DELETE from Material;
DBCC CHECKIDENT ('Material', RESEED, 0);

DELETE from Com_Non_Com;
DBCC CHECKIDENT ('Com_Non_Com', RESEED, 0);

DELETE from Asset_Status;
DBCC CHECKIDENT ('Asset_Status', RESEED, 0);

DELETE from Asset_Type_Grouped;
DBCC CHECKIDENT ('Asset_Type_Grouped', RESEED, 0);

DELETE from Category;
DBCC CHECKIDENT ('Category', RESEED, 0);

DELETE from Sub_Category;
DBCC CHECKIDENT ('Sub_Category', RESEED, 0);

DELETE from Model;
DBCC CHECKIDENT ('Model', RESEED, 0);

DELETE from Control;
DBCC CHECKIDENT ('Control', RESEED, 0);

DELETE from UoM;
DBCC CHECKIDENT ('UoM', RESEED, 0);

DELETE from Employee;

UPDATE Employee
SET Department_ID = NULL
DELETE from Department;
DBCC CHECKIDENT ('Department', RESEED, 0);





