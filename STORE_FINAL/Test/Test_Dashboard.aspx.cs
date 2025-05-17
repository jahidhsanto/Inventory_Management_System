using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace STORE_FINAL.Test
{
    public partial class Test_Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardData();
            }
        }
        private void LoadDashboardData()
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("sp_GetDashboardSummary", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                // 1. KPIs
                if (reader.Read())
                {
                    lblTotalItems.Text = reader["TotalItems"].ToString();
                    lblStockQty.Text = reader["StockQuantity"].ToString();
                    lblPendingReq.Text = reader["PendingRequisitions"].ToString();
                    lblDefectiveStock.Text = reader["DefectiveStock"].ToString();
                    lblStockValue.Text = reader["StockValue"].ToString();
                    lblLowStock.Text = reader["LowStock"].ToString();
                    lblOutOfStock.Text = reader["OutOfStock"].ToString();
                    lblReceivedToday.Text = reader["ReceivedToday"].ToString();
                    lblIssuedToday.Text = reader["IssuedToday"].ToString();
                }

                // 2. Stock Trend (Receive + Delivery)
                reader.NextResult();
                var stockLabels = new List<string>();
                var stockReceived = new List<int>();
                var stockDelivered = new List<int>();
                while (reader.Read())
                {
                    stockLabels.Add(reader["DayName"].ToString());
                    stockReceived.Add(Convert.ToInt32(reader["ReceivedQty"]));
                    stockDelivered.Add(Convert.ToInt32(reader["DeliveredQty"]));
                }

                JavaScriptSerializer js = new JavaScriptSerializer();
                hfStockTrendLabels.Value = js.Serialize(stockLabels);
                hfStockTrendData_Receive.Value = js.Serialize(stockReceived);
                hfStockTrendData_Delivery.Value = js.Serialize(stockDelivered);

                // 3. Requisition Status
                reader.NextResult();
                var reqLabels = new List<string>();
                var reqCounts = new List<int>();
                while (reader.Read())
                {
                    reqLabels.Add(reader["Store_Status"].ToString());
                    reqCounts.Add(Convert.ToInt32(reader["Count"]));
                }
                hfRequisitionLabels.Value = js.Serialize(reqLabels);
                hfRequisitionCounts.Value = js.Serialize(reqCounts);
            }
        }
    }
}