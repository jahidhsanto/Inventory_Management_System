using STORE_FINAL.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.UserDashboard
{
	public partial class MasterDashboard : System.Web.UI.MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                string role = Session["Role"] != null ? Session["Role"].ToString() : "Guest";

                // Show different dashboards based on role
                adminMenu.Visible = (role == "Admin");
                employeeMenu.Visible = (role == "Employee");
                departmentHeadMenu.Visible = (role == "Department Head");
                StoreInChargeMenu.Visible = (role == "Store InCharge");
            }

            lblUserName.Text = Session["EmployeeName"]?.ToString();
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            // Clear the session and log out the user
            Session.Abandon();  // End the session
            Session.Clear();    // Clear session data

            // Optionally, clear any authentication cookies if needed (if using forms authentication)
            if (Request.Cookies[".ASPXAUTH"] != null)
            {
                var authCookie = new HttpCookie(".ASPXAUTH");
                authCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(authCookie);
            }

            // Redirect to the login page after logout
            Response.Redirect("~/");
        }
    }
}