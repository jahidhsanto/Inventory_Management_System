using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.UserDashboard
{
	public partial class Logout : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            // Clear the session and any authentication cookies
            Session.Abandon();  // End the session
            Session.Clear();    // Clear session data

            // Optionally, clear any authentication cookies if needed (if using forms authentication)
            if (Request.Cookies[".ASPXAUTH"] != null)
            {
                var authCookie = new HttpCookie(".ASPXAUTH");
                authCookie.Expires = DateTime.Now.AddDays(-1); // Expire the cookie
                Response.Cookies.Add(authCookie); // Add the expired cookie
            }

            Response.Redirect("~");
        }
    }
}