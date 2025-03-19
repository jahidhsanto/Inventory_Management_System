using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL
{
	public partial class Dashboard : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (Session["Username"] != null && Session["Role"] != null)
            {
                lblUsername.Text = Session["Username"].ToString();
                lblUserRole.Text = Session["Role"].ToString();
            }
            else
            {
                Response.Redirect("~/UserAuthentication/Login.aspx");
            }
        }
    }
}