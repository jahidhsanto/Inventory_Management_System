using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

namespace STORE_FINAL.Test
{
    public partial class Requisitions_Test : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProjects();
                LoadEmployees();
                LoadDepartments();
                LoadZones();
                LoadMaterials();
            }
        }

        // Load all DropDowns
        private void LoadProjects()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT Project_Code, CONCAT(Department, ' - ', Project_Name) AS Project_Name 
                                FROM Project;";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlProjectFor.DataSource = reader;
                    ddlProjectFor.DataTextField = "Project_Name";
                    ddlProjectFor.DataValueField = "Project_Code";
                    ddlProjectFor.DataBind();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading projects: " + ex.Message, false);
                }
            }

            ddlProjectFor.Items.Insert(0, new ListItem("-- Select Project --", "0"));
            ddlProjectFor.SelectedValue = "0";
        }
        private void LoadEmployees()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT Employee_ID, CONCAT(Employee_ID, ' - ', Name) AS Employee 
                                FROM Employee
                                ORDER BY Employee_ID ASC;";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlEmployeeFor.DataSource = reader;
                    ddlEmployeeFor.DataTextField = "Employee";
                    ddlEmployeeFor.DataValueField = "Employee_ID";
                    ddlEmployeeFor.DataBind();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading projects: " + ex.Message, false);
                }
            }

            ddlEmployeeFor.Items.Insert(0, new ListItem("-- Select Employee --", "0"));
            ddlEmployeeFor.SelectedValue = "0";
        }
        private void LoadDepartments()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT d.Department_ID, d.DepartmentName 
                                FROM Department d
                                INNER JOIN Employee e
                                    ON d.Department_Head_ID = e.Employee_ID;";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlDepartmentFor.DataSource = reader;
                    ddlDepartmentFor.DataTextField = "DepartmentName";
                    ddlDepartmentFor.DataValueField = "Department_ID";
                    ddlDepartmentFor.DataBind();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading projects: " + ex.Message, false);
                }
            }

            ddlDepartmentFor.Items.Insert(0, new ListItem("-- Select Department --", "0"));
            ddlDepartmentFor.SelectedValue = "0";
        }
        private void LoadZones()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT Zone_ID, Zone_Name 
                                FROM Zone;";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlZoneFor.DataSource = reader;
                    ddlZoneFor.DataTextField = "Zone_Name";
                    ddlZoneFor.DataValueField = "Zone_ID";
                    ddlZoneFor.DataBind();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading projects: " + ex.Message, false);
                }
            }

            ddlZoneFor.Items.Insert(0, new ListItem("-- Select Zone --", "0"));
            ddlZoneFor.SelectedValue = "0";
        }
        private void LoadMaterials()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT 
                                    m.Material_ID, 
                                    m.Materials_Name
                                FROM 
                                    Material m
                                JOIN 
                                    Stock s ON m.Material_ID = s.Material_ID
                                WHERE 
                                    s.Availability = 'AVAILABLE' 
                                    AND s.Quantity > 0
                                GROUP BY 
                                    m.Material_ID, 
                                    m.Materials_Name;";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlMaterial.DataSource = reader;
                    ddlMaterial.DataTextField = "Materials_Name";
                    ddlMaterial.DataValueField = "Material_ID";
                    ddlMaterial.DataBind();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading materials: " + ex.Message, false);
                }
            }

            ddlMaterial.Items.Insert(0, new ListItem("-- Select Material --", "0"));
            ddlMaterial.SelectedValue = "0";
        }

        protected void rblRequisitionFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = rblRequisitionFor.SelectedValue;

            ddlProjectFor.SelectedValue = "0";
            ddlRequisitionPurpose.SelectedValue = "0";
            ddlEmployeeFor.SelectedValue = "0";
            ddlDepartmentFor.SelectedValue = "0";
            ddlZoneFor.SelectedValue = "0";
            ddlMaterial.SelectedValue = "0";
            txtQuantity.Text = "";
            imgMaterial.Visible = false;
            Clear_ViewStateAndGrid();

            switch (selectedValue)
            {
                case "Project":
                    dropdownProjectFor.Visible = true;
                    dropdownEmployeeFor.Visible = false;
                    dropdownDepartmentFor.Visible = false;
                    dropdownZoneFor.Visible = false;
                    break;

                case "Employee":
                    dropdownProjectFor.Visible = false;
                    dropdownEmployeeFor.Visible = true;
                    dropdownDepartmentFor.Visible = false;
                    dropdownZoneFor.Visible = false;
                    break;

                case "Department":
                    dropdownProjectFor.Visible = false;
                    dropdownEmployeeFor.Visible = false;
                    dropdownDepartmentFor.Visible = true;
                    dropdownZoneFor.Visible = false;
                    break;

                case "Zone":
                    dropdownProjectFor.Visible = false;
                    dropdownEmployeeFor.Visible = false;
                    dropdownDepartmentFor.Visible = false;
                    dropdownZoneFor.Visible = true;
                    break;

                default:
                    // Optional: handle unexpected value
                    break;
            }
        }

        protected void resetSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMaterial.SelectedValue = "0";
            txtQuantity.Text = "";
            imgMaterial.Visible = false;
            Clear_ViewStateAndGrid();

        }

        protected void ddlMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedMaterialId = ddlMaterial.SelectedValue;

            string imagePath = GetMaterialImagePath(selectedMaterialId);

            if (!string.IsNullOrEmpty(imagePath))
            {
                imgMaterial.ImageUrl = imagePath;
                imgMaterial.Visible = true;
            }
            else
            {
                imgMaterial.Visible = false;
            }
        }

        private string GetMaterialImagePath(string materialId)
        {
            string imagePath = string.Empty;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "SELECT Material_Image_Path FROM Material WHERE Material_ID = @MaterialID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@MaterialID", materialId);

                con.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    imagePath = result.ToString();
                }
            }

            return imagePath;
        }

        private DataTable CreateItemTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Material_ID");
            dt.Columns.Add("Material_Name");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("RequisitionFor");
            dt.Columns.Add("EmployeeFor");
            dt.Columns.Add("DepartmentFor");
            dt.Columns.Add("ZoneFor");
            dt.Columns.Add("ProjectFor");
            dt.Columns.Add("RequisitionPurpose");

            return dt;
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            DataTable dt;

            if (ViewState["Items"] == null)
            {
                dt = CreateItemTable();
            }
            else
            {
                dt = (DataTable)ViewState["Items"];
            }

            string materialId = ddlMaterial.SelectedValue;
            string materialName = ddlMaterial.SelectedItem.Text;
            string quantity = txtQuantity.Text.Trim();
            string RequisitionFor = rblRequisitionFor.SelectedValue;
            string employeeID = ddlEmployeeFor.SelectedValue;
            string departmentID = ddlDepartmentFor.SelectedValue;
            string zoneID = ddlZoneFor.SelectedValue;
            string projectID = ddlProjectFor.SelectedValue;
            string requisitionPurpose = ddlRequisitionPurpose.SelectedValue;

            if (ValidateInput(RequisitionFor, quantity))
            {
                // Check for duplicate item
                bool alreadyExists = dt.AsEnumerable().Any(r => r["Material_ID"].ToString() == materialId);

                if (alreadyExists)
                {
                    ShowMessage("This material has already been added.", false);
                    return;
                }

                DataRow row = dt.NewRow();
                row["Material_ID"] = materialId;
                row["Material_Name"] = materialName;
                row["Quantity"] = quantity;
                row["RequisitionFor"] = RequisitionFor;
                row["EmployeeFor"] = employeeID;
                row["DepartmentFor"] = departmentID;
                row["ZoneFor"] = zoneID;
                row["ProjectFor"] = projectID;
                row["RequisitionPurpose"] = requisitionPurpose;

                dt.Rows.Add(row);

                ViewState["Items"] = dt;

                gvItems.DataSource = dt;
                gvItems.DataBind();
            }
        }

        protected void gvItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (ViewState["Items"] != null)
            {
                DataTable dt = (DataTable)ViewState["Items"];

                // Use index from the GridView row
                dt.Rows[e.RowIndex].Delete();

                ViewState["Items"] = dt;

                gvItems.DataSource = dt;
                gvItems.DataBind();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Session["EmployeeID"] == null)
            {
                ShowMessage("Session expired. Please log in again.", false);
                Response.Redirect("~/");
                return;
            }

            string CreatedByEmployee_ID = Session["EmployeeID"]?.ToString();

            if (ViewState["Items"] == null)
            {
                ShowMessage("No data to save.", false);
                return;
            }

            DataTable dt = (DataTable)ViewState["Items"];

            // Prepare the item list DataTable for table-valued parameter
            DataTable itemsTable = new DataTable();
            itemsTable.Columns.Add("Material_ID", typeof(int));
            itemsTable.Columns.Add("Quantity", typeof(int));

            foreach (DataRow row in dt.Rows)
            {
                int materialId = Convert.ToInt32(row["Material_ID"]);
                int quantity = Convert.ToInt32(row["Quantity"]);
                itemsTable.Rows.Add(materialId, quantity);
            }

            // Collect requisition meta data
            string requisitionFor = rblRequisitionFor.SelectedValue;
            string requisitionPurpose = ddlRequisitionPurpose.SelectedValue != "0" ? ddlRequisitionPurpose.SelectedValue : null;
            int? employeeFor = ddlEmployeeFor.SelectedValue != "0" ? Convert.ToInt32(ddlEmployeeFor.SelectedValue) : (int?)null;
            int? departmentFor = ddlDepartmentFor.SelectedValue != "0" ? Convert.ToInt32(ddlDepartmentFor.SelectedValue) : (int?)null;
            int? zoneFor = ddlZoneFor.SelectedValue != "0" ? Convert.ToInt32(ddlZoneFor.SelectedValue) : (int?)null;
            string projectCode = ddlProjectFor.SelectedValue != "0" ? ddlProjectFor.SelectedValue : null;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    using (SqlCommand cmd = new SqlCommand("InsertRequisitionWithItems", conn, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CreatedByEmployee_ID", Convert.ToInt32(CreatedByEmployee_ID));
                        cmd.Parameters.AddWithValue("@Dept_Status", "Pending");
                        cmd.Parameters.AddWithValue("@Store_Status", "Pending");
                        cmd.Parameters.AddWithValue("@Requisition_For", requisitionFor);

                        cmd.Parameters.AddWithValue("@Employee_ID_For", (object)employeeFor ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Department_ID_For", (object)departmentFor ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Zone_ID_For", (object)zoneFor ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Project_Code_For", (object)projectCode ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Requisition_Purpose", (object)requisitionPurpose ?? DBNull.Value);

                        SqlParameter itemListParam = cmd.Parameters.AddWithValue("@ItemList", itemsTable);
                        itemListParam.SqlDbType = SqlDbType.Structured;
                        itemListParam.TypeName = "RequisitionItemType";

                        SqlParameter outputParam = new SqlParameter("@NewRequisitionID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        cmd.ExecuteNonQuery();

                        int newRequisitionID = Convert.ToInt32(outputParam.Value);
                        transaction.Commit();

                        ShowMessage("Requisition submitted successfully. ID: " + newRequisitionID, true);
                        Clear_ViewStateAndGrid();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ShowMessage("Error saving requisition: " + ex.Message, false);
                }
            }


        }

        private bool ValidateInput(string selectedRequisitionFor, string quantityText)
        {
            // Check if a material is selected
            if (ddlMaterial.SelectedValue == "0")
            {
                ShowMessage("Please select a material.", false);
                return false;
            }

            // Check if quantity is valid
            int quantity;
            if (string.IsNullOrEmpty(quantityText) || !int.TryParse(quantityText, out quantity) || quantity <= 0)
            {
                ShowMessage("Please enter a valid quantity (positive number).", false);
                return false;
            }

            // Validate Based on Selected RadioButton
            if (selectedRequisitionFor == "Project")
            {
                if (ddlProjectFor.SelectedValue == "0")
                {
                    ShowMessage("Please select a project.", false);
                    return false;
                }
                if (ddlRequisitionPurpose.SelectedValue == "0")
                {
                    ShowMessage("Please select a purpose.", false);
                    return false;
                }
            }
            else if (selectedRequisitionFor == "Employee")
            {
                if (ddlEmployeeFor.SelectedIndex == 0)
                {
                    ShowMessage("Please select an employee.", false);
                    return false;
                }
            }
            else if (selectedRequisitionFor == "Department")
            {
                if (ddlDepartmentFor.SelectedIndex == 0)
                {
                    ShowMessage("Please select a department.", false);
                    return false;
                }
            }
            else if (selectedRequisitionFor == "Zone")
            {
                if (ddlZoneFor.SelectedIndex == 0)
                {
                    ShowMessage("Please select a zone.", false);
                    return false;
                }
            }

            return true; // If all validations pass
        }

        // Clear ViewState and Grid
        private void Clear_ViewStateAndGrid()
        {
            ViewState["Items"] = null;
            gvItems.DataSource = null;
            gvItems.DataBind();

        }
        private void ShowMessage(string message, bool isSuccess)
        {
            string messageType = isSuccess ? "success" : "error";
            string escapedMessage = message.Replace("'", "\\'"); // Escape single quotes

            string js = $@"
                            setTimeout(function() {{
                                if (typeof showToast === 'function') {{
                                    showToast('{escapedMessage}', '{messageType}');
                                }}
                            }}, 100);
                        ";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowToastMessage", js, true);
        }
    }
}