using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Role_StoreIncharge.Report
{
    public partial class Report_01 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadReport();
            }
        }

        private void LoadReport()
        {
            // Set the processing mode for the ReportViewer to Local or Remote
            ReportViewer1.ProcessingMode = ProcessingMode.Local;

            // Path to your RDLC file
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/Report01.rdlc");

            // Get your data source (example: a DataTable or list)
            var dt = GetReportData(); // implement this method to fetch data

            // Clear previous data sources
            ReportViewer1.LocalReport.DataSources.Clear();

            // Add new data source
            ReportDataSource rds = new ReportDataSource("DataSet1", dt);
            ReportViewer1.LocalReport.DataSources.Add(rds);

            // Refresh the report
            ReportViewer1.LocalReport.Refresh();
        }

    }
}