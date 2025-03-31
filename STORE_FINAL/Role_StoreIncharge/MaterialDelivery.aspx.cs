using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Role_StoreIncharge
{
    public partial class MaterialDelivery : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // If session key doesn't exist, create a new one
                if (Session["DeliverySessionID"] == null)
                {
                    Session["DeliverySessionID"] = Guid.NewGuid().ToString();  // Unique session key
                }

                LoadRequisitionDropdown();
                LoadEmployeeDropdown();
                LoadStockDropdown();
                LoadMaterialDropdown();
                LoadDeliveryItems();
            }
        }

        // 1️⃣ Load Dropdowns
        void LoadRequisitionDropdown()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                                

                string query = "SELECT DISTINCT r.Requisition_ID, CONCAT(r.Requisition_ID, ' - ', m.Materials_Name) AS Materials_Name FROM Requisition r JOIN Material m ON r.Material_ID = m.Material_ID WHERE Store_Status = 'Processing';";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                ddlRequisition.DataSource = dt;
                ddlRequisition.DataTextField = "Materials_Name";
                ddlRequisition.DataValueField = "Requisition_ID";
                ddlRequisition.DataBind();

                ddlRequisition.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Requisition --", "0"));
            }
        }
        void LoadEmployeeDropdown()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "select Employee_ID, Name, CONCAT(Employee_ID, ' - ', Name) AS List from Employee;";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                ddlEmployee.DataSource = dt;
                ddlEmployee.DataTextField = "List";
                ddlEmployee.DataValueField = "Employee_ID";
                ddlEmployee.DataBind();

                ddlEmployee.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Employee --", "0"));
            }
        }
        void LoadStockDropdown()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "select Stock_ID from Stock;";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                ddlStock.DataSource = dt;
                ddlStock.DataTextField = "Stock_ID";
                ddlStock.DataValueField = "Stock_ID";
                ddlStock.DataBind();

                ddlStock.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Stock ID --", "0"));
            }
        }
        void LoadMaterialDropdown()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "select Material_ID, Materials_Name from Material;";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                ddlMaterial.DataSource = dt;
                ddlMaterial.DataTextField = "Materials_Name";
                ddlMaterial.DataValueField = "Material_ID";
                ddlMaterial.DataBind();

                ddlMaterial.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Material --", "0"));
            }
        }

        // 2️⃣ Insert Selected Item into Temp_Delivery
        protected void btnAddToDelivery_Click(object sender, EventArgs e)
        {
            string sessionID = Session["DeliverySessionID"].ToString();  // Get fixed session key
            //string sessionID = Session.SessionID;
            int stockID = int.Parse(ddlStock.SelectedValue);
            int materialID = int.Parse(ddlMaterial.SelectedValue);
            string serialNumber = txtSerialNumber.Text.Trim();
            decimal deliveredQuantity = decimal.Parse(txtQuantity.Text);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "INSERT INTO Temp_Delivery (Stock_ID, Material_ID, Serial_Number, Delivered_Quantity, Session_ID) " +
                               "VALUES (@Stock_ID, @Material_ID, @Serial_Number, @Delivered_Quantity, @Session_ID)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Stock_ID", stockID);
                cmd.Parameters.AddWithValue("@Material_ID", materialID);
                cmd.Parameters.AddWithValue("@Serial_Number", (object)serialNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Delivered_Quantity", deliveredQuantity);
                cmd.Parameters.AddWithValue("@Session_ID", sessionID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadDeliveryItems();
        }

        // 3️⃣ Load Temp_Delivery Items
        void LoadDeliveryItems()
        {
            string sessionID = Session["DeliverySessionID"].ToString();  // Get fixed session key

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Temp_Delivery WHERE Session_ID = @Session_ID";
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
            string sessionID = Session["DeliverySessionID"].ToString();
            int requisitionID = int.Parse(ddlRequisition.SelectedValue);
            int employeeID = int.Parse(ddlEmployee.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1️⃣ Insert into Challan
                    string insertChallan = "INSERT INTO Challan (Requisition_ID, Received_Employee_ID, Status, Remarks) " +
                                           "VALUES (@Requisition_ID, @Employee_ID, 'Delivered', 'All items delivered'); " +
                                           "SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd = new SqlCommand(insertChallan, conn, transaction);
                    cmd.Parameters.AddWithValue("@Requisition_ID", requisitionID);
                    cmd.Parameters.AddWithValue("@Employee_ID", employeeID);
                    int challanID = Convert.ToInt32(cmd.ExecuteScalar());

                    // 2️⃣ Move items to Challan_Items
                    string insertItems = "INSERT INTO Challan_Items (Challan_ID, Stock_ID, Material_ID, Serial_Number, Delivered_Quantity) " +
                                         "SELECT @Challan_ID, Stock_ID, Material_ID, Serial_Number, Delivered_Quantity FROM Temp_Delivery WHERE Session_ID = @Session_ID";
                    SqlCommand cmdItems = new SqlCommand(insertItems, conn, transaction);
                    cmdItems.Parameters.AddWithValue("@Challan_ID", challanID);
                    cmdItems.Parameters.AddWithValue("@Session_ID", sessionID);
                    cmdItems.ExecuteNonQuery();

                    // 3️⃣ Update Stock
                    new SqlCommand("UPDATE Stock SET Quantity = Quantity - Delivered_Quantity FROM Stock S JOIN Temp_Delivery TD ON S.Stock_ID = TD.Stock_ID WHERE TD.Session_ID = @Session_ID AND TD.Serial_Number IS NULL", conn, transaction).ExecuteNonQuery();
                    new SqlCommand("UPDATE Stock SET Status = 'Delivered' WHERE Stock_ID IN (SELECT Stock_ID FROM Temp_Delivery WHERE Serial_Number IS NOT NULL AND Session_ID = @Session_ID)", conn, transaction).ExecuteNonQuery();
                    new SqlCommand("UPDATE Stock SET Status = 'Delivered' WHERE Quantity = 0", conn, transaction).ExecuteNonQuery();

                    // 4️⃣ Clear Temp_Delivery
                    new SqlCommand("DELETE FROM Temp_Delivery WHERE Session_ID = @Session_ID", conn, transaction).ExecuteNonQuery();

                    // 5️⃣ Reset Session Key for Next Transaction
                    Session["DeliverySessionID"] = Guid.NewGuid().ToString();

                    transaction.Commit();
                    Response.Write("<script>alert('Delivery Successful');</script>");
                    LoadDeliveryItems();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }
    }
}