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
	public partial class Return_Table : System.Web.UI.Page
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
                LoadReturn_Table();
            }
        }
        private void LoadReturn_Table()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
						SELECT *
						FROM Return_Table";

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
                    Return_TableGridView.DataSource = dt;
                    Return_TableGridView.DataBind();
                }
            }
        }
    }
}