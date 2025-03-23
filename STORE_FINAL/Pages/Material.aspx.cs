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
                LoadMaterials("", "", "", "", "", "", "", "", ""); // Default Load

                // Load all dropdowns
                LoeadCom_Non_Com();
                LoeadAsset_Status();
                LoeadAsset_Type_Grouped();
                LoeadCategory();
                LoeadSub_Category();
                LoeadModel();
                LoeadControl();
            }
        }

        private void LoadMaterials(string searchPartID, string searchMaterialName, string filterCom_Non_Com, string filterAsset_Status, string filterAsset_Type_Grouped, string filterCategory, string filterSub_Category, string filterControl, string filterModel)
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
                                    uom.UoM
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
                                    Control ctl ON m.Control_ID = ctl.Control_ID
                                JOIN
	                                UoM uom ON m.UoM_ID = uom.UoM_ID";

                if (!string.IsNullOrEmpty(searchPartID))
                {
                    query += " AND Part_Id LIKE @SearchPartID";
                }
                if (!string.IsNullOrEmpty(searchMaterialName))
                {
                    query += " AND Materials_Name LIKE @SearchMaterialName";
                }
                if (!string.IsNullOrEmpty(filterCom_Non_Com))
                {
                    query += " AND Com_Non_Com LIKE @FilterCom_Non_Com";
                }
                if (!string.IsNullOrEmpty(filterAsset_Status))
                {
                    query += " AND Asset_Status LIKE @FilterAsset_Status";
                }
                if (!string.IsNullOrEmpty(filterAsset_Type_Grouped))
                {
                    query += " AND Asset_Type_Grouped LIKE @FilterAsset_Type_Grouped";
                }
                if (!string.IsNullOrEmpty(filterCategory))
                {
                    query += " AND Category LIKE @FilterCategory";
                }
                if (!string.IsNullOrEmpty(filterSub_Category))
                {
                    query += " AND Sub_Category LIKE @FilterSub_Category";
                }
                if (!string.IsNullOrEmpty(filterControl))
                {
                    query += " AND Control LIKE @FilterControl";
                }
                if (!string.IsNullOrEmpty(filterModel))
                {
                    query += " AND Model LIKE @FilterModel";
                }



                //if (stockFilter == "Available")
                //{
                //    query += " AND Stock_Quantity > 0";
                //}
                //else if (stockFilter == "Low")
                //{
                //    query += " AND Stock_Quantity < 5";
                //}


                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(searchPartID))
                    {
                        cmd.Parameters.AddWithValue("SearchPartID", "%" + searchPartID + "%");
                    }
                    if (!string.IsNullOrEmpty(searchMaterialName))
                    {
                        cmd.Parameters.AddWithValue("@SearchMaterialName", "%" + searchMaterialName + "%");
                    }
                    if (!string.IsNullOrEmpty(filterCom_Non_Com))
                    {
                        cmd.Parameters.AddWithValue("@FilterCom_Non_Com", "%" + filterCom_Non_Com + "%");
                    }
                    if (!string.IsNullOrEmpty(filterAsset_Status))
                    {
                        cmd.Parameters.AddWithValue("@FilterAsset_Status", "%" + filterAsset_Status + "%");
                    }
                    if (!string.IsNullOrEmpty(filterAsset_Type_Grouped))
                    {
                        cmd.Parameters.AddWithValue("@FilterAsset_Type_Grouped", "%" + filterAsset_Type_Grouped + "%");
                    }
                    if (!string.IsNullOrEmpty(filterCategory))
                    {
                        cmd.Parameters.AddWithValue("@FilterCategory", "%" + filterCategory + "%");
                    }
                    if (!string.IsNullOrEmpty(filterSub_Category))
                    {
                        cmd.Parameters.AddWithValue("@FilterSub_Category", "%" + filterSub_Category + "%");
                    }
                    if (!string.IsNullOrEmpty(filterControl))
                    {
                        cmd.Parameters.AddWithValue("@FilterControl", "%" + filterControl + "%");
                    }
                    if (!string.IsNullOrEmpty(filterModel))
                    {
                        cmd.Parameters.AddWithValue("@FilterModel", "%" + filterModel + "%");
                    }

                    conn.Open();
                    //Run SQL & store in da
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    //Create DataTable for store output
                    DataTable dt = new DataTable();

                    //Send loaded DataTable to frontend
                    da.Fill(dt);
                    //start binding data with frontend
                    gvMaterials.DataSource = dt;
                    gvMaterials.DataBind();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchPartID = txtSearchPartID.Text.Trim();
            string searchMaterialName = txtSearchMaterialName.Text.Trim();

            string filterCom_Non_Com = ddlCom_Non_Com.SelectedValue;
            string filterAsset_Status = ddlAsset_Status.SelectedValue;
            string filterAsset_Type_Grouped = ddlAsset_Type_Grouped.SelectedValue;
            string filterCategory = ddlCategory.SelectedValue;
            string filterSub_Category = ddlSub_Category.SelectedValue;
            string filterModel = ddlModel.SelectedValue;
            string filterControl = ddlControl.SelectedValue;

            LoadMaterials(searchPartID, searchMaterialName, filterCom_Non_Com, filterAsset_Status, filterAsset_Type_Grouped, filterCategory, filterSub_Category, filterControl, filterModel);
        }

        protected void gvMaterials_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                string materialId = e.CommandArgument.ToString();
                Response.Redirect("MaterialDetails.aspx?Material_ID=" + materialId);
            }
        }

        private void LoeadCom_Non_Com()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                select Com_Non_Com 
                                From Com_Non_Com;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlCom_Non_Com.DataSource = reader;
                ddlCom_Non_Com.DataTextField = "Com_Non_Com";
                ddlCom_Non_Com.DataValueField = "Com_Non_Com";
                ddlCom_Non_Com.DataBind();
            }

            ddlCom_Non_Com.Items.Insert(0, new ListItem("-- Select Com_Non_Com --", ""));
        }

        private void LoeadAsset_Status()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                select Asset_Status 
                                From Asset_Status;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlAsset_Status.DataSource = reader;
                ddlAsset_Status.DataTextField = "Asset_Status";
                ddlAsset_Status.DataValueField = "Asset_Status";
                ddlAsset_Status.DataBind();
            }

            ddlAsset_Status.Items.Insert(0, new ListItem("-- Select Asset Status --", ""));
        }

        private void LoeadAsset_Type_Grouped()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                select Asset_Type_Grouped 
                                From Asset_Type_Grouped;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlAsset_Type_Grouped.DataSource = reader;
                ddlAsset_Type_Grouped.DataTextField = "Asset_Type_Grouped";
                ddlAsset_Type_Grouped.DataValueField = "Asset_Type_Grouped";
                ddlAsset_Type_Grouped.DataBind();
            }

            ddlAsset_Type_Grouped.Items.Insert(0, new ListItem("-- Select Asset Type Grouped --", ""));
        }

        private void LoeadCategory()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                select Category 
                                From Category;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlCategory.DataSource = reader;
                ddlCategory.DataTextField = "Category";
                ddlCategory.DataValueField = "Category";
                ddlCategory.DataBind();
            }

            ddlCategory.Items.Insert(0, new ListItem("-- Select Category --", ""));
        }

        private void LoeadSub_Category()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                select Sub_Category 
                                From Sub_Category;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlSub_Category.DataSource = reader;
                ddlSub_Category.DataTextField = "Sub_Category";
                ddlSub_Category.DataValueField = "Sub_Category";
                ddlSub_Category.DataBind();
            }

            ddlSub_Category.Items.Insert(0, new ListItem("-- Select Sub Category --", ""));
        }

        private void LoeadModel()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                select Model 
                                From Model;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlModel.DataSource = reader;
                ddlModel.DataTextField = "Model";
                ddlModel.DataValueField = "Model";
                ddlModel.DataBind();
            }

            ddlModel.Items.Insert(0, new ListItem("-- Select Model --", ""));
        }

        private void LoeadControl()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                select Control 
                                From Control;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlControl.DataSource = reader;
                ddlControl.DataTextField = "Control";
                ddlControl.DataValueField = "Control";
                ddlControl.DataBind();
            }

            ddlControl.Items.Insert(0, new ListItem("-- Select Control --", ""));
        }

    }
}