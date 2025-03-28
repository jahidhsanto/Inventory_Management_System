using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Forms
{
	public partial class ApproveRequisition : System.Web.UI.Page
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
            int departmentHeadID = Convert.ToInt32(Session["EmployeeID"]); // Logged-in Department Head

            string query = @"
                    SELECT R.Requisition_ID, E.Name AS Requested_By, M.Materials_Name AS Material_Name, R.Quantity, R.Status, R.Created_Date
                    FROM Requisition R
                    JOIN Employee E ON R.Employee_ID = E.Employee_ID
                    JOIN Material M ON R.Material_ID = M.Material_ID";

            if (status != "All")
            {
                query += " AND R.Status = @Status";
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentHeadID", departmentHeadID);
                    if (status != "All")
                        cmd.Parameters.AddWithValue("@Status", status);

                    conn.Open();
                    ApproveRequisitionGridView.DataSource = cmd.ExecuteReader();
                    ApproveRequisitionGridView.DataBind();
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