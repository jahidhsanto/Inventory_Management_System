using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Forms
{
	public partial class AddNewMaterial : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                LoadCom_NonCom();
                LoadAssetStatus();
                LoadAssetType();
                LoadCategory();
                LoadSubCategory();
                LoadModel();
                LoadControl();
            }
		}

        private void LoadCom_NonCom()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT Com_Non_Com_ID, Com_Non_Com
                                 FROM Com_Non_Com;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlCom_NonCom.DataSource = reader;
                ddlCom_NonCom.DataTextField = "Com_Non_Com";
                ddlCom_NonCom.DataValueField = "Com_Non_Com_ID";
                ddlCom_NonCom.DataBind();
            }

            ddlCom_NonCom.Items.Insert(0, new ListItem("-- Select Option --", "0"));
        }

        private void LoadAssetStatus()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT Asset_Status_ID, Asset_Status
                                 FROM Asset_Status;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlAssetStatus.DataSource = reader;
                ddlAssetStatus.DataTextField = "Asset_Status";
                ddlAssetStatus.DataValueField = "Asset_Status_ID";
                ddlAssetStatus.DataBind();
            }

            ddlAssetStatus.Items.Insert(0, new ListItem("-- Select Option --", "0"));
        }

        private void LoadAssetType()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT Asset_Type_Grouped_ID, Asset_Type_Grouped
                                 FROM Asset_Type_Grouped;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlAssetType.DataSource = reader;
                ddlAssetType.DataTextField = "Asset_Type_Grouped";
                ddlAssetType.DataValueField = "Asset_Type_Grouped_ID";
                ddlAssetType.DataBind();
            }

            ddlAssetType.Items.Insert(0, new ListItem("-- Select Option --", "0"));
        }

        private void LoadCategory()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT Category_ID,  Category
                                 FROM Category;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlCategory.DataSource = reader;
                ddlCategory.DataTextField = "Category";
                ddlCategory.DataValueField = "Category_ID";
                ddlCategory.DataBind();
            }

            ddlCategory.Items.Insert(0, new ListItem("-- Select Option --", "0"));
        }

        private void LoadSubCategory()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT Sub_Category_ID, Sub_Category
                                 FROM Sub_Category;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlSubCategory.DataSource = reader;
                ddlSubCategory.DataTextField = "Sub_Category";
                ddlSubCategory.DataValueField = "Sub_Category_ID";
                ddlSubCategory.DataBind();
            }

            ddlSubCategory.Items.Insert(0, new ListItem("-- Select Option --", "0"));
        }

        private void LoadModel()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT Model_ID, Model
                                 FROM Model;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlModel.DataSource = reader;
                ddlModel.DataTextField = "Model";
                ddlModel.DataValueField = "Model_ID";
                ddlModel.DataBind();
            }

            ddlModel.Items.Insert(0, new ListItem("-- Select Option --", "0"));
        }

        private void LoadControl()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT Control_ID, Control
                                 FROM Control;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlControl.DataSource = reader;
                ddlControl.DataTextField = "Control";
                ddlControl.DataValueField = "Control_ID";
                ddlControl.DataBind();
            }

            ddlControl.Items.Insert(0, new ListItem("-- Select Option --", "0"));
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
            String stockQuantityText = txtStockQuantity.Text.Trim();
            String rackNumber = txtRackNumber.Text.Trim();
            String shelfNumber = txtShelfNumber.Text.Trim();

            decimal unitPrice;
            decimal stockQuantity;

            // Validation
            if (string.IsNullOrEmpty(materialName) ||
                string.IsNullOrEmpty(category) ||
                string.IsNullOrEmpty(unitPriceText) ||
                string.IsNullOrEmpty(stockQuantityText))
            {
                lblMessage.Text = "Please fill in all required fields.";
                return;
            }

            if (com_NonCom == "0" || assetStatus == "0" || 
                assetType == "0" || category == "0" || 
                subCategory == "0" || model == "0" || control == "0")
            {
                lblMessage.Text = "Please select all required options.";
                return;
            }


            // Check if unitPrice is a valid decimal value
            if (!decimal.TryParse(unitPriceText, out unitPrice) || unitPrice <= 0)
            {
                lblMessage.Text = "Please enter a valid positive value for the unit price.";
                return;
            }

            // Check if stockQuantity is a valid integer value
            if (!decimal.TryParse(stockQuantityText, out stockQuantity) || stockQuantity < 0)
            {
                lblMessage.Text = "Please enter a valid non-negative value for stock quantity.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"INSERT INTO Material (Part_Id, Materials_Name, Unit_Price, Stock_Quantity, Rack_Number, Shelf_Number, Com_Non_Com_ID, Asset_Status_ID, Asset_Type_Grouped_ID, Category_ID, Sub_Category_ID, Model_ID, Control_ID) 
                                 VALUES (@part_Id, @materialName, @unitPrice, @stockQuantity, @rackNumber, @shelfNumber, @com_NonCom, @assetStatus, @assetType, @category, @subCategory, @model, @control)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@part_Id", part_Id);
                cmd.Parameters.AddWithValue("@materialName", materialName);
                cmd.Parameters.AddWithValue("@unitPrice", unitPrice);
                cmd.Parameters.AddWithValue("@stockQuantity", stockQuantity);
                cmd.Parameters.AddWithValue("@rackNumber", rackNumber);
                cmd.Parameters.AddWithValue("@shelfNumber", shelfNumber);
                cmd.Parameters.AddWithValue("@com_NonCom", com_NonCom);
                cmd.Parameters.AddWithValue("@assetStatus", assetStatus);
                cmd.Parameters.AddWithValue("@assetType", assetType);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@subCategory", subCategory);
                cmd.Parameters.AddWithValue("@model", model);
                cmd.Parameters.AddWithValue("@control", control);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    lblMessage.Text = "Material added successfully!";
                    ClearFields();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }
        }

        private void ClearFields()
        {
            txtMaterialName.Text = "";
            txtPart_Id.Text = "";
            txtUnitPrice.Text = "";
            txtStockQuantity.Text = "";
            txtRackNumber.Text = "";
            txtShelfNumber.Text = "";
            ddlCom_NonCom.SelectedIndex = 0;
            ddlAssetStatus.SelectedIndex = 0;
            ddlAssetType.SelectedIndex = 0;
            ddlCategory.SelectedIndex = 0;
            ddlSubCategory.SelectedIndex = 0;
            ddlModel.SelectedIndex = 0;
            ddlControl.SelectedIndex = 0;
        }


    }
}