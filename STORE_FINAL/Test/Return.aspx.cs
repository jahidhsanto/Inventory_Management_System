using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Test
{
    public partial class Return : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlChallan.Visible = false;
                lblMessage.Text = "";
            }
        }

        protected void btnFetchChallan_Click(object sender, EventArgs e)
        {
            string challanID = txtChallanNumber.Text.Trim();
            if (string.IsNullOrEmpty(challanID))
            {
                lblMessage.Text = "⚠️ Please enter a Challan Number.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                SqlCommand cmdHeader = new SqlCommand(@"
                    SELECT TOP 1 Challan_ID, Challan_Date, E.Name AS CreatedBy
                    FROM Challan C
                    INNER JOIN Employee E ON C.Employee_ID = E.Employee_ID
                    WHERE Challan_ID = @Challan_ID", con);
                cmdHeader.Parameters.AddWithValue("@Challan_ID", challanID);

                SqlDataReader reader = cmdHeader.ExecuteReader();
                if (reader.Read())
                {
                    lblChallanNumber.Text = reader["Challan_ID"].ToString();
                    lblDate.Text = Convert.ToDateTime(reader["Challan_Date"]).ToString("dd-MM-yyyy");
                    lblDeliveredTo.Text = reader["CreatedBy"].ToString();
                    reader.Close();

                    SqlCommand cmdDetails = new SqlCommand(@"
                        SELECT M.Materials_Name, ISNULL(S.Serial_Number, '-') AS Serial_Number, C_I.Quantity
                        FROM Challan_Items C_I
                        INNER JOIN Material M ON C_I.Material_ID = M.Material_ID
                        LEFT JOIN Stock S ON C_I.Stock_ID = S.Stock_ID
                        WHERE C_I.Challan_ID = @Challan_ID", con);
                    cmdDetails.Parameters.AddWithValue("@Challan_ID", challanID);

                    SqlDataAdapter da = new SqlDataAdapter(cmdDetails);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptChallanDetails.DataSource = dt;
                    rptChallanDetails.DataBind();

                    pnlChallan.Visible = true;
                    lblMessage.Text = "";
                }
                else
                {
                    lblMessage.Text = "❌ Challan not found.";
                    pnlChallan.Visible = false;
                    reader.Close();
                }
            }
        }
    }
}