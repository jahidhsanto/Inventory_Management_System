using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;

namespace STORE_FINAL.Role_StoreIncharge.Report
{
    public partial class MaterialLedger : System.Web.UI.Page
    {
        string connStr = WebConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMaterials();
            }
        }
        private void LoadMaterials()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                            SELECT DISTINCT m.Material_ID, m.Materials_Name
                            FROM Material m
                            INNER JOIN Material_Ledger ml ON m.Material_ID = ml.Material_ID
                            ORDER BY m.Materials_Name;";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlMaterial.DataSource = dt;
                ddlMaterial.DataTextField = "Materials_Name";
                ddlMaterial.DataValueField = "Material_ID";
                ddlMaterial.DataBind();

                ddlMaterial.Items.Insert(0, new ListItem("-- Select Material --", "0"));
            }
        }

        protected void ddlMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblExportError.Text = "";
            int materialId = Convert.ToInt32(ddlMaterial.SelectedValue);
            if (materialId > 0)
            {
                LoadMaterialLedger(materialId);
            }
            else
            {
                gvMaterialLedger.DataSource = null;
                gvMaterialLedger.DataBind();
            }
        }

        private void LoadMaterialLedger(int materialId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            string query = @"
                            SELECT Challan_Date, Ledger_Type, In_Quantity, Out_Quantity, 
                            Unit_Price, Balance_After_Transaction, Valuation_After_Transaction
                            FROM Material_Ledger
                            WHERE Material_ID = @Material_ID";

            if (fromDate.HasValue)
                query += " AND Challan_Date >= @FromDate";

            if (toDate.HasValue)
                query += " AND Challan_Date <= @ToDate";

            query += " ORDER BY Challan_Date";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Material_ID", materialId);

                    if (fromDate.HasValue)
                        cmd.Parameters.AddWithValue("@FromDate", fromDate.Value);

                    if (toDate.HasValue)
                        cmd.Parameters.AddWithValue("@ToDate", toDate.Value);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvMaterialLedger.DataSource = dt;
                    gvMaterialLedger.DataBind();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int materialId = Convert.ToInt32(ddlMaterial.SelectedValue);
            if (materialId > 0)
            {
                DateTime? fromDate = null;
                DateTime? toDate = null;

                if (DateTime.TryParse(txtFromDate.Text, out DateTime fDate))
                    fromDate = fDate;

                if (DateTime.TryParse(txtToDate.Text, out DateTime tDate))
                    toDate = tDate;

                // Validate that both dates are provided and fromDate <= toDate
                if (fromDate.HasValue && toDate.HasValue && fromDate > toDate)
                {
                    lblExportError.Text = "From Date must be earlier than or equal to To Date.";
                    lblExportError.Visible = true;

                    gvMaterialLedger.DataSource = null;
                    gvMaterialLedger.DataBind();
                    return;
                }

                lblExportError.Visible = false;
                LoadMaterialLedger(materialId, fromDate, toDate);
            }
            else
            {
                gvMaterialLedger.DataSource = null;
                gvMaterialLedger.DataBind();
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            int materialId;
            if (!int.TryParse(ddlMaterial.SelectedValue, out materialId) || materialId == 0)
            {
                // Show error or handle gracefully
                lblExportError.Text = "Please select a material before exporting.";
                lblExportError.Visible = true;
                return;
            }

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=MaterialLedger.xlsx");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                gvMaterialLedger.AllowPaging = false;
                DateTime? fromDate = null, toDate = null;
                if (DateTime.TryParse(txtFromDate.Text, out DateTime fDate)) fromDate = fDate;
                if (DateTime.TryParse(txtToDate.Text, out DateTime tDate)) toDate = tDate;
                LoadMaterialLedger(materialId, fromDate, toDate);

                gvMaterialLedger.RenderControl(hw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        public override void VerifyRenderingInServerForm(Control control) { }
    }
}