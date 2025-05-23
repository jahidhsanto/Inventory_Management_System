using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Test_02
{
    public partial class RequisitionsApproval_Test : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
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
        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRequisitions();
        }
        private void LoadRequisitions()
        {
            int employeeID = Convert.ToInt32(Session["EmployeeID"]); // Logged-in Department Head

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string baseQuery = @"
                    SELECT R.Requisition_ID, E.Name AS CreatedBy, E.Employee_ID AS CreatedBy_ID, R.Created_Date, R.Requisition_For, 
                            R.Dept_Status, R.Dept_Approval_Remarks, R.Store_Status
                    FROM Requisition_Parent R
                    INNER JOIN Employee E ON R.CreatedByEmployee_ID = E.Employee_ID
					LEFT JOIN Department D ON E.Department_ID = D.Department_ID
					LEFT JOIN Employee ED ON D.Department_Head_ID = ED.Employee_ID
                    WHERE 1 = 1 AND E.Department_ID = (
						SELECT Department_ID FROM Employee WHERE Employee_ID = @EmployeeID)";

                if (!string.IsNullOrEmpty(ddlStatusFilter.SelectedValue))
                    baseQuery += " AND R.Dept_Status = @Status";

                if (!string.IsNullOrEmpty(ddlEmployeeFilter.SelectedValue))
                    baseQuery += " AND R.CreatedByEmployee_ID = @Employee_ID";

                if (!string.IsNullOrEmpty(txtSearchRequisitionID.Text))
                    baseQuery += " AND R.Requisition_ID LIKE @Requisition_ID";

                if (!string.IsNullOrEmpty(txtStartDate.Text))
                    baseQuery += " AND R.Created_Date >= @StartDate";

                if (!string.IsNullOrEmpty(txtEndDate.Text))
                    baseQuery += " AND R.Created_Date <= @EndDate";

                SqlCommand cmd = new SqlCommand(baseQuery, con);
                cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                if (!string.IsNullOrEmpty(ddlStatusFilter.SelectedValue))
                    cmd.Parameters.AddWithValue("@Status", ddlStatusFilter.SelectedValue);
                if (!string.IsNullOrEmpty(ddlEmployeeFilter.SelectedValue))
                    cmd.Parameters.AddWithValue("@Employee_ID", ddlEmployeeFilter.SelectedValue);
                if (!string.IsNullOrEmpty(txtSearchRequisitionID.Text))
                    cmd.Parameters.AddWithValue("@Requisition_ID", "%" + txtSearchRequisitionID.Text + "%");
                if (!string.IsNullOrEmpty(txtStartDate.Text))
                    cmd.Parameters.AddWithValue("@StartDate", DateTime.Parse(txtStartDate.Text));
                if (!string.IsNullOrEmpty(txtEndDate.Text))
                    cmd.Parameters.AddWithValue("@EndDate", DateTime.Parse(txtEndDate.Text));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Bind the filtered requisitions to the Repeater
                rptRequisitions.DataSource = dt;
                rptRequisitions.DataBind();

                // Populate the Status Dropdown dynamically based on available statuses in the data
                PopulateStatusDropdown(dt);

                // Populate the Employee Dropdown dynamically based on available employees in the data
                PopulateEmployeeDropdown(dt);

                //UpdateSummaryCounts(con);
                UpdateSummary(dt);
            }
        }
        private void PopulateStatusDropdown(DataTable dt)
        {
            var statuses = dt.AsEnumerable()
                             .Select(row => row["Dept_Status"].ToString())
                             .Distinct()
                             .ToList();

            var lastSelected = ddlStatusFilter.SelectedValue;
            ddlStatusFilter.Items.Clear();            
            ddlStatusFilter.Items.Add(new ListItem("All Status", ""));
            foreach (var status in statuses)
            {
                ddlStatusFilter.Items.Add(new ListItem(status, status));
            }
            ddlStatusFilter.SelectedValue = lastSelected;
        }
        private void PopulateEmployeeDropdown(DataTable dt)
        {
            var employees = dt.AsEnumerable()
                              .Select(row => new
                              {
                                  EmployeeID = row["CreatedBy_ID"].ToString(),
                                  EmployeeName = row["CreatedBy"].ToString()
                              })
                              .Distinct()
                              .ToList();

            var lastSelected = ddlEmployeeFilter.SelectedValue;
            ddlEmployeeFilter.Items.Clear();
            ddlEmployeeFilter.Items.Add(new ListItem("All Employees", ""));
            foreach (var employee in employees)
            {
                ddlEmployeeFilter.Items.Add(new ListItem(employee.EmployeeName, employee.EmployeeID));
            }
            ddlEmployeeFilter.SelectedValue = lastSelected;
        }
        private void UpdateSummary(DataTable dt)
        {
            var statusCounts = dt.AsEnumerable()
                             .GroupBy(row => row["Dept_Status"].ToString())  // Group by Dept_Status
                             .Select(group => new
                             {
                                 DeptStatus = group.Key,      // Dept_Status (e.g., Pending, Approved)
                                 TotalCount = group.Count()   // Count of occurrences of each Dept_Status
                             })
                             .ToList();

            // Initialize counters for each status type
            int pending = 0, approved = 0, rejected = 0;

            // Loop through the grouped status counts and update the counters based on the status
            foreach (var status in statusCounts)
            {
                switch (status.DeptStatus)
                {
                    case "Pending":
                        pending = status.TotalCount;
                        break;

                    case "Approved":
                        approved = status.TotalCount;
                        break;

                    case "Rejected":
                        rejected = status.TotalCount;
                        break;

                    default:
                        break;
                }
            }

            // Update the UI labels with the counts
            if (lblPendingCount != null)
                lblPendingCount.Text = pending.ToString();  // Update Pending count label

            if (lblApprovedCount != null)
                lblApprovedCount.Text = approved.ToString(); // Update Approved count label

            if (lblRejectedCount != null)
                lblRejectedCount.Text = rejected.ToString(); // Update Rejected count label
        }

        protected void rptRequisitions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Access the current Requisition_ID, Dept_Status, Store_Status, etc.
                int requisitionId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Requisition_ID"));
                string deptStatus = DataBinder.Eval(e.Item.DataItem, "Dept_Status").ToString();
                string storeStatus = DataBinder.Eval(e.Item.DataItem, "Store_Status").ToString();

                // Get the buttons from the Repeater item
                Button btnApprove = (Button)e.Item.FindControl("btnApprove");
                Button btnReject = (Button)e.Item.FindControl("btnReject");
                Button btnPending = (Button)e.Item.FindControl("btnPending");

                // Logic for displaying buttons based on Dept_Status and Store_Status
                if (btnApprove != null)
                {
                    btnApprove.Visible = deptStatus == "Pending";
                }

                if (btnReject != null)
                {
                    btnReject.Visible = deptStatus == "Pending";
                }

                if (btnPending != null)
                {
                    // Button for changing the status to "Pending" should only be visible if it's either Approved or Rejected, and Store_Status is Pending or NULL
                    btnPending.Visible = (deptStatus == "Approved" || deptStatus == "Rejected") &&
                                         (storeStatus == "Pending" || string.IsNullOrEmpty(storeStatus));
                }

                GridView gvItems = (GridView)e.Item.FindControl("gvItems");

                // Fetch the remarks data
                string status = DataBinder.Eval(e.Item.DataItem, "Dept_Status").ToString();
                string remarks = DataBinder.Eval(e.Item.DataItem, "Dept_Approval_Remarks").ToString();

                // Find the Remarks TextBox and set the text based on status
                TextBox txtRemarks = (TextBox)e.Item.FindControl("txtRemarks");

                if (status == "Approved" || status == "Rejected")
                {
                    // Show the remarks if status is approved or rejected
                    txtRemarks.Text = remarks;
                    txtRemarks.Enabled = false; // Disable the textbox for display-only
                }
                else
                {
                    txtRemarks.Enabled = true; // Enable textbox for editing if not Approved or Rejected
                }

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string query = @"
                        SELECT M.Materials_Name, C.Quantity
                        FROM Requisition_Item_Child C
                        INNER JOIN Material M ON C.Material_ID = M.Material_ID
                        WHERE C.Requisition_ID = @Requisition_ID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Requisition_ID", requisitionId);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dtItems = new DataTable();
                    da.Fill(dtItems);
                    gvItems.DataSource = dtItems;
                    gvItems.DataBind();
                }
            }
        }

        protected void rptRequisitions_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // Get the Requisition_ID from the CommandArgument
            int requisitionId = Convert.ToInt32(e.CommandArgument);

            // Get the status values for Dept_Status and Store_Status
            string deptStatus = (string)DataBinder.Eval(e.Item.DataItem, "Dept_Status");
            object storeStatusObj = DataBinder.Eval(e.Item.DataItem, "Store_Status");
            string storeStatus = string.Empty;

            // Check if storeStatusObj is DBNull or null
            if (storeStatusObj != DBNull.Value && storeStatusObj != null)
            {
                storeStatus = storeStatusObj.ToString();
            }

            // Depending on the command (Approve, Reject, or Pending), perform the update
            using (SqlConnection con = new SqlConnection(connStr))
            {
                // Prepare the SQL query for updating the requisition status
                string updateQuery = string.Empty;
                string status = string.Empty;
                string remarks = string.Empty;

                // Get the Remarks textbox from the Repeater Item
                TextBox txtRemarks = (TextBox)e.Item.FindControl("txtRemarks");
                remarks = txtRemarks?.Text ?? string.Empty;

                // Determine the action based on the CommandName
                if (e.CommandName == "Approve")
                {
                    status = "Approved";  // Set the status to "Approved"
                }
                else if (e.CommandName == "Reject")
                {
                    status = "Rejected";  // Set the status to "Rejected"
                }
                else if (e.CommandName == "Pending")
                {
                    status = "Pending";   // Set the status to "Pending"
                }

                // Only update if Store_Status is either "Pending" or NULL
                if (storeStatus == "Pending" || string.IsNullOrEmpty(storeStatus))
                {
                    updateQuery = @"
                        UPDATE Requisition_Parent
                        SET Dept_Status = @Status,
                            Dept_Approved_By = @ApprovedBy,
                            Dept_Approval_Remarks = @Remarks,
                            Dept_Approval_Date = GETDATE()
                        WHERE Requisition_ID = @Requisition_ID";

                    // Prepare the SQL command and add parameters
                    SqlCommand cmd = new SqlCommand(updateQuery, con);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@ApprovedBy", Convert.ToInt32(Session["EmployeeID"])); // Assuming EmployeeID is stored in the session
                    cmd.Parameters.AddWithValue("@Remarks", remarks);
                    cmd.Parameters.AddWithValue("@Requisition_ID", requisitionId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    // Reload the requisitions after updating the status
                    LoadRequisitions();
                }
                else
                {
                    // If the condition doesn't match (e.g., trying to approve a non-pending requisition), you can show a message or log it
                    // Optionally, you could display a message here or log an error to inform the user.
                    // For now, we are just returning early and not performing any update
                    return;
                }
            }
        }
    }
}