using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Pages
{
    public partial class LocationDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string rack_Number = Request.QueryString["Rack_Number"];
                if (!string.IsNullOrEmpty(rack_Number))
                {
                    LoadLocationDetails(rack_Number);
                    LoadLocationTracking(rack_Number);
                }

            }
        }

        private void LoadLocationDetails(string rack_Number)
        {
            string connStr = WebConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT rack_number 
                                FROM stock 
                                WHERE Rack_Number = @RackNumber
                                group by rack_number";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@RackNumber", rack_Number);
                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    lblMaterialName.Text = "Material: " + result.ToString();
                }
            }
        }

        private void LoadLocationTracking(string rack_Number)
        {
            string connStr = WebConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                select s.Material_ID, m.Part_Id, m.Materials_Name, s.Shelf_number, Count(*) as Quantity
                                from stock s
                                left join Material m
                                on s.Material_ID = m.Material_ID
                                Where s.Rack_Number = @RackNumber
                                group by s.Material_ID, m.Part_Id, m.Materials_Name, s.Shelf_number;";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@RackNumber", rack_Number);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvMaterialTracking.DataSource = dt;
                gvMaterialTracking.DataBind();
            }
        }
    }
}