using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Role_DepartmentHead
{
    public partial class RequisitionApproval : System.Web.UI.Page
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
                LoadProjects("All");
                LoadEmployees("All");

                LoadRequisitions("All", "All", "All"); // Load all requisitions on first load
            }
        }

        private void LoadRequisitions(string status, string projectCode, string requestedBy_EmployeeID)
        {
            int employeeID = Convert.ToInt32(Session["EmployeeID"]); // Logged-in Department Head

            string query = @"
                        SELECT 
                            r.Requisition_ID, 
	                        m.Materials_Name, 
                            r.Quantity, 
                            CONCAT(p.Department, ' - ', p.Project_Name) Project_Name,
	                        r.Created_Date, 
                            emp.Name AS Requested_By,
                            r.Status AS Dept_Status, 
                            r.Store_Status, 
                            eh.Name AS Dept_Head
                        FROM requisition r
                        JOIN Material m 
                            ON r.Material_ID = m.Material_ID
						LEFT JOIN Project p
							ON r.Project_Code = p.Project_Code
                        LEFT JOIN Employee emp 
                            ON r.Employee_ID = emp.Employee_ID
                        LEFT JOIN Department d 
                            ON emp.Department_ID = d.Department_ID
                        LEFT JOIN Employee eh 
                            ON d.Department_Head_ID = eh.Employee_ID
                        WHERE emp.Department_ID = (
                            SELECT Department_ID FROM Employee WHERE Employee_ID = @EmployeeID)";

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
                query += " AND r.Status = @Status";
            }

            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
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
            string selectedStatus = ddlStatus.SelectedValue;
            string selectedProject = ddlProject.SelectedValue;
            string selectedEmployee = ddlEmployee.SelectedValue;

            LoadRequisitions(selectedStatus, selectedProject, selectedEmployee);
        }

        protected void ApproveRequisitionGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Approve" || e.CommandName == "Reject" || e.CommandName == "Pending")
            {
                int requisitionID = Convert.ToInt32(e.CommandArgument);
                string newStatus = "";

                if (e.CommandName == "Approve")
                    newStatus = "Approved";
                else if (e.CommandName == "Reject")
                    newStatus = "Rejected";
                else if (e.CommandName == "Pending")
                    newStatus = "Pending";

                UpdateRequisitionStatus(requisitionID, newStatus);

                // Reload filtered data
                string selectedStatus = ddlStatus.SelectedValue;
                string selectedProject = ddlProject.SelectedValue;
                string selectedEmployee = ddlEmployee.SelectedValue;

                LoadRequisitions(selectedStatus, selectedProject, selectedEmployee);
            }
        }

        private void UpdateRequisitionStatus(int requisitionID, string status)
        {
            int departmentHeadID = Convert.ToInt32(Session["EmployeeID"]); // Logged-in Department Head

            string query = @"
                            UPDATE Requisition 
                            SET Status = @Status, Approved_By = @DepartmentHeadID";

            if (status == "Approved")
            {
                query += ", Store_Status = 'Pending'"; // Update Store_Status to 'Pending' when approved
            }
            else if (status == "Pending" || status == "Rejected")
            {
                query += ", Store_Status = NULL"; // Set Store_Status to NULL for Pending or Rejected
            }

            query += " WHERE Requisition_ID = @RequisitionID"; // Ensure we're updating the correct requisition

            string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@DepartmentHeadID", departmentHeadID);
                    cmd.Parameters.AddWithValue("@RequisitionID", requisitionID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            LoadRequisitions("All", "All", "All"); // Refresh the list
        }
    }
}