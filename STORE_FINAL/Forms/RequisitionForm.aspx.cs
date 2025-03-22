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
            // Check if the session values exist, otherwise redirect to login page
            if (Session["Username"] == null)
            {
                Response.Redirect("~/");
            }

            if (!IsPostBack)
            {
                RESET();
                LoadMaterials();
            }
        }

        protected void btnSubmitRequisition_Click(object sender, EventArgs e)
        {
            string employeeID = Session["EmployeeID"]?.ToString();
            string departmentID = Session["EmployeeDepartmentID"]?.ToString();

            string materialID = ddlMaterials.SelectedValue;
            string quantity = txtQuantity.Text.Trim();

            if (string.IsNullOrEmpty(materialID) || string.IsNullOrEmpty(quantity))
            {
                lblMessage.Text = "All fields are required.";
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                INSERT INTO Requisition (Employee_ID, Material_ID, Department_ID, Quantity, Status) 
                                VALUES (@EmployeeID, @MaterialID, @DepartmentID, @Quantity, @Status)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
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

        protected void RESET()
        {
            ddlMaterials.SelectedIndex = 0;
            txtQuantity.Text = "";
        }
    }
}