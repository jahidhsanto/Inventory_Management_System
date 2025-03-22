using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Forms
{
	public partial class MaterialApprovalForm : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            // Check if the session values exist, otherwise redirect to login page
            if (Session["Username"] == null)
            {
                Response.Redirect("~/");
            }
            
            if (!IsPostBack)
            {
                LoadRequisitions();
            }
        }

        private void LoadRequisitions()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT 
                                     R.Requisition_ID AS RequisitionID,
                                     R.Employee_ID AS EmployeeID,
                                     E.Name AS EmployeeName,
                                     R.Material_ID AS MaterialID,
                                     M.Name AS MaterialName
                                 FROM 
                                     Requisition R
                                 JOIN 
                                     Employee E ON R.Employee_ID = E.Employee_ID
                                 JOIN 
                                     Material M ON R.Material_ID = M.Material_ID
                                 WHERE R.Status = 'Pending'";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlRequisitions.DataSource = reader;
                ddlRequisitions.DataTextField = "EmployeeName";
                ddlRequisitions.DataValueField = "RequisitionID";
                ddlRequisitions.DataBind();
            }

            ddlRequisitions.Items.Insert(0, new ListItem("-- Select Requisition --", "0"));
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {

        }
    }
}