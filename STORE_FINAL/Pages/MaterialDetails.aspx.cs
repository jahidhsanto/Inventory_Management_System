using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Pages
{
	public partial class MaterialDetails : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                string materialId = Request.QueryString["Material_ID"];
                if (!string.IsNullOrEmpty(materialId))
                {
                    LoadMaterialDetails(materialId);
                    LoadMaterialTracking(materialId);
                }
            }
        }
        private void LoadMaterialDetails(string materialId)
        {
            string connStr = WebConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT Materials_Name 
                                FROM Material 
                                WHERE Material_ID = @MaterialID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaterialID", materialId);
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    lblMaterialName.Text = "Material: " + result.ToString();
                }
            }
        }

        private void LoadMaterialTracking(string materialId)
        {
            string connStr = WebConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT Serial_Number, Rack_Number, Shelf_Number, Status, Received_Date
                                FROM Stock 
                                WHERE Material_ID = @MaterialID";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@MaterialID", materialId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvMaterialTracking.DataSource = dt;
                gvMaterialTracking.DataBind();
            }
        }
    }
}