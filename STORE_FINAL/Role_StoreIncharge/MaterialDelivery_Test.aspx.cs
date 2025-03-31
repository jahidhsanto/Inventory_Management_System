using STORE_FINAL.Role_Admin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Role_StoreIncharge
{
    public partial class MaterialDelivery_Test : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRequisitions();
                LoadEmployees();
            }
        }

        private void LoadRequisitions()
        {
            string query = "SELECT Requisition_ID, CONCAT('Req#', Requisition_ID, ' - ', Status) AS ReqDetails FROM Requisition WHERE Status = 'Approved'";
            DataTable dt = GetData(query);

            ddlRequisition.DataSource = dt;
            ddlRequisition.DataTextField = "ReqDetails";
            ddlRequisition.DataValueField = "Requisition_ID";
            ddlRequisition.DataBind();

            ddlRequisition.Items.Insert(0, new ListItem("-- Select Requisition --", "0"));
        }
        private void LoadEmployees()
        {
            string query = "select Employee_ID, CONCAT(Employee_ID, ' - ', Name) AS Name from Employee";
            DataTable dt = GetData(query);

            ddlEmployee.DataSource = dt;
            ddlEmployee.DataTextField = "Name";
            ddlEmployee.DataValueField = "Employee_ID";
            ddlEmployee.DataBind();

            ddlEmployee.Items.Insert(0, new ListItem("-- Select Employee --", "0"));
        }
        private DataTable GetData(string query, SqlParameter[] parameters = null)
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        protected void ddlRequisition_SelectedIndexChanged(object sender, EventArgs e)
        {
            int requisitionID = Convert.ToInt32(ddlRequisition.SelectedValue);
            if (requisitionID > 0)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        string query = @"
                                SELECT DISTINCT M.Material_ID, M.Materials_Name, CONCAT(e.Employee_ID, ' - ', e.Name) AS Requested_By, R.Created_Date 
                                FROM Requisition R
                                INNER JOIN Material M ON R.Material_ID = M.Material_ID
                                JOIN Employee e ON R.Employee_ID = e.Employee_ID
                                WHERE R.Requisition_ID = @Requisition_ID;";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Requisition_ID", requisitionID);
                            conn.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    lblMaterialName.Text = reader["Materials_Name"].ToString();
                                    lblRequestedBy.Text = reader["Requested_By"].ToString();

                                    DateTime requestedDate;
                                    if (DateTime.TryParse(reader["Created_Date"].ToString(), out requestedDate))
                                    {
                                        lblRequestedDate.Text = requestedDate.ToString("dd-MMM-yyyy"); // Formats date
                                    }
                                    else
                                    {
                                        lblRequestedDate.Text = "-";
                                    }

                                    // Show material image
                                    //imgMaterial.ImageUrl = string.IsNullOrEmpty(reader["Material_Image"].ToString())
                                    //    ? "~/Images/no-image.png" : reader["Material_Image"].ToString();

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMaterialName.Text = "Error loading Requisition details.";
                    lblRequestedBy.Text = "-";
                    lblRequestedDate.Text = "-";
                    Console.WriteLine("Error: " + ex.Message);
                }

            }
        }


    }
}