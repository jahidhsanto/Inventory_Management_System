using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.UserAuthentication
{
	public partial class UserManagement : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            // Ensure only admins can access this page
            if (Session["Username"] == null || Session["Role"] == null || Session["Role"].ToString() != "Admin")
            {
                Response.Redirect("~/");
            }

            if (!IsPostBack)
            {
                LoadUsers(); // Load all users when the page first loads
                LoadDepartments();
            }
        }

        private void LoadDepartments()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT Role_ID, Role
                                FROM Role;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);  // Load data into a DataTable

                ddlRole.DataSource = dt;
                ddlRole.DataTextField = "Role";
                ddlRole.DataValueField = "Role_ID";
                ddlRole.DataBind();

                ddlNewRole.DataSource = dt;
                ddlNewRole.DataTextField = "Role";
                ddlNewRole.DataValueField = "Role_ID";
                ddlNewRole.DataBind();

            }

            // default select as Employee
            ddlRole.SelectedValue = "2";
            ddlNewRole.SelectedValue = "2";
        }


        private void LoadUsers()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString))
            {
                string query = @"
                                SELECT 
                                    u.Username, 
                                    r.Role, 
                                    u.Employee_ID, 
	                                e.Name, 
	                                d.DepartmentName
                                FROM 
                                    dbo.Users u
                                JOIN 
                                    dbo.Role r ON u.Role_ID = r.Role_ID
                                JOIN 
	                                dbo.Employee e ON u.Employee_ID = e.Employee_ID
                                JOIN 
	                                dbo.Department d ON e.Department_ID = d.Department_ID;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        gvUsers.DataSource = dt;
                        gvUsers.DataBind();
                    }
                }
            }
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = ddlRole.SelectedValue;
            string employeeIDInput = txtEmployeeID.Text.Trim();

            // Validation for empty fields
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(employeeIDInput))
            {
                ShowMessage("All fields are required.", false);
                return;
            }

            int employeeID;
            if (!int.TryParse(employeeIDInput, out employeeID) || employeeID <= 0)
            {
                ShowMessage("Please enter a valid Employee ID.", false);
                return;
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("InsertUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Role_ID", role);
                    cmd.Parameters.AddWithValue("@Employee_ID", employeeID);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        ShowMessage("User created successfully.", true);
                        LoadUsers(); // Refresh user list
                    }
                    catch (Exception ex)
                    {
                        ShowMessage("Error: " + ex.Message, true);
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        protected void btnUpdateRole_Click(object sender, EventArgs e)
        {
            string username = txtUpdateUsername.Text.Trim();
            string newRole = ddlNewRole.SelectedValue;
            string adminUsername = Session["Username"].ToString();

            // Validation for empty fields
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(newRole))
            {
                lblMessage.Text = "Please provide both username and new role.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateUserRole", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@NewRole_ID", newRole);
                    cmd.Parameters.AddWithValue("@AdminUsername", adminUsername);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        lblMessage.Text = "User role updated successfully.";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        LoadUsers(); // Refresh user list
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error: " + ex.Message;
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        protected void btnDeleteUser_Click(object sender, EventArgs e)
        {
            string username = txtDeleteUsername.Text.Trim();
            string adminUsername = Session["Username"].ToString();

            // Validation for empty username
            if (string.IsNullOrEmpty(username))
            {
                lblMessage.Text = "Please provide a username to delete.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@AdminUsername", adminUsername);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        lblMessage.Text = "User deleted successfully.";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        LoadUsers(); // Refresh user list
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error: " + ex.Message;
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            string username = txtResetUsername.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();

            // Validation for empty fields
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(newPassword))
            {
                lblMessage.Text = "Please fill in all fields.";
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
                return;
            }

            // Password length validation
            if (newPassword.Length < 6)
            {
                lblMessage.Text = "Password must be at least 6 characters long.";
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
                return;
            }

            try
            {
                // Call the stored procedure to reset the password
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDB"].ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("ResetPassword", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@NewPassword", newPassword);

                        cmd.ExecuteNonQuery();
                    }
                }

                lblMessage.Text = "Password reset successfully.";
                lblMessage.CssClass = "alert alert-success";
                lblMessage.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
            }
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "alert alert-success" : "alert alert-danger";
            lblMessage.Visible = true;
        }
    }
}