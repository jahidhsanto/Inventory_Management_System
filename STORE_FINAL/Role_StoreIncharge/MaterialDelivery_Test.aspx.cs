using STORE_FINAL.Pages;
using STORE_FINAL.Role_Admin;
using STORE_FINAL.Role_Employee;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Role_StoreIncharge
{
    public partial class MaterialDelivery_Test : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["DeliverySessionID"] == null)
                {
                    Session["DeliverySessionID"] = Guid.NewGuid().ToString();
                }
                hfDeliverySessionID.Value = Session["DeliverySessionID"].ToString();

                LoadRequisitions();
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
        private void LoadEmployees()
        {
            string query = "select Employee_ID, CONCAT(Employee_ID, ' - ', Name) AS Name from Employee";
            DataTable dt = GetData(query);

            ddlEmployee.DataSource = dt;
            ddlEmployee.DataTextField = "Name";
            ddlEmployee.DataValueField = "Employee_ID";
            ddlEmployee.DataBind();

            ddlEmployee.Items.Insert(0, new ListItem("-- Select Employee --", "0"));
        }
        protected void ddlRequisition_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ✅ Clear Fields when a new requisition is selected
            lblMaterialName.Text = "-";
            lblRequestedBy.Text = "-";
            lblRequestedDate.Text = "-";
            ddlSerialNumber.Items.Clear();
            ddlLocation.Items.Clear();
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
	                                    AND S.Status = 'Available'
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
                                    TD.Material_ID, 
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

        // 4️⃣ Process Final Delivery
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
            if (ddlEmployee.SelectedValue == "0")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please select an employee.');", true);
                return;
            }
            int employeeID = int.Parse(ddlEmployee.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    int challanID = 0;

                    // 3️⃣ Insert into Challan Table
                    string insertChallanQuery = @"
                                                INSERT INTO Challan (Received_Employee_ID, Status, Remarks) 
                                                VALUES (@Employee_ID, 'Delivered', 'All items delivered'); 
                                                SELECT SCOPE_IDENTITY();";

                    SqlParameter[] challanParams =
                    {
                        new SqlParameter("@Employee_ID", employeeID)
                    };

                    DataTable dtChallan = GetData(insertChallanQuery, challanParams);

                    if (dtChallan.Rows.Count > 0)
                    {
                        challanID = Convert.ToInt32(dtChallan.Rows[0][0]);
                    }

                    if (challanID == 0)
                    {
                        throw new Exception("Failed to create a new challan.");
                    }

                    // 4️⃣ Move Items from Temp_Delivery to Challan_Items
                    string insertItemsQuery = @"
                                                INSERT INTO Challan_Items (Requisition_ID, Challan_ID, Material_ID, Stock_ID, Serial_Number, Delivered_Quantity) 
                                                SELECT 
                                                    td.Requisition_ID, 
                                                    @Challan_ID, 
                                                    td.Material_ID, 
                                                    td.Stock_ID, 
                                                    CASE 
                                                        WHEN m.Requires_Serial_Number = 'Yes' THEN s.Serial_Number 
                                                        ELSE NULL 
                                                    END AS Serial_Number,
                                                    td.Delivered_Quantity 
                                                FROM Temp_Delivery td
                                                JOIN Material m ON td.Material_ID = m.Material_ID
                                                LEFT JOIN Stock_Serial s ON td.Stock_ID = s.Stock_ID -- Ensures serial is valid
                                                WHERE td.Session_ID = @Session_ID;";

                    //SqlCommand cmdInsertItems = new SqlCommand(insertItemsQuery, conn, transaction);
                    //cmdInsertItems.Parameters.AddWithValue("@Challan_ID", challanID);
                    //cmdInsertItems.Parameters.AddWithValue("@Session_ID", sessionID);
                    //cmdInsertItems.ExecuteNonQuery();


                    using (SqlCommand cmdInsertItems = new SqlCommand(insertItemsQuery, conn, transaction))
                    {
                        cmdInsertItems.Parameters.AddWithValue("@Challan_ID", challanID);
                        cmdInsertItems.Parameters.AddWithValue("@Session_ID", sessionID);

                        conn.Open();
                        cmdInsertItems.ExecuteNonQuery();
                    }










                    // 5️⃣ Update Stock Based on Serial Number
                    string updateStockWithSerial = @"
                                                    UPDATE Stock 
                                                    SET Status = 'Delivered', Quantity = Quantity - TD.Delivered_Quantity
                                                    FROM Stock S
                                                    JOIN Temp_Delivery TD ON S.Stock_ID = TD.Stock_ID
                                                    WHERE TD.Session_ID = @Session_ID AND S.Serial_Number IS NOT NULL;";

                    SqlCommand cmdUpdateStockWithSerial = new SqlCommand(updateStockWithSerial, conn, transaction);
                    cmdUpdateStockWithSerial.Parameters.AddWithValue("@Session_ID", sessionID);
                    cmdUpdateStockWithSerial.ExecuteNonQuery();

                    string updateStockWithoutSerial = @"
                                                        UPDATE Stock 
                                                        SET Quantity = Quantity - TD.Delivered_Quantity
                                                        FROM Stock S
                                                        JOIN Temp_Delivery TD ON S.Stock_ID = TD.Stock_ID
                                                        WHERE TD.Session_ID = @Session_ID AND S.Serial_Number IS NULL;";

                    SqlCommand cmdUpdateStockWithoutSerial = new SqlCommand(updateStockWithoutSerial, conn, transaction);
                    cmdUpdateStockWithoutSerial.Parameters.AddWithValue("@Session_ID", sessionID);
                    cmdUpdateStockWithoutSerial.ExecuteNonQuery();

                    string markStockAsDelivered = @"
                                                    UPDATE Stock 
                                                    SET Status = 'Delivered' 
                                                    WHERE Quantity = 0;";

                    SqlCommand cmdMarkStockAsDelivered = new SqlCommand(markStockAsDelivered, conn, transaction);
                    cmdMarkStockAsDelivered.ExecuteNonQuery();

                    // 6️⃣ Clear Temp_Delivery Table
                    string deleteTempQuery = "DELETE FROM Temp_Delivery WHERE Session_ID = @Session_ID";
                    SqlCommand cmdDeleteTemp = new SqlCommand(deleteTempQuery, conn, transaction);
                    cmdDeleteTemp.Parameters.AddWithValue("@Session_ID", sessionID);
                    cmdDeleteTemp.ExecuteNonQuery();

                    // 7️⃣ Commit Transaction
                    transaction.Commit();

                    // 8️⃣ Reset Session & Refresh UI
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