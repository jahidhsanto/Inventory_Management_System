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

                //departmentHeadMenu.Visible = (role == "Admin");
                //storePersonMenu.Visible = (role == "Store InCharge");
            }
        }
	}
}