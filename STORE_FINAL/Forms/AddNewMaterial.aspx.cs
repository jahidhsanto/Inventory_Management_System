using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Forms
{
	public partial class AddNewMaterial : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}
        protected void btnAddMaterial_Click(object sender, EventArgs e)
        {
            string materialName = txtMaterialName.Text.Trim();
            string description = txtDescription.Text.Trim();
            string category = txtCategory.Text.Trim();
            decimal unitPrice;
            int stockQuantity;
            string rackNumber = txtRackNumber.Text.Trim();
            string shelfNumber = txtShelfNumber.Text.Trim();

            // Validation
            if (string.IsNullOrEmpty(materialName) || string.IsNullOrEmpty(category) ||
                !decimal.TryParse(txtUnitPrice.Text, out unitPrice) ||
                !int.TryParse(txtStockQuantity.Text, out stockQuantity))
            {
                lblMessage.Text = "Please enter valid values for all required fields.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "INSERT INTO Material (Name, Description, Category, Unit_Price, Stock_Quantity, Rack_Number, Shelf_Number) " +
                               "VALUES (@Name, @Description, @Category, @UnitPrice, @StockQuantity, @RackNumber, @ShelfNumber)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", materialName);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                cmd.Parameters.AddWithValue("@StockQuantity", stockQuantity);
                cmd.Parameters.AddWithValue("@RackNumber", rackNumber);
                cmd.Parameters.AddWithValue("@ShelfNumber", shelfNumber);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    lblMessage.Text = "Material added successfully!";
                    ClearFields();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }
        }

        private void ClearFields()
        {
            txtMaterialName.Text = "";
            txtDescription.Text = "";
            txtCategory.Text = "";
            txtUnitPrice.Text = "";
            txtStockQuantity.Text = "";
            txtRackNumber.Text = "";
            txtShelfNumber.Text = "";
        }

    }
}