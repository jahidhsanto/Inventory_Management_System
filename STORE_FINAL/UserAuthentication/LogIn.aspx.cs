using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.UserAuthentication
{
	public partial class LogIn : System.Web.UI.Page
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

			if(AuthenticateUser(username, password, out string role))
			{
				Session["Username"] = username;
				Session["Role"] = role;
				Response.Redirect("~/Dashboard.aspx");
			}
			else
			{
                lblMessage.Text = "Invalid username or password.";
            }
        }

		private bool AuthenticateUser(string username, string password, out string role)
        {
			role = string.Empty;

			string connStr=ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

			using(SqlConnection conn = new SqlConnection(connStr))
			{
				using (SqlCommand cmd = new SqlCommand("UserLogin", conn))
				{
					cmd.CommandType=CommandType.StoredProcedure;
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
    }
}