using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace STORE_FINAL.Pages
{
    public partial class StockReview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the session values exist, otherwise redirect to login page
            //if (Session["Username"] == null)
            //{
            //    Response.Redirect("~/");
            //}

            if (!IsPostBack)
            {
                LoadRequisitions("All"); // Load all requisitions on first load
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            string selectedStatus = ddlStatus.SelectedValue;
            LoadRequisitions(selectedStatus);
        }

        protected void ApproveRequisitionGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Ordered" || e.CommandName == "Out of Stock" || e.CommandName == "Delivered")
            {
                int requisitionID = Convert.ToInt32(e.CommandArgument);
                string newStatus = "";

                if (e.CommandName == "Ordered")
                    newStatus = "Ordered";
                else if (e.CommandName == "Out of Stock")
                    newStatus = "Out of Stock";
                else if (e.CommandName == "Delivered")
                    newStatus = "Delivered";

                UpdateRequisitionStatus(requisitionID, newStatus);

                // Reload filtered data
                string selectedStatus = ddlStatus.SelectedValue;
                LoadRequisitions(selectedStatus);
            }
        }

        private void UpdateRequisitionStatus(int requisitionID, string status)
        {
            string query = @"
                            UPDATE Requisition 
                            SET Store_Status = @Status 
                            WHERE Requisition_ID = @RequisitionID";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@RequisitionID", requisitionID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            LoadRequisitions("All");
        }

        private void LoadRequisitions(string status)
        {
            string query = @"
                            select R.Requisition_ID,
                                E.Name AS Requested_By, 
                                M.Materials_Name AS Material_Name, 
                                R.Quantity, 
                                R.Created_Date, 
                                R.Status,
                                R.Store_Status,
	                            (SELECT SUM(Stock_Quantity) FROM Material WHERE Material_ID = R.Material_ID) AS Stock_Quantity
                            from Requisition R
                            JOIN Employee E ON R.Employee_ID = E.Employee_ID
                            JOIN Material M ON R.Material_ID = M.Material_ID
                            WHERE 1=1";

            if (status != "All")
            {
                query += " AND R.Status = @Status";
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (status != "All")
                        cmd.Parameters.AddWithValue("@Status", status);

                    conn.Open();
                    ApproveRequisitionGridView.DataSource = cmd.ExecuteReader();
                    ApproveRequisitionGridView.DataBind();
                }
            }
        }
    }
}