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
    public partial class RequisitionList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null || Session["Role"] == null ||
                Session["Role"].ToString() != "Store InCharge")
            {
                Response.Redirect("~/");
            }

            if (!IsPostBack)
            {
                LoadProjects("All");
                LoadEmployees("All");

                LoadRequisitions("All", "All", "All");
            }
        }

        private void LoadRequisitions(string status, string projectCode, string requestedBy_EmployeeID)
        {
            string query = @"
                            SELECT 
                                r.Requisition_ID, m.Materials_Name, r.Quantity, 
	                            r.Created_Date, emp.Name AS Requested_By, r.Status AS Dept_Status, 
                                r.Store_Status, eh.Name AS Dept_Head, r.Store_Status_By, 
	                            SUM(s.Quantity) AS Stock_Quantity
                            FROM requisition r
                            JOIN Material m 
                                ON r.Material_ID = m.Material_ID
                            LEFT JOIN Employee emp 
                                ON r.Employee_ID = emp.Employee_ID
                            LEFT JOIN Department d 
                                ON emp.Department_ID = d.Department_ID
                            LEFT JOIN Employee eh 
                                ON d.Department_Head_ID = eh.Employee_ID
                            JOIN Stock s
	                            ON r.Material_ID = s.Material_ID
                            WHERE r.Status = 'Approved'";

            if (projectCode != "All")
            {
                query += " AND r.Project_Code = @ProjectCode";
            }

            if (requestedBy_EmployeeID != "All")
            {
                query += " AND r.Employee_ID = @RequestedBy_EmployeeID";
            }

            if (status != "All")
            {
                query += " AND r.Store_Status = @Status";
            }

            query += " GROUP BY r.Requisition_ID, m.Materials_Name, r.Quantity, " +
                "r.Created_Date, emp.Name, r.Status, " +
                "r.Store_Status, eh.Name, r.Store_Status_By;";

            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (projectCode != "All")
                    {
                        cmd.Parameters.AddWithValue("@ProjectCode", projectCode);
                    }

                    if (requestedBy_EmployeeID != "All")
                    {
                        cmd.Parameters.AddWithValue("@RequestedBy_EmployeeID", requestedBy_EmployeeID);
                    }

                    if (status != "All")
                    {
                        cmd.Parameters.AddWithValue("@Status", status);
                    }

                    conn.Open();
                    RequisitionApprovalGridView.DataSource = cmd.ExecuteReader();
                    RequisitionApprovalGridView.DataBind();
                }
            }
        }

        private void LoadProjects(String employeeID)
        {
            string query = @"
                            SELECT DISTINCT p.Project_Code, p.Project_Name
                            FROM Requisition r
                            JOIN Project p ON r.Project_Code = p.Project_Code";

            if (employeeID != "All")
            {
                query += " WHERE r.Employee_ID = @EmployeeID";
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (employeeID != "All")
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
                    }

                    conn.Open();
                    ddlProject.DataSource = cmd.ExecuteReader();
                    ddlProject.DataTextField = "Project_Name";
                    ddlProject.DataValueField = "Project_Code";
                    ddlProject.DataBind();
                }

                ddlProject.Items.Insert(0, new ListItem("-- All Projects --", "All"));
            }
        }

        private void LoadEmployees(string projectCode)
        {
            string query = @"
                    SELECT DISTINCT e.Employee_ID, e.Name
                    FROM Requisition r
                    JOIN Employee e ON r.Employee_ID = e.Employee_ID";

            if (projectCode != "All")
            {
                query += " WHERE r.Project_Code = @ProjectCode";
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (projectCode != "All")
                    {
                        cmd.Parameters.AddWithValue("@ProjectCode", projectCode);
                    }

                    conn.Open();
                    ddlEmployee.DataSource = cmd.ExecuteReader();
                    ddlEmployee.DataTextField = "Name";
                    ddlEmployee.DataValueField = "Employee_ID";
                    ddlEmployee.DataBind();
                }

                ddlEmployee.Items.Insert(0, new ListItem("-- All Employees --", "All"));
            }
        }

        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedProjectCode = ddlProject.SelectedValue;

            // Load employees based on the selected project or all if no project selected
            LoadEmployees(selectedProjectCode);
        }
        protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedEmployeeID = ddlEmployee.SelectedValue;

            // Load projects based on the selected employee or all if no project selected
            LoadProjects(selectedEmployeeID);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            string selectedStatus = ddlStoreStatus.SelectedValue;
            string selectedProject = ddlProject.SelectedValue;
            string selectedEmployee = ddlEmployee.SelectedValue;

            LoadRequisitions(selectedStatus, selectedProject, selectedEmployee);
        }

        protected void ApproveRequisitionGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Approve" || e.CommandName == "Reject" ||
                e.CommandName == "Pending" || e.CommandName == "Processing" ||
                e.CommandName == "OutOfStock" || e.CommandName == "Ordered" ||
                e.CommandName == "Delivered")
            {
                int requisitionID = Convert.ToInt32(e.CommandArgument);
                string newStatus = "";

                switch (e.CommandName)
                {
                    case "Approve":
                        newStatus = "Approved";
                        break;
                    case "Reject":
                        newStatus = "Rejected";
                        break;
                    case "Pending":
                        newStatus = "Pending";
                        break;
                    case "Processing":
                        newStatus = "Processing";
                        break;
                    case "OutOfStock":
                        newStatus = "Out of Stock";
                        break;
                    case "Ordered":
                        newStatus = "Ordered";
                        break;
                    case "Delivered":
                        newStatus = "Delivered";
                        break;
                }

                UpdateRequisitionStatus(requisitionID, newStatus);

                // Reload filtered data
                string selectedStatus = ddlStoreStatus.SelectedValue;
                string selectedProject = ddlProject.SelectedValue;
                string selectedEmployee = ddlEmployee.SelectedValue;

                LoadRequisitions(selectedStatus, selectedProject, selectedEmployee);
            }
        }

        private void UpdateRequisitionStatus(int requisitionID, string status)
        {
            int storeInchargeID = Convert.ToInt32(Session["EmployeeID"]);

            string query = @"
                            UPDATE Requisition 
                            SET Store_Status = @StoreStatus, Store_Status_By = @StoreInchargeID
                            WHERE Requisition_ID = @RequisitionID";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StoreStatus", status);
                    cmd.Parameters.AddWithValue("@StoreInchargeID", storeInchargeID);
                    cmd.Parameters.AddWithValue("@RequisitionID", requisitionID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            LoadRequisitions("All", "All", "All"); // Refresh the list
        }
    }
}