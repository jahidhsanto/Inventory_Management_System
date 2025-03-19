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
                Response.Redirect("LogIn.aspx");
            }

            if (!IsPostBack)
            {
                LoadUsers(); // Load all users when the page first loads
            }
        }

        private void LoadUsers()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Username, Role, Employee_ID FROM Users", conn))
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
            int employeeID = Convert.ToInt32(txtEmployeeID.Text.Trim());

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("CreateUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.Parameters.AddWithValue("@Employee_ID", employeeID);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        lblMessage.Text = "User created successfully.";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        LoadUsers(); // Refresh user list
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Error: " + ex.Message;
                    }
                }
            }
        }

        protected void btnUpdateRole_Click(object sender, EventArgs e)
        {
            string username = txtUpdateUsername.Text.Trim();
            string newRole = ddlNewRole.SelectedValue;
            string adminUsername = Session["Username"].ToString();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateUserRole", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@NewRole", newRole);
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
                    }
                }
            }
        }

        protected void btnDeleteUser_Click(object sender, EventArgs e)
        {
            string username = txtDeleteUsername.Text.Trim();
            string adminUsername = Session["Username"].ToString();

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
                    }
                }
            }
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            string username = txtResetUsername.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();

            // Ensure all fields are provided
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(newPassword))
            {
                lblMessage.Text = "Please fill in all fields.";
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
    }
}