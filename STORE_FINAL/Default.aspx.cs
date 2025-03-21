using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                Session.Clear();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (AuthenticateUser(username, password, out string role))
            {
                // If user authentication is pass then create session for the user
                if (EmployeeInformation(username))
                {
                    Response.Redirect("~/Dashboard.aspx");
                }
                else
                {
                    lblMessage.Text = "Employee information could not be retrieved.";
                }
            }
            else
            {
                lblMessage.Text = "Invalid username or password.";
            }
        }

        private bool AuthenticateUser(string username, string password, out string role)
        {
            role = string.Empty;

            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("UserLogin", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters.AddWithValue("password", password);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            role = reader["Role"].ToString();
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text += ex.Message;
                    }
                }
            }
            return false;
        }

        private bool EmployeeInformation(string username)
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("GetEmployeeByUsername", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("username", username);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            // Create session variables for employee details
                            Session["Username"] = reader["Username"];
                            Session["Role"] = reader["Role"];
                            Session["EmployeeID"] = reader["Employee_ID"];
                            Session["EmployeeName"] = reader["Name"];
                            Session["EmployeeDepartment"] = reader["Department"];
                            Session["EmployeeDesignation"] = reader["Designation"];
                            return true;
                        }
                        else
                        {
                            lblMessage.Text = "No employee found for the given username.";
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text += " Error retrieving Employee ID: " + ex.Message;
                        return false;
                    }
                }
            }
        }
    }
}
