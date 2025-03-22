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
                LoadPendingRequisitions();
            }
        }


        private void LoadPendingRequisitions()
        {
            int departmentHeadID = Convert.ToInt32(Session["EmployeeID"]); // Logged-in Department Head

            string query = @"
                            SELECT R.Requisition_ID, E.Name AS Requested_By, M.Materials_Name AS Material_Name, R.Quantity, R.Status, R.Created_Date
                            FROM Requisition R
                            JOIN Employee E ON R.Employee_ID = E.Employee_ID
                            JOIN Material M ON R.Material_ID = M.Material_ID
                            JOIN Department D ON R.Department_ID = D.Department_ID
                            WHERE D.Department_Head_ID = @DepartmentHeadID
                            AND R.Status = 'Pending'";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentHeadID", departmentHeadID);
                    conn.Open();
                    ApproveRequisitionGridView.DataSource = cmd.ExecuteReader();
                    ApproveRequisitionGridView.DataBind();
                }
            }
        }

        protected void ApproveRequisitionGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Approve" || e.CommandName == "Reject")
            {
                int requisitionID = Convert.ToInt32(e.CommandArgument);
                string newStatus = (e.CommandName == "Approve") ? "Approved" : "Rejected";

                UpdateRequisitionStatus(requisitionID, newStatus);
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

            LoadPendingRequisitions(); // Refresh the list
        }

    }
}