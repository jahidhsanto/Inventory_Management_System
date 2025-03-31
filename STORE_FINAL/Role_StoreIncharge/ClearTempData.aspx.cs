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
    public partial class ClearTempData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sessionID = Request.QueryString["sessionID"];
            if (!string.IsNullOrEmpty(sessionID))
            {
                string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "DELETE FROM Temp_Delivery WHERE Session_ID = @Session_ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Session_ID", sessionID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}