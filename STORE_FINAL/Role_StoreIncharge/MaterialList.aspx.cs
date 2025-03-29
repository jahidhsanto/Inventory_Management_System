using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Role_StoreIncharge
{
    public partial class MaterialList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null || Session["Role"] == null ||
                Session["Role"].ToString() != "Store InCharge")
            {
                Response.Redirect("~/");
            }

            if (!IsPostBack)
            {
                LoadMaterials("", "", "", "", "", "", "", "", "", ""); // Default Load

                // Load all dropdowns
                LoeadCom_Non_Com();
                LoeadAsset_Status();
                LoeadAsset_Type_Grouped();
                LoeadCategory();
                LoeadSub_Category();
                LoeadModel();
                LoeadControl();
                LoadUoM();
            }

        }

        private void LoadMaterials(string searchPartID, string searchMaterialName, string filterCom_Non_Com, string filterAsset_Status, string filterAsset_Type_Grouped, string filterCategory, string filterSub_Category, string filterControl, string filterModel, string filterStock)
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
                                    uom.UoM,
                                    m.MSQ
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
                if (!string.IsNullOrEmpty(filterStock))
                {
                    query += " AND Stock_Quantity > 0";
                }

                query += " ORDER BY m.Material_ID DESC";  // Sorting by Material_ID in descending order

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
                        cmd.Parameters.AddWithValue("@FilterCom_Non_Com", filterCom_Non_Com);
                    }
                    if (!string.IsNullOrEmpty(filterAsset_Status))
                    {
                        cmd.Parameters.AddWithValue("@FilterAsset_Status", filterAsset_Status);
                    }
                    if (!string.IsNullOrEmpty(filterAsset_Type_Grouped))
                    {
                        cmd.Parameters.AddWithValue("@FilterAsset_Type_Grouped", filterAsset_Type_Grouped);
                    }
                    if (!string.IsNullOrEmpty(filterCategory))
                    {
                        cmd.Parameters.AddWithValue("@FilterCategory", filterCategory);
                    }
                    if (!string.IsNullOrEmpty(filterSub_Category))
                    {
                        cmd.Parameters.AddWithValue("@FilterSub_Category", filterSub_Category);
                    }
                    if (!string.IsNullOrEmpty(filterControl))
                    {
                        cmd.Parameters.AddWithValue("@FilterControl", filterControl);
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
            string filterStock = ddlStockFilter.SelectedValue;

            LoadMaterials(searchPartID, searchMaterialName, filterCom_Non_Com, filterAsset_Status, filterAsset_Type_Grouped, filterCategory, filterSub_Category, filterControl, filterModel, filterStock);
        }

        protected void gvMaterials_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                string materialId = e.CommandArgument.ToString();
                Response.Redirect("MaterialDetail.aspx?Material_ID=" + materialId);
            }
        }

        private void LoeadCom_Non_Com()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                select Com_Non_Com_ID, Com_Non_Com 
                                From Com_Non_Com;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);  // Load data into a DataTable

                ddlCom_Non_Com.DataSource = dt;
                ddlCom_Non_Com.DataTextField = "Com_Non_Com";
                ddlCom_Non_Com.DataValueField = "Com_Non_Com";
                ddlCom_Non_Com.DataBind();

                ddlCom_NonCom.DataSource = dt;
                ddlCom_NonCom.DataTextField = "Com_Non_Com";
                ddlCom_NonCom.DataValueField = "Com_Non_Com_ID";
                ddlCom_NonCom.DataBind();
            }

            ddlCom_Non_Com.Items.Insert(0, new ListItem("-- Select Com_Non_Com --", ""));
            ddlCom_NonCom.Items.Insert(0, new ListItem("-- Select Com_Non_Com --", ""));
        }

        private void LoeadAsset_Status()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                select Asset_Status_ID, Asset_Status 
                                From Asset_Status;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);  // Load data into a DataTable

                ddlAsset_Status.DataSource = dt;
                ddlAsset_Status.DataTextField = "Asset_Status";
                ddlAsset_Status.DataValueField = "Asset_Status";
                ddlAsset_Status.DataBind();

                ddlAssetStatus.DataSource = dt;
                ddlAssetStatus.DataTextField = "Asset_Status";
                ddlAssetStatus.DataValueField = "Asset_Status_ID";
                ddlAssetStatus.DataBind();
            }

            ddlAsset_Status.Items.Insert(0, new ListItem("-- Select Asset Status --", ""));
            ddlAssetStatus.Items.Insert(0, new ListItem("-- Select Asset Status --", ""));
        }

        private void LoeadAsset_Type_Grouped()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                select Asset_Type_Grouped_ID, Asset_Type_Grouped 
                                From Asset_Type_Grouped;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);  // Load data into a DataTable

                ddlAsset_Type_Grouped.DataSource = dt;
                ddlAsset_Type_Grouped.DataTextField = "Asset_Type_Grouped";
                ddlAsset_Type_Grouped.DataValueField = "Asset_Type_Grouped";
                ddlAsset_Type_Grouped.DataBind();

                ddlAssetType.DataSource = dt;
                ddlAssetType.DataTextField = "Asset_Type_Grouped";
                ddlAssetType.DataValueField = "Asset_Type_Grouped_ID";
                ddlAssetType.DataBind();
            }

            ddlAsset_Type_Grouped.Items.Insert(0, new ListItem("-- Select Asset Type Grouped --", ""));
            ddlAssetType.Items.Insert(0, new ListItem("-- Select Asset Type --", ""));
        }

        private void LoeadCategory()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                select Category_ID,  Category
                                From Category;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);  // Load data into a DataTable

                ddl_Category.DataSource = dt;
                ddl_Category.DataTextField = "Category";
                ddl_Category.DataValueField = "Category";
                ddl_Category.DataBind();

                ddlCategory.DataSource = dt;
                ddlCategory.DataTextField = "Category";
                ddlCategory.DataValueField = "Category_ID";
                ddlCategory.DataBind();
            }

            ddl_Category.Items.Insert(0, new ListItem("-- Select Category --", ""));
            ddlCategory.Items.Insert(0, new ListItem("-- Select Category --", ""));
        }

        private void LoeadSub_Category()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                select Sub_Category_ID, Sub_Category
                                From Sub_Category;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);  // Load data into a DataTable

                ddlSub_Category.DataSource = dt;
                ddlSub_Category.DataTextField = "Sub_Category";
                ddlSub_Category.DataValueField = "Sub_Category";
                ddlSub_Category.DataBind();

                ddlSubCategory.DataSource = dt;
                ddlSubCategory.DataTextField = "Sub_Category";
                ddlSubCategory.DataValueField = "Sub_Category_ID";
                ddlSubCategory.DataBind();
            }

            ddlSub_Category.Items.Insert(0, new ListItem("-- Select Sub Category --", ""));
            ddlSubCategory.Items.Insert(0, new ListItem("-- Select Sub Category --", ""));
        }

        private void LoeadModel()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT Model_ID, Model
                                From Model;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);  // Load data into a DataTable

                ddl_Model.DataSource = dt;
                ddl_Model.DataTextField = "Model";
                ddl_Model.DataValueField = "Model";
                ddl_Model.DataBind();

                ddlModel.DataSource = dt;
                ddlModel.DataTextField = "Model";
                ddlModel.DataValueField = "Model_ID";
                ddlModel.DataBind();
            }

            ddl_Model.Items.Insert(0, new ListItem("-- Select Model --", ""));
            ddlModel.Items.Insert(0, new ListItem("-- Select Model --", ""));
        }

        private void LoeadControl()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                select Control_ID, Control
                                From Control;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);  // Load data into a DataTable

                ddl_Control.DataSource = dt;
                ddl_Control.DataTextField = "Control";
                ddl_Control.DataValueField = "Control";
                ddl_Control.DataBind();

                ddlControl.DataSource = dt;
                ddlControl.DataTextField = "Control";
                ddlControl.DataValueField = "Control_ID";
                ddlControl.DataBind();
            }

            ddl_Control.Items.Insert(0, new ListItem("-- Select Control --", ""));
            ddlControl.Items.Insert(0, new ListItem("-- Select Control --", ""));
        }

        private void LoadUoM()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT UoM_ID, UoM
                                FROM UoM;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlUoM.DataSource = reader;
                ddlUoM.DataTextField = "UoM";
                ddlUoM.DataValueField = "UoM_ID";
                ddlUoM.DataBind();
            }

            ddlUoM.Items.Insert(0, new ListItem("-- Select UoM --", "0"));
        }

        protected void btnAddMaterial_Click(object sender, EventArgs e)
        {
            String com_NonCom = ddlCom_NonCom.SelectedValue;
            String assetStatus = ddlAssetStatus.SelectedValue;
            String assetType = ddlAssetType.SelectedValue;
            String category = ddlCategory.SelectedValue;
            String subCategory = ddlSubCategory.SelectedValue;
            String model = ddlModel.SelectedValue;
            String control = ddlControl.SelectedValue;
            String materialName = txtMaterialName.Text.Trim();
            String part_Id = txtPart_Id.Text.Trim();
            String unitPriceText = txtUnitPrice.Text.Trim();
            String MSQText = txtMSQ.Text.Trim();
            String uom = ddlUoM.SelectedValue;

            decimal unitPrice;
            decimal MSQ;

            // Validation
            if (string.IsNullOrEmpty(materialName) ||
                string.IsNullOrEmpty(category) ||
                string.IsNullOrEmpty(unitPriceText) ||
                string.IsNullOrEmpty(MSQText))
            {
                ShowMessage("Please fill in all required fields.", false);
                return;
            }

            if (com_NonCom == "0" || assetStatus == "0" ||
                assetType == "0" || category == "0" ||
                subCategory == "0" || model == "0" ||
                control == "0" || uom == "0")
            {
                ShowMessage("Please select all required options.", false);
                return;
            }


            // Check if unitPrice is a valid decimal value
            if (!decimal.TryParse(unitPriceText, out unitPrice) || unitPrice <= 0)
            {
                ShowMessage("Please enter a valid positive value for the unit price.", false);
                return;
            }
            
            // Check if MSQ is a valid decimal value
            if (!decimal.TryParse(MSQText, out MSQ) || unitPrice <= 0)
            {
                ShowMessage("Please enter a valid positive value for the MSQ.", false);
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"INSERT INTO Material (Part_Id, Materials_Name, Unit_Price, MSQ, Stock_Quantity, Com_Non_Com_ID, Asset_Status_ID, Asset_Type_Grouped_ID, Category_ID, Sub_Category_ID, Model_ID, Control_ID, UoM_ID) 
                                 VALUES (@part_Id, @materialName, @unitPrice, @MSQ, @stockQuantity, @com_NonCom, @assetStatus, @assetType, @category, @subCategory, @model, @control, @uom)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@part_Id", part_Id);
                cmd.Parameters.AddWithValue("@materialName", materialName);
                cmd.Parameters.AddWithValue("@unitPrice", unitPrice);
                cmd.Parameters.AddWithValue("@MSQ", MSQ);
                cmd.Parameters.AddWithValue("@stockQuantity", "0");
                cmd.Parameters.AddWithValue("@com_NonCom", com_NonCom);
                cmd.Parameters.AddWithValue("@assetStatus", assetStatus);
                cmd.Parameters.AddWithValue("@assetType", assetType);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@subCategory", subCategory);
                cmd.Parameters.AddWithValue("@model", model);
                cmd.Parameters.AddWithValue("@control", control);
                cmd.Parameters.AddWithValue("@uom", uom);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    ShowMessage("Material added successfully!", true);
                    ClearFields();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error: " + ex.Message, false);
                }
            }
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "alert alert-success" : "alert alert-danger";
            lblMessage.Visible = true;
        }

        private void ClearFields()
        {
            txtMaterialName.Text = "";
            txtPart_Id.Text = "";
            txtUnitPrice.Text = "";
            txtMSQ.Text = "";
            ddlCom_NonCom.SelectedIndex = 0;
            ddlAssetStatus.SelectedIndex = 0;
            ddlAssetType.SelectedIndex = 0;
            ddlCategory.SelectedIndex = 0;
            ddlSubCategory.SelectedIndex = 0;
            ddlModel.SelectedIndex = 0;
            ddlControl.SelectedIndex = 0;    
            ddlUoM.SelectedIndex = 0;
        }
    }
}