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

namespace STORE_FINAL.Role_StoreIncharge
{
    public partial class MaterialDelivery : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //// If session key doesn't exist, create a new one
                //if (Session["DeliverySessionID"] == null)
                //{
                //    Session["DeliverySessionID"] = Guid.NewGuid().ToString();  // Unique session key
                //}

                // Get the session ID from the hidden field sent by JavaScript
                if (Request.Form["hfDeliverySessionID"] != null)
                {
                    Session["DeliverySessionID"] = Request.Form["hfDeliverySessionID"];
                }
                else if (Session["DeliverySessionID"] == null)
                {
                    Session["DeliverySessionID"] = Guid.NewGuid().ToString();
                }


                //LoadRequisitionDropdown();
                //LoadEmployeeDropdown();
                //LoadStockDropdown();
                //LoadMaterialDropdown();
                //LoadDeliveryItems();

                LoadRequisitions();
                LoadEmployees();
            }
        }

        // 1️⃣ Load Dropdowns
        private void LoadRequisitions()
        {
            string query = "SELECT Requisition_ID, CONCAT('Req#', Requisition_ID, ' - ', Status) AS ReqDetails FROM Requisition WHERE Status = 'Approved'";
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
            int requisitionID = Convert.ToInt32(ddlRequisition.SelectedValue);
            if (requisitionID > 0)
            {
                string query = @"
                                SELECT DISTINCT M.Material_ID, M.Materials_Name 
                                FROM Requisition R
                                INNER JOIN Material M ON R.Material_ID = M.Material_ID
                                WHERE R.Requisition_ID = @Requisition_ID";

                SqlParameter[] parameters = {
            new SqlParameter("@Requisition_ID", requisitionID)
        };

                DataTable dt = GetData(query, parameters);

                ddlMaterial.DataSource = dt;
                ddlMaterial.DataTextField = "Materials_Name";
                ddlMaterial.DataValueField = "Material_ID";
                ddlMaterial.DataBind();

                ddlMaterial.Items.Insert(0, new ListItem("-- Select Material --", "0"));
            }
        }
        protected void ddlMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            int materialID = Convert.ToInt32(ddlMaterial.SelectedValue);
            if (materialID > 0)
            {
                string query = @"
                                SELECT Stock_ID, 
                                       ISNULL(Serial_Number, 'No Serial') AS Serial_Number 
                                FROM Stock 
                                WHERE Material_ID = @Material_ID 
                                  AND Status = 'Available' 
                                  AND Quantity > 0";

                SqlParameter[] parameters = {
            new SqlParameter("@Material_ID", materialID)
        };

                DataTable dt = GetData(query, parameters);

                ddlStock.DataSource = dt;
                ddlStock.DataTextField = "Serial_Number";
                ddlStock.DataValueField = "Stock_ID";
                ddlStock.DataBind();

                ddlStock.Items.Insert(0, new ListItem("-- Select Stock --", "0"));
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


        // 2️⃣ Insert Selected Item into Temp_Delivery
        protected void btnAddToDelivery_Click(object sender, EventArgs e)
        {
            string sessionID = Session["DeliverySessionID"].ToString();  // Get tab-specific session ID
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
                //SqlTransaction transaction = conn.BeginTransaction();

                int challanID = 0; // ✅ Declare challanID once
                try
                {
                    //1️⃣ Insert into Challan
                    string insertChallan = "INSERT INTO Challan (Requisition_ID, Received_Employee_ID, Status, Remarks) " +
                                           "VALUES (@Requisition_ID, @Employee_ID, 'Delivered', 'All items delivered'); " +
                                           "SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd = new SqlCommand(insertChallan, conn/*, transaction*/);
                    cmd.Parameters.AddWithValue("@Requisition_ID", requisitionID);
                    cmd.Parameters.AddWithValue("@Employee_ID", employeeID);

                    object result = cmd.ExecuteScalar();
                    challanID = result != null ? Convert.ToInt32(result) : 0; // ✅ No duplicate declaration

                    // 2️⃣ Move items to Challan_Items
                    string insertItems = "INSERT INTO Challan_Items (Challan_ID, Stock_ID, Material_ID, Serial_Number, Delivered_Quantity) " +
                                         "SELECT @Challan_ID, Stock_ID, Material_ID, Serial_Number, Delivered_Quantity FROM Temp_Delivery WHERE Session_ID = @Session_ID";
                    SqlCommand cmdItems = new SqlCommand(insertItems, conn/*, transaction*/);
                    cmdItems.Parameters.AddWithValue("@Challan_ID", challanID);
                    cmdItems.Parameters.AddWithValue("@Session_ID", sessionID);
                    cmdItems.ExecuteNonQuery();

                    // 3️⃣ Update Stock
                    //new SqlCommand("UPDATE Stock SET Quantity = Quantity - Delivered_Quantity FROM Stock S JOIN Temp_Delivery TD ON S.Stock_ID = TD.Stock_ID WHERE TD.Session_ID = @Session_ID AND TD.Serial_Number IS NULL", conn, transaction).ExecuteNonQuery();
                    //new SqlCommand("UPDATE Stock SET Status = 'Delivered' WHERE Stock_ID IN (SELECT Stock_ID FROM Temp_Delivery WHERE Serial_Number IS NOT NULL AND Session_ID = @Session_ID)", conn, transaction).ExecuteNonQuery();
                    //new SqlCommand("UPDATE Stock SET Status = 'Delivered' WHERE Quantity = 0", conn, transaction).ExecuteNonQuery();

                    string updateStockWithSerial = @"
                                                    UPDATE Stock 
                                                    SET Status = 'Delivered', Quantity = Quantity - TD.Delivered_Quantity
                                                    FROM Stock S
                                                    JOIN Temp_Delivery TD ON S.Stock_ID = TD.Stock_ID
                                                    WHERE TD.Session_ID = @Session_ID AND S.Serial_Number IS NOT NULL;";
                    SqlCommand cmdUpdateStockWithSerial = new SqlCommand(updateStockWithSerial, conn/*, transaction*/);
                    cmdUpdateStockWithSerial.Parameters.AddWithValue("@Session_ID", sessionID);
                    cmdUpdateStockWithSerial.ExecuteNonQuery();

                    string updateStockWithoutSerial = @"
                                                        UPDATE Stock 
                                                        SET Quantity = Quantity - TD.Delivered_Quantity
                                                        FROM Stock S
                                                        JOIN Temp_Delivery TD ON S.Stock_ID = TD.Stock_ID
                                                        WHERE TD.Session_ID = @Session_ID AND S.Serial_Number IS NULL;";
                    SqlCommand cmdUpdateStockWithoutSerial = new SqlCommand(updateStockWithoutSerial, conn/*, transaction*/);
                    cmdUpdateStockWithoutSerial.Parameters.AddWithValue("@Session_ID", sessionID);
                    cmdUpdateStockWithoutSerial.ExecuteNonQuery();

                    string markStockAsDelivered = @"
                                                    UPDATE Stock 
                                                    SET Status = 'Delivered' 
                                                    WHERE Quantity = 0;";
                    SqlCommand cmdMarkStockAsDelivered = new SqlCommand(markStockAsDelivered, conn/*, transaction*/);
                    cmdMarkStockAsDelivered.ExecuteNonQuery();

                    // 4️⃣ Clear Temp_Delivery
                    new SqlCommand("DELETE FROM Temp_Delivery WHERE Session_ID = @Session_ID", conn/*, transaction*/).ExecuteNonQuery();

                    // 5️⃣ Reset Session Key for Next Transaction
                    Session["DeliverySessionID"] = Guid.NewGuid().ToString();

                    //transaction.Commit();
                    Response.Write("<script>alert('Delivery Successful');</script>");
                    LoadDeliveryItems();
                }
                catch
                {
                    //transaction.Rollback();
                }
            }
        }
    }
}