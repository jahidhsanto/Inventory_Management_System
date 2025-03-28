using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Role_Admin
{
	public partial class Departments : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            // Ensure only admins can access this page
            if (Session["Username"] == null || Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/");
            }

            if (!IsPostBack)
            {
                LoadDepartment();
            }
        }

        private void LoadDepartment()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT d.DepartmentName, d.Department_Head_ID, e.Name, e.Designation
                                FROM Department d
                                JOIN 
	                                Employee e ON d.Department_Head_ID = e.Employee_ID;";

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
                    DepartmentGridView.DataSource = dt;
                    DepartmentGridView.DataBind();
                }
            }
        }
    }
}