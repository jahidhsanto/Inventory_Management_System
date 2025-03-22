using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace STORE_FINAL.Pages
{
    public partial class LocationReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRacks();
                LoadShelfs();
                LoadLocationReport();
            }
        }

        private void LoadRacks()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT Rack_Number
                                FROM Stock
                                GROUP BY rack_number;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlRack.DataSource = reader;
                ddlRack.DataTextField = "Rack_Number";
                ddlRack.DataValueField = "Rack_Number";
                ddlRack.DataBind();
            }

            ddlRack.Items.Insert(0, new ListItem("-- Select Rack --", "0"));
        }

        private void LoadShelfs()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT Shelf_Number
                                FROM Stock
                                GROUP BY Shelf_Number;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlShelf.DataSource = reader;
                ddlShelf.DataTextField = "Shelf_Number";
                ddlShelf.DataValueField = "Shelf_Number";
                ddlShelf.DataBind();
            }

            ddlShelf.Items.Insert(0, new ListItem("-- Select Shelf --", "0"));
        }

        private void LoadLocationReport()
        {
            string connStr = WebConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT Rack_Number, Shelf_Number, COUNT(*) AS Total_Stock 
                                FROM Material_Tracking 
                                GROUP BY Rack_Number, Shelf_Number 
                                ORDER BY Rack_Number, Shelf_Number";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvLocationReport.DataSource = dt;
                gvLocationReport.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //string searchPartID = txtSearchPartID.Text.Trim();
            //string searchMaterialName = txtSearchMaterialName.Text.Trim();
            string ddlRackSearch = ddlRack.SelectedValue;
            string ddlShelfSearch = ddlShelf.SelectedValue;
            LoadLocations(ddlRackSearch, ddlShelfSearch);
        }

        private void LoadLocations(string ddlRackSearch, string ddlShelfSearch)
        {
            string connStr = WebConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Start the query with a WHERE clause that will always be valid.
                string query = @"
                        SELECT Rack_Number, Shelf_Number
                        FROM Stock
                        WHERE 1=1"; // This ensures the query structure is always valid.

                // Dynamically add the filters if values are provided.
                if (!string.IsNullOrEmpty(ddlRackSearch) && ddlRackSearch != "0")
                {
                    query += " AND Rack_Number LIKE @RackSearch";
                }

                if (!string.IsNullOrEmpty(ddlShelfSearch) && ddlShelfSearch != "0")
                {
                    query += " AND Shelf_Number LIKE @ShelfSearch";
                }

                query += " GROUP BY Rack_Number, Shelf_Number";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters if filters are applied.
                    if (!string.IsNullOrEmpty(ddlRackSearch) && ddlRackSearch != "0")
                    {
                        cmd.Parameters.AddWithValue("@RackSearch", "%" + ddlRackSearch + "%");
                    }

                    if (!string.IsNullOrEmpty(ddlShelfSearch) && ddlShelfSearch != "0")
                    {
                        cmd.Parameters.AddWithValue("@ShelfSearch", "%" + ddlShelfSearch + "%");
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Bind the results to the GridView
                    gvLocationReport.DataSource = dt;
                    gvLocationReport.DataBind();
                }
            }
        }

        protected void gvLocationReport_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                string Rack_Number = e.CommandArgument.ToString();
                Response.Redirect("LocationDetails.aspx?Rack_Number=" + Rack_Number);
            }
        }

    }
}