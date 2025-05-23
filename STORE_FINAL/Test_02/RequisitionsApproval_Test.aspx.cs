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
            if (!IsPostBack)
            {
                BindEmployeeDropdown();
                LoadRequisitions();
            }
        }
        private void BindEmployeeDropdown()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "SELECT DISTINCT E.Employee_ID, E.Name FROM Employee E INNER JOIN Requisition_Parent R ON E.Employee_ID = R.CreatedByEmployee_ID";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlEmployeeFilter.DataSource = dt;
                ddlEmployeeFilter.DataTextField = "Name";
                ddlEmployeeFilter.DataValueField = "Employee_ID";
                ddlEmployeeFilter.DataBind();
                ddlEmployeeFilter.Items.Insert(0, new ListItem("All Employees", ""));
            }
        }
        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRequisitions();
        }
        private void LoadRequisitions()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string baseQuery = @"
            SELECT R.Requisition_ID, E.Name AS CreatedBy, R.Created_Date, R.Requisition_For, R.Dept_Status, R.Dept_Approval_Remarks
            FROM Requisition_Parent R
            INNER JOIN Employee E ON R.CreatedByEmployee_ID = E.Employee_ID
            WHERE 1 = 1";

                if (!string.IsNullOrEmpty(ddlStatusFilter.SelectedValue))
                    baseQuery += " AND R.Dept_Status = @Status";

                if (!string.IsNullOrEmpty(ddlEmployeeFilter.SelectedValue))
                    baseQuery += " AND R.CreatedByEmployee_ID = @Employee_ID";

                if (!string.IsNullOrEmpty(txtSearchRequisitionID.Text))
                    baseQuery += " AND R.Requisition_ID LIKE @Requisition_ID";

                SqlCommand cmd = new SqlCommand(baseQuery, con);
                if (!string.IsNullOrEmpty(ddlStatusFilter.SelectedValue))
                    cmd.Parameters.AddWithValue("@Status", ddlStatusFilter.SelectedValue);
                if (!string.IsNullOrEmpty(ddlEmployeeFilter.SelectedValue))
                    cmd.Parameters.AddWithValue("@Employee_ID", ddlEmployeeFilter.SelectedValue);
                if (!string.IsNullOrEmpty(txtSearchRequisitionID.Text))
                    cmd.Parameters.AddWithValue("@Requisition_ID", "%" + txtSearchRequisitionID.Text + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                rptRequisitions.DataSource = dt;
                rptRequisitions.DataBind();

                UpdateSummaryCounts(con);
            }
        }

        private void UpdateSummaryCounts(SqlConnection con)
        {
            string countQuery = @"
        SELECT Dept_Status, COUNT(*) AS Total
        FROM Requisition_Parent
        GROUP BY Dept_Status";

            SqlCommand cmd = new SqlCommand(countQuery, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            int pending = 0, approved = 0, rejected = 0;

            foreach (DataRow row in dt.Rows)
            {
                string status = row["Dept_Status"].ToString();
                int count = Convert.ToInt32(row["Total"]);
                if (status == "Pending") pending = count;
                else if (status == "Approved") approved = count;
                else if (status == "Rejected") rejected = count;
            }

            RepeaterItem header = rptRequisitions.Controls.OfType<RepeaterItem>().FirstOrDefault(i => i.ItemType == ListItemType.Header);
            if (header != null)
            {
                Label lblPendingCount = (Label)header.FindControl("lblPendingCount");
                Label lblApprovedCount = (Label)header.FindControl("lblApprovedCount");
                Label lblRejectedCount = (Label)header.FindControl("lblRejectedCount");

                if (lblPendingCount != null) lblPendingCount.Text = pending.ToString();
                if (lblApprovedCount != null) lblApprovedCount.Text = approved.ToString();
                if (lblRejectedCount != null) lblRejectedCount.Text = rejected.ToString();
            }
        }


        protected void rptRequisitions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int requisitionId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Requisition_ID"));
                GridView gvItems = (GridView)e.Item.FindControl("gvItems");

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
            if (e.CommandName == "Approve" || e.CommandName == "Reject")
            {
                int requisitionId = Convert.ToInt32(e.CommandArgument);
                string status = (e.CommandName == "Approve") ? "Approved" : "Rejected";
                TextBox txtRemarks = (TextBox)e.Item.FindControl("txtRemarks");
                string remarks = txtRemarks?.Text ?? "";
                int approvedBy = Convert.ToInt32(Session["EmployeeID"]);

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string query = @"
                UPDATE Requisition_Parent
                SET Dept_Status = @Status,
                    Dept_Approved_By = @ApprovedBy,
                    Dept_Approval_Remarks = @Remarks,
                    Dept_Approval_Date = GETDATE()
                WHERE Requisition_ID = @Requisition_ID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@ApprovedBy", approvedBy);
                    cmd.Parameters.AddWithValue("@Remarks", remarks);
                    cmd.Parameters.AddWithValue("@Requisition_ID", requisitionId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                LoadRequisitions(); // reload after action
            }
        }
    }
}