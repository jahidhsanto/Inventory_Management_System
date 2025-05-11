using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using STORE_FINAL.Pages;

namespace STORE_FINAL.Role_StoreIncharge
{
    public partial class MaterialDelivery : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null || Session["Role"] == null ||
                Session["Role"].ToString() != "Store InCharge")
            {
                Response.Redirect("~/");
            }

            if (!IsPostBack)
            {
                if (Session["DeliverySessionID"] == null)
                {
                    Session["DeliverySessionID"] = Guid.NewGuid().ToString();
                }
                hfDeliverySessionID.Value = Session["DeliverySessionID"].ToString();

                LoadRequisitions();
                LoadRequisitionBy();
                LoadEmployees();

                // Display the DeliverySessionID in the label
                lblMessage.Text = "Session ID: " + Session["DeliverySessionID"].ToString();
            }
        }
        private DataTable GetData(string query, SqlParameter[] parameters = null)
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        // 1️⃣ Load Dropdowns
        private void LoadRequisitions()
        {
            string query = @"SELECT R.Requisition_ID, CONCAT('Req#', R.Requisition_ID, ' - ', M.Materials_Name, ' - ', E.Name) AS ReqDetails 
                            FROM Requisition R
                            INNER JOIN Material M ON R.Material_ID = M.Material_ID
                            JOIN Employee E ON R.Employee_ID = E.Employee_ID
                            WHERE R.Status = 'Approved' AND R.Store_Status = 'Processing';";

            DataTable dt = GetData(query);
            ddlRequisition.DataSource = dt;
            ddlRequisition.DataTextField = "ReqDetails";
            ddlRequisition.DataValueField = "Requisition_ID";
            ddlRequisition.DataBind();

            ddlRequisition.Items.Insert(0, new ListItem("-- Select Requisition --", "0"));
        }
        private void LoadRequisitionBy()
        {
            string query = @"
                            SELECT DISTINCT R.Employee_ID, CONCAT(R.Employee_ID, ' - ', E.Name) AS RequisitionBy
                            FROM Requisition R
                            JOIN Employee E ON R.Employee_ID = E.Employee_ID
                            WHERE R.Status = 'Approved' AND R.Store_Status = 'Processing'
                            ORDER BY R.Employee_ID ASC;";

            DataTable dt = GetData(query);
            ddlRequisitionBy.DataSource = dt;
            ddlRequisitionBy.DataTextField = "RequisitionBy";
            ddlRequisitionBy.DataValueField = "Employee_ID";
            ddlRequisitionBy.DataBind();

            ddlRequisitionBy.Items.Insert(0, new ListItem("-- Select Request Person --", "0"));
        }
        private void LoadEmployees()
        {
            string query = "SELECT Employee_ID, CONCAT(Employee_ID, ' - ', Name) AS Name FROM Employee";
            DataTable dt = GetData(query);

            ddlReceivedByEmployee.DataSource = dt;
            ddlReceivedByEmployee.DataTextField = "Name";
            ddlReceivedByEmployee.DataValueField = "Employee_ID";
            ddlReceivedByEmployee.DataBind();

            ddlReceivedByEmployee.Items.Insert(0, new ListItem("-- Select Employee --", "0"));
        }
        protected void ddlRequisitionBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ✅ Clear Fields when a new requisition is selected
            lblMaterialName.Text = "-";
            lblRequestedQuantity.Text = "-";
            lblRequestedBy.Text = "-";
            lblRequestedDate.Text = "-";
            ddlSerialNumber.Items.Clear();
            ddlSerialNumber.Items.Add(new ListItem("-- Select a Requisition --", "0"));
            ddlLocation.Items.Clear();
            ddlLocation.Items.Add(new ListItem("-- Select a Requisition --", "0")); 
            txtQuantity.Text = "";

            int requisitionByID = Convert.ToInt32(ddlRequisitionBy.SelectedValue);
            if (requisitionByID > 0)
            {
                string query = @"SELECT R.Requisition_ID, CONCAT('Req#', R.Requisition_ID, ' - ', M.Materials_Name, ' - ', E.Name) AS ReqDetails 
                            FROM Requisition R
                            INNER JOIN Material M ON R.Material_ID = M.Material_ID
                            JOIN Employee E ON R.Employee_ID = E.Employee_ID
                            WHERE R.Status = 'Approved' 
                                AND R.Store_Status = 'Processing' 
                                AND R.Employee_ID = @requisitionByID;";

                SqlParameter[] parameters = {
                    new SqlParameter("@requisitionByID", requisitionByID)
                };

                DataTable dt = GetData(query, parameters);
                ddlRequisition.DataSource = dt;
                ddlRequisition.DataTextField = "ReqDetails";
                ddlRequisition.DataValueField = "Requisition_ID";
                ddlRequisition.DataBind();

                ddlRequisition.Items.Insert(0, new ListItem("-- Select Requisition --", "0"));

            }
        }
        protected void ddlRequisition_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ✅ Clear Fields when a new requisition is selected
            lblMaterialName.Text = "-";
            lblRequestedQuantity.Text = "-";
            lblRequestedBy.Text = "-";
            lblRequestedDate.Text = "-";
            ddlSerialNumber.Items.Clear();
            ddlSerialNumber.Items.Add(new ListItem("-- Select Resuisition --", "0"));
            ddlLocation.Items.Clear();
            ddlLocation.Items.Add(new ListItem("-- Select Requisition --", "0")); 
            txtQuantity.Text = "";

            int requisitionID = Convert.ToInt32(ddlRequisition.SelectedValue);
            if (requisitionID > 0)
            {
                // 1️⃣ Fetch Requisition Details
                string queryRequisition = @"
                                            SELECT DISTINCT M.Material_ID, M.Materials_Name,R.Quantity,
                                                    CONCAT(e.Employee_ID, ' - ', e.Name) AS Requested_By, 
                                                    R.Created_Date, M.Requires_Serial_Number 
                                            FROM Requisition R
                                            INNER JOIN Material M ON R.Material_ID = M.Material_ID
                                            JOIN Employee e ON R.Employee_ID = e.Employee_ID
                                            WHERE R.Requisition_ID = @Requisition_ID";
                SqlParameter[] requisitionParams1 = { new SqlParameter("@Requisition_ID", requisitionID) };
                DataTable dtRequisition = GetData(queryRequisition, requisitionParams1);
                if (dtRequisition.Rows.Count > 0)
                {
                    DataRow row = dtRequisition.Rows[0]; // Since it returns only one row

                    lblMaterialName.Text = Convert.ToString(row["Materials_Name"]);
                    lblRequestedQuantity.Text = Convert.ToString(row["Quantity"]);
                    lblRequestedBy.Text = row["Requested_By"].ToString();
                    lblRequestedDate.Text = DateTime.TryParse(row["Created_Date"].ToString(), out DateTime requestedDate)
                                            ? requestedDate.ToString("dd-MMM-yyyy")
                                            : "-";
                }

                // 2️⃣ Fetch Stock Items for Selected Material / Requisition
                string queryStock = @"
                                    SELECT DISTINCT M.Material_ID, M.Materials_Name, 
                                                    S.Stock_ID, S.Serial_Number, 
                                                    CONCAT(S.Rack_Number, ' -> ', S.Shelf_Number) AS Location
                                    FROM Requisition R
                                    INNER JOIN Material M ON R.Material_ID = M.Material_ID
                                    LEFT JOIN Stock S ON R.Material_ID = S.Material_ID
                                    WHERE R.Requisition_ID = @Requisition_ID
	                                    AND S.Availability = 'AVAILABLE'
	                                    AND S.Quantity > 0;";
                SqlParameter[] requisitionParams2 = { new SqlParameter("@Requisition_ID", requisitionID) };
                DataTable dtStock = GetData(queryStock, requisitionParams2);

                ddlSerialNumber.DataSource = dtStock;
                ddlSerialNumber.DataTextField = "Serial_Number";
                ddlSerialNumber.DataValueField = "Stock_ID";
                ddlSerialNumber.DataBind();
                ddlSerialNumber.Items.Insert(0, new ListItem("-- Select Serial Number --", "0"));

                ddlLocation.DataSource = dtStock;
                ddlLocation.DataTextField = "Location";
                ddlLocation.DataValueField = "Stock_ID";
                ddlLocation.DataBind();
                ddlLocation.Items.Insert(0, new ListItem("-- Select Stock Location --", "0"));

                // 🔴 Fetch Material Information (Assuming dtMaterial contains material details)
                if (dtRequisition.Rows.Count > 0)
                {
                    bool requiresSerial = dtRequisition.Rows[0]["Requires_Serial_Number"].ToString().Equals("Yes", StringComparison.OrdinalIgnoreCase);

                    if (requiresSerial)  // Material requires a serial number
                    {
                        txtQuantity.Text = "1";
                        txtQuantity.Enabled = false;

                        //lblSerialNumber.Visible = true;
                        //ddlSerialNumber.Visible = true;
                        ddlSerialNumber.Enabled = true;
                        //lblLocation.Visible = false;
                        //ddlLocation.Visible = false;
                        ddlLocation.Enabled = false;
                    }
                    else  // Material does not require a serial number
                    {
                        txtQuantity.Text = "0";
                        txtQuantity.Enabled = true;
                        txtQuantity.Attributes["required"] = "true";

                        //lblSerialNumber.Visible = false;
                        //ddlSerialNumber.Visible = false;
                        ddlSerialNumber.Enabled = false;
                        //lblLocation.Visible = true;
                        //ddlLocation.Visible = true;
                        ddlLocation.Enabled = true;
                    }
                }
                else  // No material information found
                {
                    txtQuantity.Text = "0";
                    txtQuantity.Enabled = true;
                    txtQuantity.Attributes.Remove("required");

                    //lblSerialNumber.Visible = false;
                    //ddlSerialNumber.Visible = false;
                    ddlSerialNumber.Enabled = false;
                    //lblLocation.Visible = false;
                    //ddlLocation.Visible = false;
                    //lblLocation.Visible = false;
                    ddlLocation.Enabled = false;
                }
            }
        }
        
        // 2️⃣ Insert Selected Item into Temp_Delivery
        protected void btnAddToDelivery_Click(object sender, EventArgs e)
        {
            // Handle missing session scenario
            if (Session["DeliverySessionID"] == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Session expired. Please refresh the page.');", true);
                return;
            }

            string sessionID = Session["DeliverySessionID"].ToString();
            int requisitionID = int.Parse(ddlRequisition.SelectedValue);
            //string serialNumber = ddlSerialNumber.SelectedValue;
            int stockID = 0;

            // Check and assign stock ID depends on selection option.
            if (ddlSerialNumber.SelectedValue != "0")
            {
                stockID = int.Parse(ddlSerialNumber.SelectedValue);
            }
            else if (ddlLocation.SelectedValue != "0")
            {
                stockID = int.Parse(ddlLocation.SelectedValue);
            }
            else
            {
                // Show an error message if neither dropdown is selected
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please select either a Serial Number or a Stock Location.');", true);
                return; // Stop execution to prevent errors
            }

            // 🔍 Check if Stock item is already in the table
            string checkQuery = "SELECT COUNT(*) FROM Temp_Delivery WHERE Stock_ID = @Stock_ID AND Session_ID = @Session_ID";
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString))
            {
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Stock_ID", stockID);
                    checkCmd.Parameters.AddWithValue("@Session_ID", sessionID);

                    conn.Open();
                    int existingCount = (int)checkCmd.ExecuteScalar();
                    conn.Close();

                    if (existingCount > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('This stock item has already been added!');", true);
                        return;
                    }
                }
            }

            // Validation - Quantity
            decimal deliveredQuantity;
            if (!decimal.TryParse(txtQuantity.Text, out deliveredQuantity) || deliveredQuantity <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Invalid Quantity. Enter a valid number.');", true);
                return;
            }

            // 🔍 Fetch Material_ID based on selected Requisition
            string materialQuery = @"SELECT Material_ID FROM Requisition WHERE Requisition_ID = @Requisition_ID";
            SqlParameter[] materialParams = { new SqlParameter("@Requisition_ID", requisitionID) };
            DataTable dtMaterial = GetData(materialQuery, materialParams);

            // Handle case where no material is found for the requisition
            if (dtMaterial.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please select a Material.');", true);
                return;
            }

            int materialID = Convert.ToInt32(dtMaterial.Rows[0]["Material_ID"]);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"INSERT INTO Temp_Delivery (Requisition_ID, Stock_ID, Material_ID, Delivered_Quantity, Session_ID) 
                                VALUES (@Requisition_ID, @Stock_ID, @Material_ID, @Delivered_Quantity, @Session_ID)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Requisition_ID", requisitionID);
                    cmd.Parameters.AddWithValue("@Stock_ID", stockID);
                    cmd.Parameters.AddWithValue("@Material_ID", materialID);
                    cmd.Parameters.AddWithValue("@Delivered_Quantity", deliveredQuantity);
                    cmd.Parameters.AddWithValue("@Session_ID", sessionID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            LoadDeliveryItems();
        }

        // 3️⃣ Load Temp_Delivery Items
        void LoadDeliveryItems()
        {
            string sessionID = Session["DeliverySessionID"].ToString();  // Get fixed session key

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT 
                                    TD.Temp_ID,
                                    TD.Material_ID, 
                                    M.Part_Id, 
                                    M.Materials_Name, 
                                    CASE 
                                        WHEN M.Requires_Serial_Number = 'Yes' THEN S.Serial_Number 
                                        ELSE NULL 
                                    END AS Serial_Number, 
                                    TD.Delivered_Quantity 
                                FROM Temp_Delivery TD
                                JOIN Stock S ON TD.Stock_ID = S.Stock_ID 
                                JOIN Material M ON TD.Material_ID = M.Material_ID 
                                WHERE TD.Session_ID = @Session_ID;";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@Session_ID", sessionID);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvDeliveryItems.DataSource = dt;
                gvDeliveryItems.DataBind();
            }
        }

        // 4️⃣ Delete selected Temp_Delivery Items
        protected void gvDeliveryItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                int temp_ID = Convert.ToInt32(e.CommandArgument);
                string sessionID = Session["DeliverySessionID"].ToString();

                string deleteQuery = "DELETE FROM Temp_Delivery WHERE Temp_ID = @Temp_ID AND Session_ID = @Session_ID;";

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@Temp_ID", temp_ID);
                        deleteCmd.Parameters.AddWithValue("@Session_ID", sessionID);

                        conn.Open();
                        deleteCmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }

                LoadDeliveryItems();
            }
        }

        // 5️⃣ Process Final Delivery        
        protected void btnDeliver_Click(object sender, EventArgs e)
        {
            // 1️⃣ Validate Session
            if (Session["DeliverySessionID"] == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Session expired. Please refresh the page.');", true);
                return;
            }
            string sessionID = Session["DeliverySessionID"].ToString();

            // 2️⃣ Validate Required Selections
            if (ddlReceivedByEmployee.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please select an employee.');", true);
                return;
            }
            int ReceivedBy_Employee_ID = int.Parse(ddlReceivedByEmployee.SelectedValue);
            int CreatedBy_Employee_ID = int.Parse(Session["EmployeeID"].ToString());

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    decimal challanID = 0;

                    // 🔥 Check if user selected to create challan
                    // bool createChallan = chkCreateChallan.Checked; 
                    bool createChallan = true;
                    
                    // 3️⃣
                    if (createChallan)
                    {
                        // 🛠 Insert into Challan
                        string insertChallanQuery = @"
                                                    INSERT INTO Challan (Challan_Type, Remarks) 
                                                    VALUES ('DELIVERY', 'All items delivered'); 
                                                    SELECT SCOPE_IDENTITY();";

                        using (SqlCommand cmdChallan = new SqlCommand(insertChallanQuery, conn, transaction))
                        {
                            object result = cmdChallan.ExecuteScalar();
                            if (result != null && decimal.TryParse(result.ToString(), out decimal newChallanID))
                            {
                                challanID = newChallanID;
                            }
                        }

                        if (challanID <= 0)
                            throw new Exception("Failed to create a new Challan.");

                        // 🛠 Insert into Challan_Items
                        string insertItemsQuery = @"
                                                    INSERT INTO Challan_Items (Challan_ID, Requisition_ID, Material_ID, Stock_ID, Serial_Number, Quantity) 
                                                    SELECT 
                                                        @Challan_ID, 
                                                        td.Requisition_ID, 
                                                        td.Material_ID, 
                                                        td.Stock_ID, 
                                                        CASE 
                                                            WHEN m.Requires_Serial_Number = 'Yes' THEN s.Serial_Number 
                                                            ELSE NULL 
                                                        END AS Serial_Number,
                                                        td.Delivered_Quantity 
                                                    FROM Temp_Delivery td
                                                    JOIN Material m ON td.Material_ID = m.Material_ID
                                                    LEFT JOIN Stock s ON td.Stock_ID = s.Stock_ID 
                                                    WHERE td.Session_ID = @Session_ID;";

                        using (SqlCommand cmdInsertItems = new SqlCommand(insertItemsQuery, conn, transaction))
                        {
                            cmdInsertItems.Parameters.AddWithValue("@Challan_ID", challanID);
                            cmdInsertItems.Parameters.AddWithValue("@Session_ID", sessionID);
                            cmdInsertItems.ExecuteNonQuery();
                        }
                    }

                    // 4️⃣ Insert into Material_Ledger
                    string insertLedgerQuery = @"
										WITH LastBalance AS (
											SELECT
												Material_ID,
												MAX(Ledger_ID) AS Last_Ledger_ID
											FROM Material_Ledger
											GROUP BY Material_ID
										),
										PreviousBalance AS (
											SELECT
												ml.Material_ID,
												ml.Balance_After_Transaction AS Previous_Balance,
												ml.Valuation_After_Transaction AS Previous_Valuation
											FROM Material_Ledger ml
											JOIN LastBalance lb ON ml.Ledger_ID = lb.Last_Ledger_ID
										),
										ChallanData AS (
											SELECT
												c.Challan_ID,
												c.Challan_Date,
												c.Challan_Type,
												ci.Material_ID,
												ci.Quantity,
												m.Unit_Price,
												ISNULL(pb.Previous_Balance, 0) AS Previous_Balance,
												ISNULL(pb.Previous_Valuation, 0) AS Previous_Valuation
											FROM Challan c
											JOIN Challan_Items ci ON ci.Challan_ID = c.Challan_ID
											JOIN Material m ON m.Material_ID = ci.Material_ID
											LEFT JOIN PreviousBalance pb ON pb.Material_ID = ci.Material_ID
											WHERE c.Challan_ID = @Challan_ID
										),
										GroupedData AS (
											SELECT
												Material_ID,
												MAX(Challan_ID) AS Challan_ID,
												MAX(Challan_Date) AS Challan_Date,
												MAX(Challan_Type) AS Challan_Type,
												MAX(Unit_Price) AS Unit_Price,
												MAX(Previous_Balance) AS Previous_Balance,
												MAX(Previous_Valuation) AS Previous_Valuation,
												SUM(CASE WHEN Challan_Type IN ('RETURN', 'RECEIVE') THEN Quantity ELSE 0 END) AS In_Quantity,
												SUM(CASE WHEN Challan_Type = 'DELIVERY' THEN Quantity ELSE 0 END) AS Out_Quantity
											FROM ChallanData
											GROUP BY Material_ID
										)
										INSERT INTO Material_Ledger (
											Material_ID,
											Challan_ID,
											Challan_Date,
											Ledger_Type,
											In_Quantity,
    										Out_Quantity,
    										Unit_Price,
											Balance_After_Transaction,
											Valuation_After_Transaction
										)

										SELECT
											Material_ID,
											Challan_ID,
											Challan_Date,
											Challan_Type,
											In_Quantity,
											Out_Quantity,
											Unit_Price,
											Previous_Balance + In_Quantity - Out_Quantity AS New_Balance,
											(Previous_Balance + In_Quantity - Out_Quantity) * Unit_Price AS New_Valuation
										FROM GroupedData;";

                    using (SqlCommand cmdinsertLedger = new SqlCommand(insertLedgerQuery, conn, transaction))
                    {
                        cmdinsertLedger.Parameters.AddWithValue("@Challan_ID", challanID);
                        cmdinsertLedger.ExecuteNonQuery();
                    }

                    // 5️⃣ Insert into Material_Transaction_Log
                    string insertTransactionLogQuery = @"
                                                        INSERT INTO Material_Transaction_Log (
                                                            Material_ID, Stock_ID, Serial_Number, Transaction_Type, Transaction_Date,
                                                            Out_Quantity, Challan_ID, Requisition_ID, ReceivedBy_Employee_ID, Remarks, CreatedBy_Employee_ID
                                                        )
                                                        SELECT 
                                                            td.Material_ID,
                                                            td.Stock_ID,
                                                            CASE 
                                                                WHEN m.Requires_Serial_Number = 'Yes' THEN s.Serial_Number 
                                                                ELSE NULL 
                                                            END AS Serial_Number,
                                                            'DELIVERY',
                                                            GETDATE(),
                                                            td.Delivered_Quantity,
                                                            @Challan_ID,
                                                            td.Requisition_ID,
                                                            @ReceivedBy_Employee_ID,
                                                            'Delivered material',
                                                            @CreatedBy_Employee_ID
                                                        FROM Temp_Delivery td
                                                        JOIN Material m ON td.Material_ID = m.Material_ID
                                                        LEFT JOIN Stock s ON td.Stock_ID = s.Stock_ID
                                                        WHERE td.Session_ID = @Session_ID;";
                    using (SqlCommand cmdInsertLog = new SqlCommand(insertTransactionLogQuery, conn, transaction))
                    {
                        cmdInsertLog.Parameters.AddWithValue("@Challan_ID", challanID);
                        cmdInsertLog.Parameters.AddWithValue("@ReceivedBy_Employee_ID", ReceivedBy_Employee_ID);
                        cmdInsertLog.Parameters.AddWithValue("@CreatedBy_Employee_ID", CreatedBy_Employee_ID);
                        cmdInsertLog.Parameters.AddWithValue("@Session_ID", sessionID);
                        cmdInsertLog.ExecuteNonQuery();
                    }

                    // 6️⃣ Update Stock Based on Serial Number
                    string updateStockWithSerial = @"
                                                    UPDATE Stock 
                                                    SET Availability = 'DELIVERED', Quantity = Quantity - TD.Delivered_Quantity
                                                    FROM Stock S
                                                    JOIN Temp_Delivery TD ON S.Stock_ID = TD.Stock_ID
                                                    WHERE TD.Session_ID = @Session_ID AND S.Serial_Number IS NOT NULL;";

                    using (SqlCommand cmdUpdateStockWithSerial = new SqlCommand(updateStockWithSerial, conn, transaction))
                    {
                        cmdUpdateStockWithSerial.Parameters.AddWithValue("@Session_ID", sessionID);
                        cmdUpdateStockWithSerial.ExecuteNonQuery();
                    }

                    string updateStockWithoutSerial = @"
                                                        UPDATE Stock 
                                                        SET Quantity = Quantity - TD.Delivered_Quantity
                                                        FROM Stock S
                                                        JOIN Temp_Delivery TD ON S.Stock_ID = TD.Stock_ID
                                                        WHERE TD.Session_ID = @Session_ID AND S.Serial_Number IS NULL;";

                    using (SqlCommand cmdUpdateStockWithoutSerial = new SqlCommand(updateStockWithoutSerial, conn, transaction))
                    {
                        cmdUpdateStockWithoutSerial.Parameters.AddWithValue("@Session_ID", sessionID);
                        cmdUpdateStockWithoutSerial.ExecuteNonQuery();
                    }

                    // 7️⃣ Mark delivered if Quantity = 0
                    string markStockAsDelivered = @"
                                                    UPDATE Stock 
                                                    SET Availability = 'DELIVERED' 
                                                    WHERE Quantity = 0 OR Quantity < 0;";

                    using (SqlCommand cmdMarkStockAsDelivered = new SqlCommand(markStockAsDelivered, conn, transaction))
                    {
                        cmdMarkStockAsDelivered.ExecuteNonQuery();
                    }

                    // 8️⃣ Update Requisition Status
                    string updateRequisitionStatus = @"
                                                        UPDATE Requisition 
                                                        SET Store_Status = 'Delivered' 
                                                        WHERE Requisition_ID IN (
                                                            SELECT Requisition_ID 
                                                            FROM Temp_Delivery 
                                                            WHERE Session_ID = @Session_ID
                                                        );";

                    using (SqlCommand cmdUpdateReq = new SqlCommand(updateRequisitionStatus, conn, transaction))
                    {
                        cmdUpdateReq.Parameters.AddWithValue("@Session_ID", sessionID);
                        cmdUpdateReq.ExecuteNonQuery();
                    }

                    // 9️⃣ Clear Temp_Delivery Table
                    string deleteTempQuery = "DELETE FROM Temp_Delivery WHERE Session_ID = @Session_ID";
                    using (SqlCommand cmdDeleteTemp = new SqlCommand(deleteTempQuery, conn, transaction))
                    {
                        cmdDeleteTemp.Parameters.AddWithValue("@Session_ID", sessionID);
                        cmdDeleteTemp.ExecuteNonQuery();
                    }

                    // 🔟 Commit Transaction
                    transaction.Commit();

                    // 1️⃣1️⃣ Reset Session & Refresh UI
                    Session["DeliverySessionID"] = Guid.NewGuid().ToString();
                    hfDeliverySessionID.Value = Session["DeliverySessionID"].ToString();
                    lblMessage.Text = "New Session ID: " + Session["DeliverySessionID"].ToString();

                    // Reload UI Elements
                    LoadRequisitions();
                    LoadEmployees();
                    gvDeliveryItems.DataSource = null;
                    gvDeliveryItems.DataBind();

                    // ✅ Success Message
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Delivery processed successfully!');", true);
                }
                catch (Exception ex)
                {
                    // Rollback transaction if anything fails
                    transaction.Rollback();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Error processing delivery: " + ex.Message + "');", true);
                }
            }
        }
    }
}