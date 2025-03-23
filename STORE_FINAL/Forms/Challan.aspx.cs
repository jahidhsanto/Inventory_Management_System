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
	public partial class Challan : System.Web.UI.Page
	{
        string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                LoadApprovedRequisitions();
            }
        }

        private void LoadApprovedRequisitions()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Requisition_ID FROM Requisition WHERE Status = 'Approved'";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlApprovedRequisitions.DataSource = dt;
                ddlApprovedRequisitions.DataTextField = "Requisition_ID";
                ddlApprovedRequisitions.DataValueField = "Requisition_ID";
                ddlApprovedRequisitions.DataBind();
            }
        }

        protected void btnDeliver_Click(object sender, EventArgs e)
        {
            int requisitionId = Convert.ToInt32(ddlApprovedRequisitions.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string updateStock = @"
                UPDATE Stock SET Status = 'Delivered'
                WHERE Material_ID = (SELECT Material_ID FROM Requisition WHERE Requisition_ID = @RequisitionID)
                AND Status = 'Available'
                ORDER BY Received_Date ASC
                OFFSET 0 ROWS FETCH NEXT (SELECT Quantity FROM Requisition WHERE Requisition_ID = @RequisitionID) ROWS ONLY;";

                SqlCommand cmdStock = new SqlCommand(updateStock, conn);
                cmdStock.Parameters.AddWithValue("@RequisitionID", requisitionId);
                cmdStock.ExecuteNonQuery();

                string insertChallan = @"
                INSERT INTO Challan (Requisition_ID, Employee_ID, Delivery_Date, Status, Remarks)
                VALUES (@RequisitionID, @EmployeeID, GETDATE(), 'Dispatched', 'Material Delivered');";

                SqlCommand cmdChallan = new SqlCommand(insertChallan, conn);
                cmdChallan.Parameters.AddWithValue("@RequisitionID", requisitionId);
                cmdChallan.Parameters.AddWithValue("@EmployeeID", Session["UserID"]);
                cmdChallan.ExecuteNonQuery();
            }
        }

        protected void ddlApprovedRequisitions_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get selected requisition ID
            int requisitionId = Convert.ToInt32(ddlApprovedRequisitions.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = @"
            SELECT Serial_Number, Status 
            FROM Stock 
            WHERE Material_ID = (SELECT Material_ID FROM Requisition WHERE Requisition_ID = @RequisitionID)
            AND Status = 'Available'";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@RequisitionID", requisitionId);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvStockDetails.DataSource = dt;
                gvStockDetails.DataBind();
            }
        }
    }
}