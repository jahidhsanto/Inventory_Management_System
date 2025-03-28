using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Role_DepartmentHead
{
	public partial class RequisitionApproval : System.Web.UI.Page
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
                LoadRequisitions("All"); // Load all requisitions on first load
                //LoadPendingRequisitions();
            }
        }

        private void LoadRequisitions(string status)
        {
            int employeeID = Convert.ToInt32(Session["EmployeeID"]); // Logged-in Department Head

            string query = @"
                        SELECT 
                            r.Requisition_ID, 
	                        m.Materials_Name, 
                            r.Quantity, 
	                        r.Created_Date, 
                            emp.Name AS Requested_By,
                            r.Status AS Dept_Status, 
                            r.Store_Status, 
                            eh.Name AS Dept_Head
                        FROM requisition r
                        JOIN Material m 
                            ON r.Material_ID = m.Material_ID
                        LEFT JOIN Employee emp  -- Employee who made the requisition
                            ON r.Employee_ID = emp.Employee_ID
                        LEFT JOIN Department d 
                            ON emp.Department_ID = d.Department_ID
                        LEFT JOIN Employee eh 
                            ON d.Department_Head_ID = eh.Employee_ID
                        WHERE emp.Department_ID = (
                            SELECT Department_ID FROM Employee WHERE Employee_ID = @EmployeeID)";

            if (status != "All")
            {
                query += " AND r.Status = @Status";
            }

            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    if (status != "All")
                        cmd.Parameters.AddWithValue("@Status", status);

                    conn.Open();
                    RequisitionApprovalGridView.DataSource = cmd.ExecuteReader();
                    RequisitionApprovalGridView.DataBind();
                }
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            string selectedStatus = ddlStatus.SelectedValue;
            LoadRequisitions(selectedStatus);
        }

        protected void ApproveRequisitionGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Approve" || e.CommandName == "Reject" || e.CommandName == "Pending")
            {
                int requisitionID = Convert.ToInt32(e.CommandArgument);
                string newStatus = "";

                if (e.CommandName == "Approve")
                    newStatus = "Approved";
                else if (e.CommandName == "Reject")
                    newStatus = "Rejected";
                else if (e.CommandName == "Pending")
                    newStatus = "Pending";

                UpdateRequisitionStatus(requisitionID, newStatus);

                // Reload filtered data
                string selectedStatus = ddlStatus.SelectedValue;
                LoadRequisitions(selectedStatus);
            }
        }

        private void UpdateRequisitionStatus(int requisitionID, string status)
        {
            int departmentHeadID = Convert.ToInt32(Session["EmployeeID"]); // Logged-in Department Head

            string query = @"
                            UPDATE Requisition 
                            SET Status = @Status, Approved_By = @DepartmentHeadID 
                            WHERE Requisition_ID = @RequisitionID";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@DepartmentHeadID", departmentHeadID);
                    cmd.Parameters.AddWithValue("@RequisitionID", requisitionID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            LoadRequisitions("All"); // Refresh the list
        }
    }
}