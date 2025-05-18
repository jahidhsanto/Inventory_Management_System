using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Test
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardReports();
            }
        }
        private void LoadDashboardReports()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("usp_GetDashboardReportSummary", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblPurposeWise.Text = reader["PurposeWiseInOut"].ToString();
                            lblNonMoving.Text = reader["NonMoving"].ToString();
                            lblChallanPending.Text = reader["ChallanPending"].ToString();
                            lblTestPending.Text = reader["TestPending"].ToString();
                            lblDefective.Text = reader["Defective"].ToString();
                            lblPR.Text = reader["PRCount"].ToString();
                        }
                    }
                }
            }
        }
    }
}