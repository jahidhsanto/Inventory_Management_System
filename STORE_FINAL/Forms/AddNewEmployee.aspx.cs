using STORE_FINAL.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace STORE_FINAL.Forms
{
	public partial class AddNewEmployee : System.Web.UI.Page
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
            }
        }

        protected void RESET()
        {
            txtEmployeeID.Text = "";
            txtName.Text = "";
            ddlDepartment.SelectedIndex = 0;
            txtDesignation.Text = "";
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

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlDepartment.DataSource = reader;
                ddlDepartment.DataTextField = "DepartmentName";
                ddlDepartment.DataValueField = "Department_ID";
                ddlDepartment.DataBind();
            }

            ddlDepartment.Items.Insert(0, new ListItem("-- Select Material --", "0"));
        }

        protected void btnAddEmployee_Click(object sender, EventArgs e)
        {
            string employeeID = txtEmployeeID.Text.Trim();
            string name = txtName.Text.Trim();
            string department = ddlDepartment.SelectedValue;
            string designation = txtDesignation.Text.Trim();

            if (string.IsNullOrEmpty(employeeID) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(department) || string.IsNullOrEmpty(designation))
            {
                lblMessage.Text = "All fields are required.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "INSERT INTO Employee (Employee_ID, Name, Department, Designation) " +
                               "VALUES (@EmployeeID, @Name, @Department, @Designation)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Department", department);
                cmd.Parameters.AddWithValue("@Designation", designation);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    lblMessage.Text = "Employee added successfully!";
                    RESET();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }

        }
    }
}