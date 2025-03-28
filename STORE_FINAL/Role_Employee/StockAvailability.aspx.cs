using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Role_Employee
{
	public partial class StockAvailability : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            // Ensure only admins can access this page
            if (Session["Username"] == null || Session["Role"] == null || 
                Session["Role"].ToString() != "Employee" && Session["Role"].ToString() != "Department Head")
            {
                Response.Redirect("~/");
            }

            if (!IsPostBack)
            {
                LoadStock();
            }
        }

        private void LoadStock()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                        select 
	                        m.Material_ID, 	m.Materials_Name, 
	                        M.Part_Id, 	m.Stock_Quantity, 
	                        c.Category,	m.Unit_Price, 
	                        Model.model, sub_c.Sub_Category,
	                        Control.Control,
	                        CASE 
		                        WHEN m.Stock_Quantity > m.MSQ THEN 'In Stock'
		                        WHEN m.Stock_Quantity <= m.MSQ THEN 'Low Stock'
		                        ELSE 'Out of Stock'
	                        END AS Stock_Status
                        FROM Material m
                        JOIN
	                        Category c ON m.Category_ID = c.Category_ID
                        JOIN
	                        Sub_Category sub_c ON m.Category_ID = sub_c.Sub_Category_ID
                        JOIN
	                        Asset_Status a_s ON m.Asset_Status_ID = a_s.Asset_Status_ID
                        JOIN
	                        Model ON m.Model_ID = Model.Model_ID
                        JOIN
	                        Control ON m.Control_ID = Control.Control_ID
                        WHERE a_s.Asset_Status = 'READY STOCK'
                        ORDER BY M.Material_ID DESC;";

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
                    StockAvailabilityGridView.DataSource = dt;
                    StockAvailabilityGridView.DataBind();
                }
            }
        }
    }
}