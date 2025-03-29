using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Role_StoreIncharge
{
    public partial class RequisitionList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null || Session["Role"] == null ||
                Session["Role"].ToString() != "Store InCharge")
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
                                eh.Name AS Dept_Head,
                                r.Store_Status_By
                            FROM requisition r
                            JOIN Material m 
                                ON r.Material_ID = m.Material_ID
                            LEFT JOIN Employee emp 
                                ON r.Employee_ID = emp.Employee_ID
                            LEFT JOIN Department d 
                                ON emp.Department_ID = d.Department_ID
                            LEFT JOIN Employee eh 
                                ON d.Department_Head_ID = eh.Employee_ID
                            WHERE r.Status = 'Approved'";

            if (status != "All")
            {
                query += " AND r.Store_Status = @Status";
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
            string selectedStatus = ddlStoreStatus.SelectedValue;
            LoadRequisitions(selectedStatus);
        }

        protected void ApproveRequisitionGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Approve" || e.CommandName == "Reject" || 
                e.CommandName == "Pending" || e.CommandName == "Processing" || 
                e.CommandName == "OutOfStock" || e.CommandName == "Ordered" || 
                e.CommandName == "Delivered")
            {
                int requisitionID = Convert.ToInt32(e.CommandArgument);
                string newStatus = "";

                switch (e.CommandName)
                {
                    case "Approve":
                        newStatus = "Approved";
                        break;
                    case "Reject":
                        newStatus = "Rejected";
                        break;
                    case "Pending":
                        newStatus = "Pending";
                        break;
                    case "Processing":
                        newStatus = "Processing";
                        break;
                    case "OutOfStock":
                        newStatus = "Out of Stock";
                        break;
                    case "Ordered":
                        newStatus = "Ordered";
                        break;
                    case "Delivered":
                        newStatus = "Delivered";
                        break;
                }

                UpdateRequisitionStatus(requisitionID, newStatus);

                // Reload filtered data
                string selectedStatus = ddlStoreStatus.SelectedValue;
                LoadRequisitions(selectedStatus);
            }
        }

        private void UpdateRequisitionStatus(int requisitionID, string status)
        {
            int storeInchargeID = Convert.ToInt32(Session["EmployeeID"]); 

            string query = @"
                            UPDATE Requisition 
                            SET Store_Status = @StoreStatus, Store_Status_By = @StoreInchargeID
                            WHERE Requisition_ID = @RequisitionID";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StoreStatus", status);
                    cmd.Parameters.AddWithValue("@StoreInchargeID", storeInchargeID);
                    cmd.Parameters.AddWithValue("@RequisitionID", requisitionID);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            LoadRequisitions("All"); // Refresh the list
        }
    }
}