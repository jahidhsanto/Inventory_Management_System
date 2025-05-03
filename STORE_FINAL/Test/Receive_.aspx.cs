using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Test
{
    public partial class Receive_ : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMaterials();
            }
        }
        private void LoadMaterials()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT Material_ID, Materials_Name FROM Material", conn);
                conn.Open();
                ddlMaterial.DataSource = cmd.ExecuteReader();
                ddlMaterial.DataTextField = "Materials_Name";
                ddlMaterial.DataValueField = "Material_ID";
                ddlMaterial.DataBind();
                ddlMaterial.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", ""));
            }
        }

        protected void ddlMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Optionally load if Requires_Serial_Number is Yes
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int materialId = int.Parse(ddlMaterial.SelectedValue);
            string receiveType = ddlReceiveType.SelectedValue;
            int quantity = int.Parse(txtQuantity.Text.Trim());
            string[] serialNumbers = txtSerialNumbers.Text.Trim().Split(',');

            bool requiresSerial = false;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Check if material requires serial number
                SqlCommand checkSerialCmd = new SqlCommand("SELECT Requires_Serial_Number FROM Material WHERE Material_ID = @MaterialID", conn);
                checkSerialCmd.Parameters.AddWithValue("@MaterialID", materialId);
                string requires = (string)checkSerialCmd.ExecuteScalar();
                requiresSerial = (requires == "Yes");

                if (requiresSerial && serialNumbers.Length != quantity)
                {
                    lblMessage.Text = "Serial number count must match quantity.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    // Insert Challan
                    SqlCommand challanCmd = new SqlCommand("INSERT INTO Challan (Challan_Type, Remarks) OUTPUT INSERTED.Challan_ID VALUES (@Type, @Remarks)", conn, trans);
                    challanCmd.Parameters.AddWithValue("@Type", receiveType.StartsWith("RETURN") ? "RETURN" : "RECEIVE");
                    challanCmd.Parameters.AddWithValue("@Remarks", "Received via Web Form");
                    int challanId = (int)challanCmd.ExecuteScalar();

                    string status = "ACTIVE";
                    if (receiveType == "RETURN_DEFECTIVE")
                        status = "DEFECTIVE";

                    // Loop for inserting
                    for (int i = 0; i < quantity; i++)
                    {
                        string serial = requiresSerial ? serialNumbers[i].Trim() : null;

                        // Insert Stock
                        SqlCommand stockCmd = new SqlCommand(@"
                            INSERT INTO Stock (Material_ID, Serial_Number, Status, Availability, Quantity)
                            OUTPUT INSERTED.Stock_ID
                            VALUES (@MaterialID, @Serial, @Status, 'AVAILABLE', @Qty)", conn, trans);

                        stockCmd.Parameters.AddWithValue("@MaterialID", materialId);
                        stockCmd.Parameters.AddWithValue("@Serial", (object)serial ?? DBNull.Value);
                        stockCmd.Parameters.AddWithValue("@Status", status);
                        stockCmd.Parameters.AddWithValue("@Qty", requiresSerial ? 1 : quantity);

                        int stockId = (int)stockCmd.ExecuteScalar();

                        // Insert Challan_Item
                        SqlCommand itemCmd = new SqlCommand(@"
                            INSERT INTO Challan_Items (Challan_ID, Material_ID, Stock_ID, Serial_Number, Quantity)
                            VALUES (@ChallanID, @MaterialID, @StockID, @Serial, @Qty)", conn, trans);

                        itemCmd.Parameters.AddWithValue("@ChallanID", challanId);
                        itemCmd.Parameters.AddWithValue("@MaterialID", materialId);
                        itemCmd.Parameters.AddWithValue("@StockID", stockId);
                        itemCmd.Parameters.AddWithValue("@Serial", (object)serial ?? DBNull.Value);
                        itemCmd.Parameters.AddWithValue("@Qty", requiresSerial ? 1 : quantity);

                        itemCmd.ExecuteNonQuery();

                        if (!requiresSerial) break; // Avoid multiple loops if not required
                    }

                    trans.Commit();
                    lblMessage.Text = "Material received successfully.";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}