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
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the session values exist, otherwise redirect to login page
            if (Session["Username"] == null)
            {
                Response.Redirect("~/");
            }

            if (!IsPostBack)
            {
                RESET();
                LoadMaterials();
                LoadRequisition();
            }
        }

        private void LoadRequisition()
        {
            int employeeID = Convert.ToInt32(Session["EmployeeID"]); // Logged-in Department Head

            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                        SELECT 
                            r.Requisition_ID, m.Materials_Name, r.Quantity, r.Created_Date, r.Status AS Dept_Status, 
                            e1.Name AS Dept_Head, 
                            e2.Name AS Store_Status
                        FROM requisition r
                        JOIN Employee e1 
                            ON r.Employee_ID = e1.Employee_ID
                        LEFT JOIN Employee e2 
                            ON r.Approved_By = e2.Employee_ID
                        JOIN Material m 
                            ON r.Material_ID = m.Material_ID
                        WHERE r.Employee_ID = @EmployeeID";

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

            // Backend Validations
            if (!ValidateInput(employeeID, materialID, quantity))
            {
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                            INSERT INTO Requisition (Employee_ID, Material_ID, Quantity, Status, Store_Status, Approved_By) 
                            VALUES (@EmployeeID, @MaterialID, @Quantity, @Status, @StoreStatus, @ApprovedBy)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    cmd.Parameters.AddWithValue("@MaterialID", materialID);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@Status", "Pending");  // Set the Status to "Pending"
                    cmd.Parameters.AddWithValue("@StoreStatus", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ApprovedBy", DBNull.Value);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        ShowMessage("Requisition submitted successfully!", true);
                        RESET();
                        LoadRequisition();

                    }
                    catch (Exception ex)
                    {
                        ShowMessage("Database error: " + ex.Message, false);
                    }
                }
            }
        }

        private bool ValidateInput(string employeeID, string materialID, string quantityText)
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

            return true; // If all validations pass
        }

        private void LoadMaterials()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT Material_ID, Materials_Name 
                                FROM Material;";
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