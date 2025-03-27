using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Role_Admin
{
    public partial class AddEmployee : System.Web.UI.Page
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
                LoeadDepartments();
                LoadEmployee();
            }
        }

        private void LoadEmployee()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT e.Employee_ID, e.Name, e.Designation, d.DepartmentName
                                FROM Employee e
                                JOIN
	                                Department d ON e.Department_ID = d.Department_ID;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    //Run SQL & store in da
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    //Create DataTable for store output
                    DataTable dt = new DataTable();

                    //Send loaded DataTable to frontend
                    da.Fill(dt);
                    //start binding data with frontend
                    EmployeeGridView.DataSource = dt;
                    EmployeeGridView.DataBind();
                }
            }
        }

        private void LoeadDepartments()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT Department_ID, DepartmentName 
                                FROM Department;";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlDepartment.DataSource = reader;
                    ddlDepartment.DataTextField = "DepartmentName";
                    ddlDepartment.DataValueField = "Department_ID";
                    ddlDepartment.DataBind();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading departments: " + ex.Message, false);
                }
            }

            ddlDepartment.Items.Insert(0, new ListItem("-- Select Department --", "0"));
            ddlDepartment.SelectedValue = "0";
        }

        protected void btnAddEmployee_Click(object sender, EventArgs e)
        {
            string employeeID = txtEmployeeID.Text.Trim();
            string name = txtName.Text.Trim();
            string department = ddlDepartment.SelectedValue;
            string designation = txtDesignation.Text.Trim();

            // Backend Validations
            if (!ValidateInput(employeeID, name, department, designation))
            {
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "INSERT INTO Employee (Employee_ID, Name, Department_ID, Designation) " +
                               "VALUES (@Employee_ID, @Name, @Department_ID, @Designation)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Employee_ID", employeeID);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Department_ID", department);
                cmd.Parameters.AddWithValue("@Designation", designation);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    ShowMessage("Employee added successfully!", true);
                    RESET();
                }
                catch (Exception ex)
                {
                    ShowMessage("Database error: " + ex.Message, false);
                }
            }
        }

        private bool ValidateInput(string employeeID, string name, string department, string designation)
        {
            // Check if any field is empty
            if (string.IsNullOrEmpty(employeeID) || string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(designation) || department == "0")
            {
                ShowMessage("All fields are required.", false);
                return false;
            }

            // Employee ID must be numeric and between 1 to 4 digits
            if (!Regex.IsMatch(employeeID, @"^\d{1,4}$"))
            {
                ShowMessage("Employee ID must be a number (1-4 digits).", false);
                return false;
            }

            // Name should contain only letters and spaces, max length 50
            if (!Regex.IsMatch(name, @"^[A-Za-z\s]{1,50}$"))
            {
                ShowMessage("Name must contain only letters and be up to 50 characters.", false);
                return false;
            }

            // Designation max length 50
            if (designation.Length > 50)
            {
                ShowMessage("Designation should be up to 50 characters.", false);
                return false;
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
            txtEmployeeID.Text = "";
            txtName.Text = "";
            ddlDepartment.SelectedIndex = 0;
            txtDesignation.Text = "";
            lblMessage.Text = "";
            lblMessage.Visible = false;
        }
    }
}