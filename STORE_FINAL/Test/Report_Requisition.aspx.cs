using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Test
{
    public partial class Report_Requisition : System.Web.UI.Page
    {
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCategoryRepeater();
                LoadProjects();
                LoadRequisitionStatusSummary();
                LoadRequisitionsByEmployee();
                LoadRequisitionsByRecipientType();
                LoadMonthlySummary();
                LoadMaterialConsumptionSummary();
                LoadRequisitionsByProject();
                LoadStatusPivot();
                LoadRequisitionsByZone();
                LoadStoreDeliveryStatus();
            }
        }
        //=========================================================================================
        private void BindCategoryRepeater()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT Requisition_For, COUNT(*) AS TotalRequests
                    FROM Requisition
                    GROUP BY Requisition_For
                ";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptCategories.DataSource = dt;
                rptCategories.DataBind();
            }
        }

        protected void rptCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string requisitionFor = DataBinder.Eval(e.Item.DataItem, "Requisition_For").ToString();
                GridView gvDetails = (GridView)e.Item.FindControl("gvDetails");

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string query = @"
                        SELECT Requisition_Type, COUNT(*) AS Count, SUM(Quantity) AS Total_Quantity
                        FROM Requisition
                        WHERE Requisition_For = @Requisition_For
                        GROUP BY Requisition_Type
                    ";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Requisition_For", requisitionFor);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvDetails.DataSource = dt;
                    gvDetails.DataBind();
                }
            }
        }
        //=========================================================================================


        //=========================================================================================
        private void LoadProjects()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "SELECT DISTINCT Project_Code_For FROM Requisition ORDER BY Project_Code_For";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                ddlProject.DataSource = cmd.ExecuteReader();
                ddlProject.DataTextField = "Project_Code_For";
                ddlProject.DataValueField = "Project_Code_For";
                ddlProject.DataBind();
                ddlProject.Items.Insert(0, new ListItem("-- Select Project --", ""));
            }
        }

        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProject = ddlProject.SelectedValue;
            if (!string.IsNullOrEmpty(selectedProject))
            {
                LoadRequisitionTypeByProject(selectedProject);
            }
        }

        private void LoadRequisitionTypeByProject(string projectCode)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"SELECT Requisition_Type, COUNT(*) AS Total, SUM(Quantity) AS TotalQuantity 
                                 FROM Requisition
                                 WHERE Project_Code_For = @ProjectCode 
                                 GROUP BY Requisition_Type";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProjectCode", projectCode);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvRequisitionType.DataSource = dt;
                gvRequisitionType.DataBind();
            }
        }
        //=========================================================================================

        //=========================================================================================
        private void LoadRequisitionStatusSummary()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"SELECT Status, COUNT(*) AS TotalRequisitions
                         FROM Requisition
                         GROUP BY Status";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvRequisitionStatus.DataSource = dt;
                gvRequisitionStatus.DataBind();
            }
        }

        //=========================================================================================

        //=========================================================================================
        private void LoadRequisitionsByEmployee()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"SELECT Employee_ID, COUNT(*) AS TotalRequisitions
                         FROM Requisition
                         GROUP BY Employee_ID";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvRequisitionsByEmployee.DataSource = dt;
                gvRequisitionsByEmployee.DataBind();
            }
        }
        //=========================================================================================

        //=========================================================================================
        private void LoadRequisitionsByRecipientType()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"SELECT Requisition_For, COUNT(*) AS TotalRequisitions
                         FROM Requisition
                         GROUP BY Requisition_For";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvRequisitionsByRecipientType.DataSource = dt;
                gvRequisitionsByRecipientType.DataBind();
            }
        }
        //=========================================================================================

        //=========================================================================================
        private void LoadMonthlySummary()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"SELECT FORMAT(Created_Date, 'yyyy-MM') AS Month, COUNT(*) AS TotalRequisitions, SUM(Quantity) AS TotalQuantity
                         FROM Requisition
                         GROUP BY FORMAT(Created_Date, 'yyyy-MM')
                         ORDER BY Month";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvMonthlySummary.DataSource = dt;
                gvMonthlySummary.DataBind();
            }
        }
        //=========================================================================================

        //=========================================================================================
        private void LoadMaterialConsumptionSummary()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"SELECT M.Materials_Name, SUM(Quantity) AS TotalConsumed
                         FROM Requisition R
						 JOIN Material M ON R.Material_ID = M.Material_ID
						 GROUP BY M.Materials_Name";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvMaterialConsumption.DataSource = dt;
                gvMaterialConsumption.DataBind();
            }
        }
        //=========================================================================================

        //=========================================================================================
        private void LoadRequisitionsByProject()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"SELECT Project_Code_For, COUNT(*) AS TotalRequisitions
                         FROM Requisition
                         GROUP BY Project_Code_For";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvRequisitionsByProject.DataSource = dt;
                gvRequisitionsByProject.DataBind();
            }
        }
        //=========================================================================================

        //=========================================================================================
        private void LoadStatusPivot()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"SELECT Status, COUNT(*) AS TotalRequisitions
                         FROM Requisition
                         GROUP BY Status";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvStatusPivot.DataSource = dt;
                gvStatusPivot.DataBind();
            }
        }
        //=========================================================================================

        //=========================================================================================
        private void LoadRequisitionsByZone()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"SELECT Z.Zone_Name, COUNT(*) AS TotalRequisitions
                         FROM Requisition R
						 JOIN Zone Z ON R.Zone_ID_For = Z.Zone_ID
						 GROUP BY Z.Zone_Name";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvRequisitionsByZone.DataSource = dt;
                gvRequisitionsByZone.DataBind();
            }
        }
        //=========================================================================================

        //=========================================================================================
        private void LoadStoreDeliveryStatus()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"SELECT Store_Status, COUNT(*) AS TotalDeliveries
                         FROM Requisition
                         GROUP BY Store_Status";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvDeliveryStatus.DataSource = dt;
                gvDeliveryStatus.DataBind();
            }
        }
        //=========================================================================================


    }
}