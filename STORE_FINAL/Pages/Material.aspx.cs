using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Pages
{
	public partial class Material : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            // Check if the session values exist, otherwise redirect to login page
            if (Session["Username"] == null)
            {
                Response.Redirect("~/");
            }

            if (!IsPostBack)
            {
                LoadMaterial();
            }
        }
        private void LoadMaterial()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
						SELECT 
                            m.Material_ID,
                            m.Part_Id,
                            cn.Com_Non_Com,
                            asst.Asset_Status,
                            atg.Asset_Type_Grouped,
                            cat.Category,
                            subc.Sub_Category,
                            md.Model,
                            ctl.Control,
                            m.Materials_Name,
                            m.Unit_Price,
                            m.Stock_Quantity,
                            m.Rack_Number,
                            m.Shelf_Number
                        FROM 
                            Material m
                        JOIN 
                            Com_Non_Com cn ON m.Com_Non_Com_ID = cn.Com_Non_Com_ID
                        JOIN 
                            Asset_Status asst ON m.Asset_Status_ID = asst.Asset_Status_ID
                        JOIN 
                            Asset_Type_Grouped atg ON m.Asset_Type_Grouped_ID = atg.Asset_Type_Grouped_ID
                        JOIN 
                            Category cat ON m.Category_ID = cat.Category_ID
                        JOIN 
                            Sub_Category subc ON m.Sub_Category_ID = subc.Sub_Category_ID
                        JOIN 
                            Model md ON m.Model_ID = md.Model_ID
                        JOIN 
                            Control ctl ON m.Control_ID = ctl.Control_ID;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    //Run SQL & store in da
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    //Create DataTable for store output
                    DataTable dt = new DataTable();

                    //Send loaded DataTable to frontend
                    da.Fill(dt);
                    //start binding data with frontend
                    MaterialGridView.DataSource = dt;
                    MaterialGridView.DataBind();
                }
            }
        }
    }
}