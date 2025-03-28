USE STORE;

DELETE from Requisition;
DBCC CHECKIDENT ('Requisition', RESEED, 0);

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
DBCC CHECKIDENT ('Employee', RESEED, 0);

DELETE from Department;
DBCC CHECKIDENT ('Department', RESEED, 0);



