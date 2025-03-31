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
    public partial class MaterialReceive : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null || Session["Role"] == null ||
                Session["Role"].ToString() != "Store InCharge")
            {
                Response.Redirect("~/");
            }

            if (!IsPostBack)
            {
                LoadMaterials();
            }
        }

        private void LoadMaterials()
        {
            string connString = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT Material_ID, Materials_Name, Part_Id FROM Material";
                SqlCommand cmd = new SqlCommand(query, conn);
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

        protected void ddlMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMaterial.SelectedValue != "0")
            {
                ddlPartID.SelectedValue = ddlMaterial.SelectedValue;
            }
        }

        protected void ddlPartID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPartID.SelectedValue != "0")
            {
                ddlMaterial.SelectedValue = ddlPartID.SelectedValue;
            }
        }

        protected void btnAddStock_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
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
                if (string.IsNullOrEmpty(serialNumber))
                {
                    ShowMessage("Serial Number is required.", false);
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
                if (string.IsNullOrEmpty(quantityText) || !int.TryParse(quantityText, out int quantity) || quantity <= 0)
                {
                    ShowMessage("Please enter a valid quantity.", false);
                    return;
                }

                string connString = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = @"
                                    INSERT INTO Stock (Material_ID, Serial_Number, Rack_Number, Shelf_Number, Status, Quantity, Received_Date) 
                                    VALUES (@MaterialID, @SerialNumber, @RackNumber, @ShelfNumber, @Status, @Quantity, GETDATE())";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaterialID", materialID);
                    cmd.Parameters.AddWithValue("@SerialNumber", serialNumber);
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

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "alert alert-success" : "alert alert-danger";
            lblMessage.Visible = true;
        }

        private void ClearForm()
        {
            txtSerialNumber.Text = "";
            txtRackNumber.Text = "";
            txtShelfNumber.Text = "";
            ddlMaterial.SelectedValue = "0";
            ddlPartID.SelectedValue = "0";
            ddlStatus.SelectedIndex = 0;
        }

    }
}