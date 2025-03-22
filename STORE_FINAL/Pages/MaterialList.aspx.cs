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
	public partial class MaterialList : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                LoadMaterials("", "");  // Default Load
            }
        }

        private void LoadMaterials(string search, string stockFilter)
        {
            string connStr = WebConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Material_ID, Materials_Name, Stock_Quantity FROM Material WHERE 1=1";

                if (!string.IsNullOrEmpty(search))
                {
                    query += " AND Materials_Name LIKE @Search";
                }
                if (stockFilter == "Available")
                {
                    query += " AND Stock_Quantity > 0";
                }
                else if (stockFilter == "Low")
                {
                    query += " AND Stock_Quantity < 5";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                if (!string.IsNullOrEmpty(search))
                {
                    cmd.Parameters.AddWithValue("@Search", "%" + search + "%");
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvMaterials.DataSource = dt;
                gvMaterials.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearchMaterialName.Text.Trim();
            string stockFilter = ddlStockFilter.SelectedValue;
            LoadMaterials(search, stockFilter);
        }

        protected void gvMaterials_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                string materialId = e.CommandArgument.ToString();
                Response.Redirect("MaterialDetails.aspx?Material_ID=" + materialId);
            }
        }
    }
}