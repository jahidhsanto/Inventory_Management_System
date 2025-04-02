using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Role_StoreIncharge
{
    public partial class MaterialDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string materialId = Request.QueryString["Material_ID"];
                if (!string.IsNullOrEmpty(materialId))
                {
                    LoadMaterialDetails(materialId);
                    if (gvMaterialTracking != null) // Ensure GridView exists before binding
                    {
                        LoadMaterialTracking(materialId);
                    }
                }
            }
        }

        private void LoadMaterialDetails(string materialId)
        {
            string connStr = WebConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"
                                    SELECT M.Material_ID, M.Part_Id, M.Materials_Name, M.Unit_Price, M.Stock_Quantity, M.MSQ, 
                                           UoM.UoM AS UnitOfMeasure, 
                                           C.Com_Non_Com AS Com_Non_Com, 
                                           A.Asset_Status AS Asset_Status, 
                                           ATG.Asset_Type_Grouped AS Asset_Type, 
                                           Cat.Category AS Category, 
                                           SC.Sub_Category AS Sub_Category, 
                                           Mod.Model AS Model, 
                                           Cntrl.Control AS Control 
                                           --M.Material_Image  -- Added image support
                                    FROM Material M
                                    JOIN UoM ON M.UoM_ID = UoM.UoM_ID
                                    JOIN Com_Non_Com C ON M.Com_Non_Com_ID = C.Com_Non_Com_ID
                                    JOIN Asset_Status A ON M.Asset_Status_ID = A.Asset_Status_ID
                                    JOIN Asset_Type_Grouped ATG ON M.Asset_Type_Grouped_ID = ATG.Asset_Type_Grouped_ID
                                    JOIN Category Cat ON M.Category_ID = Cat.Category_ID
                                    JOIN Sub_Category SC ON M.Sub_Category_ID = SC.Sub_Category_ID
                                    JOIN Model Mod ON M.Model_ID = Mod.Model_ID
                                    JOIN Control Cntrl ON M.Control_ID = Cntrl.Control_ID
                                    WHERE M.Material_ID = @MaterialID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaterialID", materialId);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblMaterialName.Text = reader["Materials_Name"].ToString();
                                lblPartId.Text = reader["Part_Id"].ToString();
                                lblUnitPrice.Text = Convert.ToDecimal(reader["Unit_Price"]).ToString("C");

                                lblStockQuantity.Text = reader["Stock_Quantity"].ToString();
                                lblStockQuantity.Text += " " + reader["UnitOfMeasure"].ToString();

                                lblMSQ.Text = reader["MSQ"].ToString();
                                lblMSQ.Text += " " + reader["UnitOfMeasure"].ToString();

                                lblComNonCom.Text = reader["Com_Non_Com"].ToString();
                                lblAssetStatus.Text = reader["Asset_Status"].ToString();
                                lblAssetType.Text = reader["Asset_Type"].ToString();
                                lblCategory.Text = reader["Category"].ToString();
                                lblSubCategory.Text = reader["Sub_Category"].ToString();
                                lblModel.Text = reader["Model"].ToString();
                                lblControl.Text = reader["Control"].ToString();

                                // Show material image
                                //imgMaterial.ImageUrl = string.IsNullOrEmpty(reader["Material_Image"].ToString())
                                //    ? "~/Images/no-image.png" : reader["Material_Image"].ToString();

                                // Stock Progress Bar
                                decimal stockQty = Convert.ToDecimal(reader["Stock_Quantity"]);
                                decimal msq = Convert.ToDecimal(reader["MSQ"]);
                                int stockPercentage = (int)((stockQty / msq) * 100);
                                stockPercentage = stockPercentage > 100 ? 100 : stockPercentage;
                                progressStock.Attributes["style"] = $"width: {stockPercentage}%"; // ✅ Set width dynamically
                                progressStock.Attributes["class"] = stockQty > msq ? "progress-bar bg-success" :
                                                                    stockQty > 0 && stockQty <= msq ? "progress-bar bg-warning" :
                                                                    "progress-bar bg-danger";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMaterialName.Text = "Error loading material details.";
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void LoadMaterialTracking(string materialId)
        {
            string connStr = WebConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT Stock_ID, Serial_Number, Rack_Number, Shelf_Number, Status, Received_Date, Quantity
                                FROM Stock 
                                WHERE Material_ID = @MaterialID";

                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@MaterialID", materialId);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        gvMaterialTracking.DataSource = dt;
                        gvMaterialTracking.DataBind();
                    }
                    else
                    {
                        // Show message when no records found
                        dt.Rows.Add(dt.NewRow()); // Add an empty row to avoid GridView errors
                        gvMaterialTracking.DataSource = dt;
                        gvMaterialTracking.DataBind();
                        gvMaterialTracking.Rows[0].Cells.Clear();
                        gvMaterialTracking.Rows[0].Cells.Add(new TableCell { ColumnSpan = 5, Text = "No records found", HorizontalAlign = HorizontalAlign.Center });
                    }
                }
            }
        }

        protected void gvMaterialTracking_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Example: Adding a tooltip to the Status column
                e.Row.Cells[3].ToolTip = "Current status of this item";
            }
        }

    }
}