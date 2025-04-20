using STORE_FINAL.Pages;
using STORE_FINAL.Role_Admin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace STORE_FINAL.Role_Employee
{
    public partial class Requisition : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null || Session["Role"] == null ||
                Session["Role"].ToString() != "Employee" && Session["Role"].ToString() != "Department Head")
            {
                Response.Redirect("~/");
            }

            if (!IsPostBack)
            {
                RESET();
                LoadProjects();
                LoadEmployees();
                LoadDepartments();
                LoadZones();
                LoadMaterials();
                LoadRequisition();
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
                                    s.Status = 'AVAILABLE' 
                                    AND s.Quantity > 0
                                GROUP BY 
                                    m.Material_ID, 
                                    m.Materials_Name;";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlMaterials.DataSource = reader;
                    ddlMaterials.DataTextField = "Materials_Name";
                    ddlMaterials.DataValueField = "Material_ID";
                    ddlMaterials.DataBind();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading materials: " + ex.Message, false);
                }
            }

            ddlMaterials.Items.Insert(0, new ListItem("-- Select Material --", "0"));
            ddlMaterials.SelectedValue = "0";
        }
        
        protected void btnSubmitRequisition_Click(object sender, EventArgs e)
        {
            if (Session["EmployeeID"] == null)
            {
                ShowMessage("Session expired. Please log in again.", false);
                Response.Redirect("~/");
                return;
            }

            string employeeID = Session["EmployeeID"]?.ToString();
            string materialID = ddlMaterials.SelectedValue;
            string quantity = txtQuantity.Text.Trim();
            string selectedRequisitionFor = rblRequisitionFor.SelectedValue; 
            string employeeSelected = ddlEmployeeFor.SelectedValue;
            string departmentID = ddlDepartmentFor.SelectedValue;
            string zoneID = ddlZoneFor.SelectedValue;
            string projectID = ddlProjectFor.SelectedValue;
            string requisitionType = ddlRequisitionType.SelectedValue;

            // Backend Validations
            if (!ValidateInput(selectedRequisitionFor, employeeID, projectID, materialID, quantity))
            {
                return;
            }

            // Determine the value for the INSERT query based on the selected radio button
            string insertColumn = "";
            string insertValue = "";
            string additionalColumn = "";
            string additionalValue = "";

            // Depending on the radio button selected, set the respective column and value
            if (selectedRequisitionFor == "Project")
            {
                insertColumn = "Project_Code_For";
                insertValue = projectID;
                additionalColumn = ", Requisition_Type";
                additionalValue = ", @Requisition_Type";
            }
            else if (selectedRequisitionFor == "Employee")
            {
                insertColumn = "Employee_ID_For";
                insertValue = employeeSelected;
            }
            else if (selectedRequisitionFor == "Department")
            {
                insertColumn = "Department_ID_For";
                insertValue = departmentID;
            }
            else if (selectedRequisitionFor == "Zone")
            {
                insertColumn = "Zone_ID_For";
                insertValue = zoneID;
            }
            else
            {
                ShowMessage("Invalid selection. Please choose a valid option.", false);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                INSERT INTO Requisition (Employee_ID, Material_ID, Quantity, Status, Requisition_For, " + insertColumn + additionalColumn + @") 
                                VALUES (@EmployeeID, @MaterialID, @Quantity, @Status, @Requisition_For, @" + insertColumn + additionalValue + ")";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    cmd.Parameters.AddWithValue("@MaterialID", materialID);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@Status", "Pending");
                    cmd.Parameters.AddWithValue("@Requisition_For", selectedRequisitionFor);
                    cmd.Parameters.AddWithValue("@" + insertColumn, insertValue);

                    if (selectedRequisitionFor == "Project")
                    {
                        cmd.Parameters.AddWithValue("@Requisition_Type", requisitionType);
                    }

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        ShowMessage("Requisition submitted successfully!", true);
                        RESET();
                        Response.Redirect("Requisition");
                    }
                    catch (Exception ex)
                    {
                        ShowMessage("Database error: " + ex.Message, false);
                    }
                }
            }
        }

        // Load Requisition table
        private void LoadRequisition()
        {
            int employeeID = Convert.ToInt32(Session["EmployeeID"]); // Logged-in Department Head

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                        SELECT 
                            r.Requisition_ID, m.Materials_Name, r.Quantity, r.Created_Date, r.Status AS Dept_Status, 
                            r.Store_Status, eh.Name AS Dept_Head
                        FROM requisition r
                        JOIN Material m 
                            ON r.Material_ID = m.Material_ID
                        LEFT JOIN Employee emp 
                            ON r.Employee_ID = emp.Employee_ID
                        LEFT JOIN Department d 
                            ON emp.Department_ID = d.Department_ID
                        LEFT JOIN Employee eh 
                            ON d.Department_Head_ID = eh.Employee_ID
                        WHERE r.Employee_ID = @EmployeeID
                        ORDER BY r.Requisition_ID DESC;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);

                    conn.Open();
                    //Run SQL & store in da
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    //Create DataTable for store output
                    DataTable dt = new DataTable();

                    //Send loaded DataTable to frontend
                    da.Fill(dt);
                    //start binding data with frontend
                    RequisitionGridView.DataSource = dt;
                    RequisitionGridView.DataBind();
                }
            }
        }

        private bool ValidateInput(string selectedRequisitionFor, string employeeID, string projectID, string materialID, string quantityText)
        {
            // Check if a material is selected
            if (ddlMaterials.SelectedIndex == 0)
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
                if (ddlProjectFor.SelectedIndex == 0)
                {
                    ShowMessage("Please select a project.", false);
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

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "alert alert-success" : "alert alert-danger";
            lblMessage.Visible = true;
        }

        protected void RESET()
        {
            txtQuantity.Text = "";
            ddlMaterials.SelectedIndex = 0;
            lblMessage.Text = "";
            lblMessage.Visible = false;

        }
    }
}