using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Forms
{
	public partial class RequisitionForm : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            ClearFields();

            if (!IsPostBack)
            {
                LoadEmployee();
                LoadMaterials();
            }
        }
        private void LoadEmployee()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT Employee_ID, Name, CONCAT(Employee_ID,' - ', Name) AS ID_Name
                                FROM Employee
                                ORDER BY Employee_ID ASC;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlEmployeeName.DataSource = reader;
                ddlEmployeeName.DataTextField = "ID_Name";
                ddlEmployeeName.DataValueField = "Employee_ID";
                ddlEmployeeName.DataBind();
            }

            ddlEmployeeName.Items.Insert(0, new ListItem("-- Select Employee --", "0"));

        }

        private void LoadMaterials()
        {
            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT Material_ID, Materials_Name 
                                FROM Material;";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlMaterials.DataSource = reader;
                ddlMaterials.DataTextField = "Materials_Name";
                ddlMaterials.DataValueField = "Material_ID";
                ddlMaterials.DataBind();
            }

            ddlMaterials.Items.Insert(0, new ListItem("-- Select Material --", "0"));
        }


        protected void btnSubmitRequisition_Click(object sender, EventArgs e)
        {
            int employeeID = int.Parse(ddlEmployeeName.SelectedValue);
            int materialID = int.Parse(ddlMaterials.SelectedValue);
            int quantity;

            if (materialID == 0 || !int.TryParse(txtQuantity.Text, out quantity) || quantity <= 0)
            {
                lblMessage.Text = "Please enter valid values.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "INSERT INTO Requisition (Employee_ID, Material_ID, Quantity, Status) " +
                               "VALUES (@EmployeeID, @MaterialID, @Quantity, @Status)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    cmd.Parameters.AddWithValue("@MaterialID", materialID);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@Status", "Pending");  // Set the Status to "Pending"

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        lblMessage.Text = "Requisition submitted successfully!";
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = ex.Message;
                    }
                }
            }
        }

        private void ClearFields()
        {
            ddlEmployeeName.SelectedIndex = 0;
            ddlMaterials.SelectedIndex = 0;
            txtQuantity.Text = "";
        }

    }
}