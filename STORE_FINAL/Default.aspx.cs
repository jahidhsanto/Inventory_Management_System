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

            if (AuthenticateUser(username, password))
            {
                    Response.Redirect("~/Dashboard.aspx");
                    lblMessage.Text = "Employee information could not be retrieved.";
            }
            else
            {
                lblMessage.Text = "Invalid username or password.";
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
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
                            Session["Username"] = reader["Username"];
                            Session["Role"] = reader["Role"];
                            Session["EmployeeID"] = reader["Employee_ID"];
                            Session["EmployeeName"] = reader["EmployeeName"];
                            Session["EmployeeDepartmentID"] = reader["DepartmentName"];
                            Session["EmployeeDesignation"] = reader["Designation"];

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
    }
}
