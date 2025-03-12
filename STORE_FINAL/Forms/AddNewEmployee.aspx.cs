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
	public partial class AddNewEmployee : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

        }

        protected void btnAddEmployee_Click(object sender, EventArgs e)
        {
            string employeeID = txtEmployeeID.Text.Trim();
            string name = txtName.Text.Trim();
            string department = ddlDepartment.SelectedValue;
            string role = txtRole.Text.Trim();

            if (string.IsNullOrEmpty(employeeID) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(department) || string.IsNullOrEmpty(role))
            {
                lblMessage.Text = "All fields are required.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "INSERT INTO Employee (Office_ID, Name, Department, Role) VALUES (@EmployeeID, @Name, @Department, @Role)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Department", department);
                cmd.Parameters.AddWithValue("@Role", role);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    lblMessage.Text = "Employee added successfully!";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }

        }
    }
}