using STORE_FINAL.Role_Admin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Role_StoreIncharge
{
    public partial class PrintChallan : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Fetch and display the challan details
                string sessionID = Request.QueryString["SessionID"];
                if (string.IsNullOrEmpty(sessionID))
                {
                    //Response.Redirect("~/Role_StoreIncharge/MaterialDelivery.aspx");
                }
                LoadChallanDetails("a1eb3577-9ad3-42ca-a9f0-97ee9e1c75a4");
            }

        }
        private void LoadChallanDetails(string sessionID)
        {
            // Fetch the delivery items for this session
            string query = @"SELECT T.Temp_ID, M.Materials_Name, S.Serial_Number, T.Delivered_Quantity 
                             FROM Temp_Delivery T
                             INNER JOIN Material M ON T.Material_ID = M.Material_ID
                             INNER JOIN Stock S ON T.Stock_ID = S.Stock_ID
                             WHERE T.Session_ID = @Session_ID";

            SqlParameter[] parameters = { new SqlParameter("@Session_ID", sessionID) };
            DataTable dtDeliveryItems = GetData(query, parameters);

            // Display challan number and date
            string challanNumber = "CH" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string deliveryDate = DateTime.Now.ToString("dd-MMM-yyyy");

            //challanNumberLabel.Text = challanNumber;
            //deliveryDateLabel.Text = deliveryDate;

            // Bind the delivery items to the table
            if (dtDeliveryItems.Rows.Count > 0)
            {
                int index = 1;
                foreach (DataRow row in dtDeliveryItems.Rows)
                {
                    string materialName = row["Materials_Name"].ToString();
                    string serialNumber = row["Serial_Number"].ToString();
                    string quantity = row["Delivered_Quantity"].ToString();

                    // Add rows to the table dynamically
                    TableRow tableRow = new TableRow();

                    tableRow.Cells.Add(new TableCell() { Text = index.ToString() });
                    tableRow.Cells.Add(new TableCell() { Text = materialName });
                    tableRow.Cells.Add(new TableCell() { Text = serialNumber });
                    tableRow.Cells.Add(new TableCell() { Text = quantity });

                    //challanItems.Controls.Add(tableRow);
                    index++;
                }
            }
            else
            {
                Response.Write("No items found for this delivery session.");
            }
        }

        private DataTable GetData(string query, SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }
    }
}