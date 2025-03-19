using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.TEST
{
	public partial class Dashboard : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (Session["Username"] == null)
            {
                //Response.Redirect("Login.aspx"); // Redirect if not logged in
            }
            else
            {
                lblUsername.Text = Session["Username"].ToString();
            }

        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon(); // Clear session
            Response.Redirect("Login.aspx");
        }

    }
}