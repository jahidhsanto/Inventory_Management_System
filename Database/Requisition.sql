CREATE TABLE Requisition_Parent (
    Requisition_ID INT IDENTITY(1,1) PRIMARY KEY,
    CreatedByEmployee_ID INT NOT NULL, -- who created the requisition
	Created_Date DATETIME DEFAULT GETDATE(),
    
	Dept_Approved_By INT NULL,
    Dept_Approval_Date DATETIME NULL,
    Dept_Approval_Remarks NVARCHAR(500) NULL,
    Dept_Status NVARCHAR(50) CHECK (Dept_Status IN ('Rejected', 'Pending', 'Approved')) DEFAULT 'Pending',
	
	Store_Approved_By INT NULL,
	Store_Approval_Date DATETIME NULL,
	Store_Approval_Remarks NVARCHAR(500) NULL,
	Store_Status NVARCHAR(50) CHECK (Store_Status IN ('Pending', 'Out of Stock', 'Ordered', 'Delivered')),
        
    Requisition_For NVARCHAR(100) CHECK (Requisition_For IN ('Employee', 'Department', 'Zone', 'Project')),
    Employee_ID_For INT NULL,
    Department_ID_For INT NULL,
    Zone_ID_For INT NULL,
    Project_Code_For NVARCHAR(20) NULL,
    Requisition_Purpose NVARCHAR(20), -- only applicable for Project
	
    FOREIGN KEY (CreatedByEmployee_ID) REFERENCES Employee(Employee_ID),
    FOREIGN KEY (Dept_Approved_By) REFERENCES Employee(Employee_ID),
    FOREIGN KEY (Store_Approved_By) REFERENCES Employee(Employee_ID),
    FOREIGN KEY (Employee_ID_For) REFERENCES Employee(Employee_ID),
    FOREIGN KEY (Department_ID_For) REFERENCES Department(Department_ID),
    FOREIGN KEY (Zone_ID_For) REFERENCES Zone(Zone_ID),
    FOREIGN KEY (Project_Code_For) REFERENCES Project(Project_Code),

    CONSTRAINT CHK_Requisition_For_Matching_ CHECK (
        (Requisition_For = 'Employee' AND Employee_ID_For IS NOT NULL AND Department_ID_For IS NULL AND Zone_ID_For IS NULL AND Project_Code_For IS NULL AND Requisition_Purpose IS NULL) OR
        (Requisition_For = 'Department' AND Department_ID_For IS NOT NULL AND Employee_ID_For IS NULL AND Zone_ID_For IS NULL AND Project_Code_For IS NULL AND Requisition_Purpose IS NULL) OR
        (Requisition_For = 'Zone' AND Zone_ID_For IS NOT NULL AND Employee_ID_For IS NULL AND Department_ID_For IS NULL AND Project_Code_For IS NULL AND Requisition_Purpose IS NULL) OR
        (Requisition_For = 'Project' AND Project_Code_For IS NOT NULL AND Requisition_Purpose IS NOT NULL AND Employee_ID_For IS NULL AND Department_ID_For IS NULL AND Zone_ID_For IS NULL)
    )
);

CREATE TABLE Requisition_Item_Child (
    Requisition_Item_ID INT IDENTITY(1,1) PRIMARY KEY,
    Requisition_ID INT NOT NULL,
    Material_ID INT NOT NULL,
    Quantity INT NOT NULL,

    FOREIGN KEY (Requisition_ID) REFERENCES Requisition(Requisition_ID),
    FOREIGN KEY (Material_ID) REFERENCES Material(Material_ID)
);
