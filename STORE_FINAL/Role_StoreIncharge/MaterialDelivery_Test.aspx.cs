using STORE_FINAL.Pages;
using STORE_FINAL.Role_Admin;
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
                string query = @"INSERT INTO Temp_Delivery (Stock_ID, Material_ID, Delivered_Quantity, Session_ID) 
                                VALUES (@Stock_ID, @Material_ID, @Delivered_Quantity, @Session_ID)";

                using (SqlCommand cmd = new SqlCommand(query, conn)) 
                { 
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
                string query = @"SELECT T_Di.Material_ID, S.Serial_Number, T_Di.Delivered_Quantity 
                                FROM Temp_Delivery T_Di 
                                JOIN Stock S ON T_Di.Stock_ID = S.Stock_ID 
                                WHERE Session_ID = @Session_ID;";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@Session_ID", sessionID);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvDeliveryItems.DataSource = dt;
                gvDeliveryItems.DataBind();
            }
        }



    }
}