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
                BindRequisitions();
            }
        }
        private void BindRequisitions()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Requisition_Parent", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvRequisitions.DataSource = dt;
                gvRequisitions.DataBind();
            }
        }

        protected void gvRequisitions_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "View")
            {
                ShowDetails(id);
            }
            else if (e.CommandName == "Approve")
            {
                UpdateStatus(id, "Approved");
                BindRequisitions();
            }
            else if (e.CommandName == "Reject")
            {
                UpdateStatus(id, "Rejected");
                BindRequisitions();
            }
        }

        private void ShowDetails(int id)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Requisition_Item_Child WHERE Requisition_ID=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lblModalContent.Text = $"<strong>Requisition No:</strong> {dr["Requisition_ID"]}";
                    //lblModalContent.Text = $"<strong>Requisition No:</strong> {dr["Requisition_ID"]}<br/>" +
                    //                       $"<strong>Date:</strong> {Convert.ToDateTime(dr["Created_Date"]).ToString("yyyy-MM-dd")}<br/>" +
                    //                       $"<strong>Requested By:</strong> {dr["CreatedByEmployee_ID"]}<br/>" +
                    //                       $"<strong>Description:</strong><br/>{dr["Description"]}";
                }
                conn.Close();
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
        }

        private void UpdateStatus(int id, string status)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Requisitions SET Status=@Status WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}