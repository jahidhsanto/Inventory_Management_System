﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using STORE_FINAL.Pages;
using WebGrease.ImageAssemble;
using STORE_FINAL.Role_Employee;

namespace STORE_FINAL.Role_StoreIncharge
{
    public partial class MaterialReceive : System.Web.UI.Page
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
                if (Session["ReceiveSessionID"] == null)
                {
                    Session["ReceiveSessionID"] = Guid.NewGuid().ToString();
                }
                hfReceiveSessionID.Value = Session["ReceiveSessionID"].ToString();

                LoadMaterialsNameID();
                LoadReceivingItems();

                // Display the DeliverySessionID in the label
                lblSession_2.Text = "Session ID: " + Session["ReceiveSessionID"].ToString();
            }
        }

        // Load DropDowns
        private void LoadMaterialsNameID()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Material_ID, Materials_Name, Part_Id FROM Material";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());

                    ddlMaterial.DataSource = dt;
                    ddlMaterial.DataTextField = "Materials_Name"; // Show Material Name
                    ddlMaterial.DataValueField = "Material_ID";  // Use Material ID as Value
                    ddlMaterial.DataBind();

                    ddlPartID.DataSource = dt;
                    ddlPartID.DataTextField = "Part_Id"; // Show Part ID
                    ddlPartID.DataValueField = "Material_ID"; // Use Material ID as Value
                    ddlPartID.DataBind();

                    // Add default items
                    ddlMaterial.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Material --", "0"));
                    ddlPartID.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Part ID --", "0"));
                }
            }
        }
        private void LoadChallans()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Challan_ID FROM Challan;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());

                    ddlChallanID.DataSource = dt;
                    ddlChallanID.DataTextField = "Challan_ID";
                    ddlChallanID.DataValueField = "Challan_ID";
                    ddlChallanID.DataBind();

                    // Add default items
                    ddlChallanID.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Challan --", "0"));

                }
            }
        }

        protected void rblReceiveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Session reset
            string sessionID = Session["ReceiveSessionID"].ToString();
            string deleteTempQuery = "DELETE FROM Temp_Receiving WHERE Session_ID = @Session_ID";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmdDeleteTemp = new SqlCommand(deleteTempQuery, conn))
                {
                    cmdDeleteTemp.Parameters.AddWithValue("@Session_ID", sessionID);
                    cmdDeleteTemp.ExecuteNonQuery();
                }

                Session["ReceiveSessionID"] = Guid.NewGuid().ToString();
                hfReceiveSessionID.Value = Session["ReceiveSessionID"].ToString();
                lblSession_2.Text = "New Session ID: " + Session["ReceiveSessionID"].ToString();

                LoadReceivingItems();
            }

            string selectedValue = rblReceiveType.SelectedValue;

            if (selectedValue == "ReturnActiveReceive" || selectedValue == "ReturnDefectiveReceive")
            {
                LoadChallans();
                ReturnAgainst.Attributes["class"] = "row"; // show

                ddlChallanItemsID.Enabled = false;
                ddlChallanItemsID.Items.Clear();
                ddlChallanItemsID.Items.Add(new ListItem("-- Select A Challan --", "0"));

                ddlMaterial.Enabled = false;
                ddlMaterial.Items.Clear();
                ddlMaterial.Items.Add(new ListItem("-- Material --", "0"));

                ddlPartID.Enabled = false;
                ddlPartID.Items.Clear();
                ddlPartID.Items.Add(new ListItem("-- Part ID --", "0"));

                txtSerialNumber.Enabled = false;
                txtSerialNumber.Text = "";
                txtQuantity.Enabled = false;
                txtQuantity.Text = "";
            }
            else
            {
                ddlChallanID.Items.Clear();
                ddlChallanItemsID.Items.Clear();
                ReturnAgainst.Attributes["class"] = "row d-none"; // hide

                ddlMaterial.Enabled = true;
                ddlPartID.Enabled = true;
                LoadMaterialsNameID();
            }
        }
        protected void ddlChallanID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string challanId = ddlChallanID.SelectedValue;

            ddlChallanItemsID.Enabled = false;

            ddlChallanItemsID.Items.Clear();
            ddlChallanItemsID.Items.Add(new ListItem("-- Select A Challan --", "0"));

            ddlMaterial.Items.Clear();
            ddlMaterial.Items.Add(new ListItem("-- Material --", "0"));

            ddlPartID.Items.Clear();
            ddlPartID.Items.Add(new ListItem("-- Part ID --", "0"));

            txtSerialNumber.Enabled = false;
            txtSerialNumber.Text = "";
            txtQuantity.Enabled = false;
            txtQuantity.Text = "";

            if (!string.IsNullOrEmpty(challanId) && challanId != "0")
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"
                                    SELECT ci.Challan_Item_ID, CONCAT(m.Materials_Name, ' - ', ci.Serial_Number) AS Item_Name
                                    FROM Challan_Items ci
                                    JOIN Material m
	                                    ON ci.Material_ID = m.Material_ID
                                    WHERE Challan_ID = @ChallanID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ChallanID", challanId);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        ddlChallanItemsID.DataSource = dt;
                        ddlChallanItemsID.DataTextField = "Item_Name";
                        ddlChallanItemsID.DataValueField = "Challan_Item_ID";
                        ddlChallanItemsID.DataBind();

                        ddlChallanItemsID.Items.Insert(0, new ListItem("-- Select Item --", "0"));

                        ddlChallanItemsID.Enabled = true;
                    }
                }
            }
        }
        protected void ddlChallanItemsID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string challanItemId = ddlChallanItemsID.SelectedValue;

            ddlMaterial.Items.Clear();
            ddlMaterial.Items.Add(new ListItem("-- Material --", "0"));

            ddlPartID.Items.Clear();
            ddlPartID.Items.Add(new ListItem("-- Part ID --", "0"));

            txtSerialNumber.Enabled = false;
            txtSerialNumber.Text = "";
            txtQuantity.Enabled = false;
            txtQuantity.Text = "";

            if (!string.IsNullOrEmpty(challanItemId) && challanItemId != "0")
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = @"
                                    select ci.Material_ID, m.Part_Id, m.Materials_Name, ci.Challan_Item_ID, ci.Serial_Number
                                    from Challan_Items ci
                                    JOIN Material m
	                                    ON ci.Material_ID = m.Material_ID
                                    where ci.Challan_Item_ID = @ChallanItemsID;";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ChallanItemsID", challanItemId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string materialId = reader["Material_ID"].ToString();
                                string partId = reader["Part_Id"].ToString();
                                string materialName = reader["Materials_Name"].ToString();
                                string serialNumber = reader["Serial_Number"].ToString();

                                ddlMaterial.Items.Add(new ListItem(materialName, materialId));
                                ddlMaterial.SelectedValue = materialId;
                                ddlPartID.Items.Add(new ListItem(partId, materialId));
                                ddlPartID.SelectedValue = materialId;

                                ShowHide_SerialQuantity();

                                txtSerialNumber.Enabled = false;
                                txtSerialNumber.Text = serialNumber;
                            }
                        }
                    }
                }
            }
        }
        protected void ddlMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlMaterial.SelectedValue != "0")
            {
                ddlPartID.SelectedValue = ddlMaterial.SelectedValue;
                ShowHide_SerialQuantity();
            }
        }
        protected void ddlPartID_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlPartID.SelectedValue != "0")
            {
                ddlMaterial.SelectedValue = ddlPartID.SelectedValue;
                ShowHide_SerialQuantity();
            }
        }

        private void ShowHide_SerialQuantity()
        {
            //string materialID = ddlMaterial.SelectedValue;

            //if (materialID != "0")
            //{
            //    if (CheckRequiredSerialNumber(materialID))
            //    {
            //        txtQuantity.Text = "1";
            //        txtQuantity.Enabled = false;

            //        txtSerialNumber.Enabled = true;
            //        txtSerialNumber.Text = "";

            //    }
            //    else
            //    {
            //        txtQuantity.Text = "0";
            //        txtQuantity.Enabled = true;
            //        txtQuantity.Attributes["required"] = "true";

            //        txtSerialNumber.Enabled = false;
            //        txtSerialNumber.Text = "";
            //    }
            //}
            //else
            //{
            //    txtSerialNumber.Enabled = false;
            //    txtSerialNumber.Text = "";
            //    txtQuantity.Enabled = false;
            //    txtQuantity.Text = "0";
            //}

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                int materialID = Convert.ToInt32(ddlMaterial.SelectedValue);

                if (materialID > 0)
                {
                    string query = "SELECT Requires_Serial_Number FROM Material WHERE Material_ID = @MaterialID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaterialID", materialID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dtMaterial = new DataTable();
                    da.Fill(dtMaterial);

                    if (dtMaterial.Rows.Count > 0)
                    {
                        bool requiresSerial = dtMaterial.Rows[0]["Requires_Serial_Number"].ToString().Equals("Yes", StringComparison.OrdinalIgnoreCase);

                        if (requiresSerial)  // Material requires a serial number
                        {
                            txtQuantity.Text = "1";
                            txtQuantity.Enabled = false;

                            txtSerialNumber.Enabled = true;
                            txtSerialNumber.Text = "";
                        }
                        else  // Material does not require a serial number
                        {
                            txtQuantity.Text = "0";
                            txtQuantity.Enabled = true;
                            txtQuantity.Attributes["required"] = "true";

                            txtSerialNumber.Enabled = false;
                            txtSerialNumber.Text = "";
                        }
                    }
                    else  // No material information found
                    {
                        txtSerialNumber.Enabled = false;
                        txtSerialNumber.Text = "";

                        txtQuantity.Enabled = true;
                        txtQuantity.Text = "0";
                        txtQuantity.Attributes.Remove("required");
                    }
                }
                else
                {
                    txtSerialNumber.Enabled = false;
                    txtSerialNumber.Text = "";
                    txtQuantity.Enabled = false;
                    txtQuantity.Text = "0";
                }
            }
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "alert alert-success" : "alert alert-danger";
            lblMessage.Visible = true;
        }

        private void ClearForm()
        {
            ddlChallanID.SelectedValue = "0";
            ddlChallanItemsID.SelectedValue = "0";
            ddlMaterial.SelectedValue = "0";
            ddlPartID.SelectedValue = "0";
            txtSerialNumber.Text = "";
            txtQuantity.Text = "";
            txtRackNumber.Text = "";
            txtShelfNumber.Text = "";
        }

        void LoadReceivingItems()
        {
            string sessionID = Session["ReceiveSessionID"].ToString();

            string query = @"
                            SELECT 
                                tr.Temp_ID, m.Materials_Name, tr.Serial_Number, tr.Quantity, tr.Rack_Number, tr.Shelf_Number 
                            FROM Temp_Receiving tr
                            JOIN Material m
	                            ON tr.Material_ID = m.Material_ID
                            WHERE tr.Session_ID = @SessionID";
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@SessionID", sessionID);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvReceivingItems.DataSource = dt;
                gvReceivingItems.DataBind();
            }
        }

        private bool CheckRequiredSerialNumber(string materialID)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Requires_Serial_Number FROM Material WHERE Material_ID = @MaterialID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaterialID", materialID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dtMaterial = new DataTable();
                da.Fill(dtMaterial);
                if (dtMaterial.Rows[0]["Requires_Serial_Number"].ToString().Equals("Yes", StringComparison.OrdinalIgnoreCase))  // Material requires a serial number
                {
                    return true;
                }
                else  // Material does not require a serial number
                {
                    return false;
                }
            }
        }

        protected void btnAddToReceiving_Click(object sender, EventArgs e)
        {
            if (Session["ReceiveSessionID"] == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Session expired. Please refresh the page.');", true);
                return;
            }

            bool requiresSerial = false;

            string sessionID = Session["ReceiveSessionID"].ToString();
            string receiveType = rblReceiveType.SelectedValue;
            string materialID = ddlMaterial.SelectedValue;
            //int requisitionID = int.Parse(ddlRequisition.SelectedValue);
            string serialNumber = txtSerialNumber.Text.Trim();
            string quantityText = txtQuantity.Text.Trim();
            string rackNumber = txtRackNumber.Text.Trim();
            string shelfNumber = txtShelfNumber.Text.Trim();
            int createdBy = int.Parse(Session["EmployeeID"].ToString());

            requiresSerial = CheckRequiredSerialNumber(materialID);

            // Validate that all required fields are filled in
            if (materialID == "0")
            {
                ShowMessage("Please select a valid Material.", false);
                return;
            }
            // Validate Serial Number only if the field is enabled (i.e., required)
            if (string.IsNullOrEmpty(serialNumber) && txtSerialNumber.Enabled)
            {
                ShowMessage("Serial Number is required for this material.", false);
                return;
            }
            if (string.IsNullOrWhiteSpace(quantityText) || !decimal.TryParse(quantityText, out decimal quantity) || quantity <= 0)
            {
                ShowMessage("Please enter a valid Quantity greater than zero.", false);
                return;
            }
            if (string.IsNullOrEmpty(rackNumber))
            {
                ShowMessage("Rack Number is required.", false);
                return;
            }
            if (string.IsNullOrEmpty(shelfNumber))
            {
                ShowMessage("Shelf Number is required.", false);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                if (requiresSerial)
                {
                    // 🔍 Fetch stock entry for this serial (if any)
                    string checkSerialQuery01 = "SELECT Availability FROM Stock WHERE Serial_Number = @Serial";
                    using (SqlCommand cmd = new SqlCommand(checkSerialQuery01, conn))
                    {
                        cmd.Parameters.AddWithValue("@Serial", serialNumber);
                        object stockStatus = cmd.ExecuteScalar();

                        // Serial must NOT exist in Stock
                        if (receiveType == "NewReceive")
                        {
                            if (stockStatus != null)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Serial number already exists in stock. Cannot receive again.');", true);
                                return;
                            }
                        }
                        // Serial must exist in Stock and be Availability = 'DELIVERED'
                        else if (receiveType == "ReturnActiveReceive" || receiveType == "ReturnDefectiveReceive")
                        {
                            if (stockStatus == null)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Serial number not found in stock. Cannot return.');", true);
                                return;
                            }
                            else if (!string.Equals(stockStatus.ToString(), "DELIVERED", StringComparison.OrdinalIgnoreCase))
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Serial number must be in DELIVERED state to receive return.');", true);
                                return;
                            }
                        }
                    }

                    // 🔍 Prevent duplicate in temp table
                    string checkSerialQuery02 = "SELECT COUNT(*) FROM Temp_Receiving WHERE Serial_Number = @Serial AND Session_ID = @Session_ID";
                    using (SqlCommand checkCmd = new SqlCommand(checkSerialQuery02, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Serial", serialNumber);
                        checkCmd.Parameters.AddWithValue("@Session_ID", sessionID);
                        int exists = (int)checkCmd.ExecuteScalar();

                        if (exists > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Serial already added in list.');", true);
                            return;
                        }
                    }

                    // Insert into Temp_Receiving
                    string insertTemp = @"INSERT INTO Temp_Receiving (Material_ID, Serial_Number, Quantity, Rack_Number, Shelf_Number, Receive_Type, Session_ID)
                                      VALUES (@Material_ID, @Serial, 1, @RackNumber, @ShelfNumber, @ReceiveType, @Session_ID)";
                    using (SqlCommand insertCmd = new SqlCommand(insertTemp, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@Material_ID", materialID);
                        insertCmd.Parameters.AddWithValue("@Serial", serialNumber);
                        insertCmd.Parameters.AddWithValue("@RackNumber", rackNumber);
                        insertCmd.Parameters.AddWithValue("@ShelfNumber", shelfNumber);
                        insertCmd.Parameters.AddWithValue("@ReceiveType", receiveType);
                        insertCmd.Parameters.AddWithValue("@Session_ID", sessionID);
                        insertCmd.ExecuteNonQuery();
                    }
                    txtSerialNumber.Text = "";
                }
                else
                {
                    // 🔄 Handle Quantity-Based Material (no serial required)

                    // 2️⃣ 
                    if (receiveType == "NewReceive")
                    {
                        // ✅ For new receives, insert into Temp_Receiving
                        string insertTemp = @"
                                        INSERT INTO Temp_Receiving (Material_ID, Quantity, Rack_Number, Shelf_Number, Receive_Type, Session_ID)
                                        VALUES (@Material_ID, @Quantity, @RackNumber, @ShelfNumber, @ReceiveType, @Session_ID)";
                        using (SqlCommand insertCmd = new SqlCommand(insertTemp, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@Material_ID", materialID);
                            insertCmd.Parameters.AddWithValue("@Quantity", quantity);
                            insertCmd.Parameters.AddWithValue("@RackNumber", rackNumber);
                            insertCmd.Parameters.AddWithValue("@ShelfNumber", shelfNumber);
                            insertCmd.Parameters.AddWithValue("@ReceiveType", receiveType);
                            insertCmd.Parameters.AddWithValue("@Session_ID", sessionID);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string challanID = ddlChallanID.SelectedValue; // Assume you have a dropdown for challans

                        // Step 1: Validate the challan item
                        string issuedQtyQuery = @"
                                    SELECT ISNULL(SUM(Quantity), 0) 
                                    FROM Challan_Items 
                                    WHERE Challan_ID = @Challan_ID AND Material_ID = @Material_ID";

                        decimal issuedQty = 0;
                        using (SqlCommand cmd = new SqlCommand(issuedQtyQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@Challan_ID", challanID);
                            cmd.Parameters.AddWithValue("@Material_ID", materialID);
                            object result = cmd.ExecuteScalar();
                            if (result != null)
                                issuedQty = Convert.ToDecimal(result);
                        }

                        if (issuedQty == 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('This material was not issued in the selected challan.');", true);
                            return;
                        }

                        // Step 2: Get already returned quantity for this material & challan
                        string returnedQtyQuery = @"
                                    SELECT ISNULL(SUM(Quantity), 0)
                                    FROM Temp_Receiving
                                    WHERE Material_ID = @Material_ID
                                      AND Challan_ID = @Challan_ID
                                      AND Session_ID = @Session_ID
                                      AND Receive_Type IN ('ReturnActiveReceive', 'ReturnDefectiveReceive')";

                        decimal alreadyReturned = 0;
                        using (SqlCommand cmdReturned = new SqlCommand(returnedQtyQuery, conn))
                        {
                            cmdReturned.Parameters.AddWithValue("@Material_ID", materialID);
                            cmdReturned.Parameters.AddWithValue("@Challan_ID", challanID);
                            cmdReturned.Parameters.AddWithValue("@Session_ID", sessionID);
                            object returned = cmdReturned.ExecuteScalar();
                            if (returned != null)
                                alreadyReturned = Convert.ToDecimal(returned);
                        }

                        // Step 3: Compare quantities
                        if ((alreadyReturned + quantity) > issuedQty)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage",
                                $"alert('Total return quantity ({alreadyReturned + quantity}) exceeds issued quantity ({issuedQty}) for this challan.');", true);
                            return;
                        }

                        // Step 4: Insert into Temp_Receiving
                        string insertTemp = @"
                                    INSERT INTO Temp_Receiving (Challan_ID, Material_ID, Quantity, Rack_Number, Shelf_Number, Session_ID, Receive_Type)
                                    VALUES (@Challan_ID, @Material_ID, @Quantity, @RackNumber, @ShelfNumber, @Session_ID, @ReceiveType)";
                        using (SqlCommand insertCmd = new SqlCommand(insertTemp, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@Challan_ID", challanID);
                            insertCmd.Parameters.AddWithValue("@Material_ID", materialID);
                            insertCmd.Parameters.AddWithValue("@Quantity", quantity);
                            insertCmd.Parameters.AddWithValue("@RackNumber", rackNumber);
                            insertCmd.Parameters.AddWithValue("@ShelfNumber", shelfNumber);
                            insertCmd.Parameters.AddWithValue("@Session_ID", sessionID);
                            insertCmd.Parameters.AddWithValue("@ReceiveType", receiveType);

                            insertCmd.ExecuteNonQuery();
                        }
                    }
                    txtQuantity.Text = "";
                }
                LoadReceivingItems();
            }
        }

        protected void gvReceivingItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                int temp_ID = Convert.ToInt32(e.CommandArgument);
                string sessionID = Session["ReceiveSessionID"].ToString();

                string deleteQuery = "DELETE FROM Temp_Receiving WHERE Temp_ID = @Temp_ID AND Session_ID = @Session_ID;";

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

                LoadReceivingItems();
            }
        }

        protected void btnAddStock_Click(object sender, EventArgs e)
        {
            try
            {
                string materialID = ddlMaterial.SelectedValue;
                string serialNumber = txtSerialNumber.Text.Trim();
                string rackNumber = txtRackNumber.Text.Trim();
                string shelfNumber = txtShelfNumber.Text.Trim();
                string status = ddlStatus.SelectedValue;
                string quantityText = txtQuantity.Text.Trim();

                // Validate that all required fields are filled in
                if (materialID == "0")
                {
                    ShowMessage("Please select a valid Material.", false);
                    return;
                }
                // Validate Serial Number only if the field is enabled (i.e., required)
                if (string.IsNullOrEmpty(serialNumber) && txtSerialNumber.Enabled)
                {
                    ShowMessage("Serial Number is required for this material.", false);
                    return;
                }
                if (string.IsNullOrEmpty(rackNumber))
                {
                    ShowMessage("Rack Number is required.", false);
                    return;
                }
                if (string.IsNullOrEmpty(shelfNumber))
                {
                    ShowMessage("Shelf Number is required.", false);
                    return;
                }
                if (status == "0")
                {
                    ShowMessage("Please select a valid Status.", false);
                    return;
                }
                if (string.IsNullOrWhiteSpace(quantityText) || !int.TryParse(quantityText, out int quantity) || quantity <= 0)
                {
                    ShowMessage("Please enter a valid Quantity greater than zero.", false);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"
                                    INSERT INTO Stock (Material_ID, Serial_Number, Rack_Number, Shelf_Number, Status, Quantity) 
                                    VALUES (@MaterialID, @SerialNumber, @RackNumber, @ShelfNumber, @Status, @Quantity)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaterialID", materialID);
                    cmd.Parameters.AddWithValue("@SerialNumber", txtSerialNumber.Enabled ? (object)serialNumber : DBNull.Value);
                    cmd.Parameters.AddWithValue("@RackNumber", rackNumber);
                    cmd.Parameters.AddWithValue("@ShelfNumber", shelfNumber);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        ShowMessage("Stock received successfully!", true);
                        ClearForm();
                    }
                    else
                    {
                        ShowMessage("Failed to add stock. Please try again.", false);
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Unique constraint error for Serial Number
                {
                    ShowMessage("Error: Serial number already exists.", false);
                }
                else
                {
                    ShowMessage("Database error: " + ex.Message, false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Unexpected error: " + ex.Message, false);
            }
        }

        protected void btnReceive_Click(object sender, EventArgs e)
        {
            // 1️⃣ Validate Session
            if (Session["ReceiveSessionID"] == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Session expired. Please refresh the page.');", true);
                return;
            }
            string sessionID = Session["ReceiveSessionID"].ToString();

            int CreatedBy_Employee_ID = int.Parse(Session["EmployeeID"].ToString());

            string receiveType = rblReceiveType.SelectedValue;

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
                        string challanType = (receiveType == "NewReceive") ? "RECEIVE" :
                             (receiveType == "ReturnActiveReceive" || receiveType == "ReturnDefectiveReceive") ?
                             "RETURN" : "INVALID_RETURN";

                        // 🛠 Insert into Challan
                        string insertChallanQuery = @"
                                                    INSERT INTO Challan (Challan_Type, Remarks) 
                                                    VALUES (@ChallanType, 'All items received'); 
                                                    SELECT SCOPE_IDENTITY();";

                        using (SqlCommand cmdChallan = new SqlCommand(insertChallanQuery, conn, transaction))
                        {
                            cmdChallan.Parameters.AddWithValue("@ChallanType", challanType);

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
                                                    INSERT INTO Challan_Items (Challan_ID, Material_ID, Serial_Number, Quantity) 
                                                    SELECT 
                                                        @Challan_ID, 
                                                        td.Material_ID, 
                                                        CASE 
                                                            WHEN m.Requires_Serial_Number = 'Yes' THEN s.Serial_Number 
                                                            ELSE NULL 
                                                        END AS Serial_Number,
                                                        td.Delivered_Quantity 
                                                    FROM Temp_Receiving td
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

                }
                catch (Exception ex)
                {
                    // Rollback transaction if anything fails
                    transaction.Rollback();
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Error processing receive: " + ex.Message + "');", true);
                }
            }
        }
    }
}