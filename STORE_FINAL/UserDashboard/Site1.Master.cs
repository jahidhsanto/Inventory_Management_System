using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.UserDashboard
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string role = Session["Role"] != null ? Session["Role"].ToString() : "Guest";
                string userName = Session["Username"] != null ? Session["Username"].ToString() : "Guest";

                lblUserName.Text = userName;

                // Show different dashboards based on role
                storePersonDashboard.Visible = (role == "Admin");         // StorePerson
                employeeDashboard.Visible = (role == "Employee");               // Employee
                departmentHeadDashboard.Visible = (role == "Manager");   // DepartmentHead

                // Redirect if the user role is invalid
                if (role == "Guest")
                {
                    //Response.Redirect("Login.aspx");
                }
            }
        }
    }
}