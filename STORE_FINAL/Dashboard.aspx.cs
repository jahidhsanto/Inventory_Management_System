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
            // Check if the session values exist, otherwise redirect to login page
            if (Session["Username"] == null)
            {
                Response.Redirect("~/");
            }

            // Retrieve session values
            string username = Session["Username"]?.ToString();
            string role = Session["Role"]?.ToString();
            string employeeID = Session["EmployeeID"]?.ToString();
            string employeeName = Session["EmployeeName"]?.ToString();
            string employeeDepartment = Session["EmployeeDepartment"]?.ToString();
            string employeeDesignation = Session["EmployeeDesignation"]?.ToString();

            // Bind session values to the corresponding labels
            lblUsername.Text = username;
            lblRole.Text = role;
            lblID.Text = employeeID;
            lblName.Text = employeeName;
            lblDepartment.Text = employeeDepartment;
            lblDesignation.Text = employeeDesignation;
        }
    }
}