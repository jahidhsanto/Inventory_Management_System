using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Test
{
    public partial class PurposeWiseReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateDropdowns();
                LoadPurposeWiseData();
            }
        }
        private void PopulateDropdowns()
        {
            ddlMonth.DataSource = Enumerable.Range(1, 12).Select(m => new { Value = m, Name = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m) });
            ddlMonth.DataTextField = "Name";
            ddlMonth.DataValueField = "Value";
            ddlMonth.DataBind();
            ddlMonth.SelectedValue = DateTime.Now.Month.ToString();

            ddlYear.DataSource = Enumerable.Range(2023, 5);
            ddlYear.DataBind();
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            LoadPurposeWiseData();
        }

        private void LoadPurposeWiseData()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("usp_GetPurposeWiseInOut", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Month", ddlMonth.SelectedValue);
                cmd.Parameters.AddWithValue("@Year", ddlYear.SelectedValue);
                DataTable dt = new DataTable();
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
                gvPurposeWise.DataSource = dt;
                gvPurposeWise.DataBind();
            }
        }
    }
}