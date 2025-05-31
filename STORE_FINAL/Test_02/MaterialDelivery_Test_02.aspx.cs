using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace STORE_FINAL.Test_02
{
    public partial class MaterialDelivery_Test_02 : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRequisitionDropdown();
            }
        }

        //Load Requisition DropDown
        private void LoadRequisitionDropdown()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    select Requisition_ID 
                    from Requisition_Parent
                    where Dept_Status = 'Approved' 
                        AND Store_Status = 'Pending';";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                ddlRequisition.DataSource = dr;
                ddlRequisition.DataTextField = "Requisition_ID";
                ddlRequisition.DataValueField = "Requisition_ID";
                ddlRequisition.DataBind();
            }

            ddlRequisition.Items.Insert(0, new ListItem("-- Select Requisition --", ""));
        }

        // On Selected Index Changed
        protected void ddlRequisition_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear_ViewStateAndGrid();

            int requisitionId = Convert.ToInt32(ddlRequisition.SelectedValue);

            LoadRequisitionDetails(requisitionId);
            LoadRequisitionMaterials(requisitionId);
        }

        private void LoadRequisitionDetails(int requisitionId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
        SELECT 
            RP.Requisition_ID,
            RP.Created_Date,
            RP.Requisition_For,
            RP.Requisition_Purpose,
            RP.Dept_Status,
            RP.Dept_Approval_Date,
            RP.Dept_Approval_Remarks,
            DeptApprover.Name AS Dept_Approved_By,
            RP.Store_Status,
            RP.Store_Approval_Date,
            RP.Store_Approval_Remarks,
            StoreApprover.Name AS Store_Approved_By,
            E.Name AS Requested_By,
            D.DepartmentName AS Requester_Department,

            -- Recipient names
            EmpFor.Name AS Employee_For_Name,
            DeptFor.DepartmentName AS Department_For_Name,
            Z.Zone_Name AS Zone_For_Name,
            P.Project_Name AS Project_For_Name

        FROM Requisition_Parent RP
        INNER JOIN Employee E ON RP.CreatedByEmployee_ID = E.Employee_ID
        LEFT JOIN Department D ON E.Department_ID = D.Department_ID

        LEFT JOIN Employee DeptApprover ON RP.Dept_Approved_By = DeptApprover.Employee_ID
        LEFT JOIN Employee StoreApprover ON RP.Store_Approved_By = StoreApprover.Employee_ID

        LEFT JOIN Employee EmpFor ON RP.Employee_ID_For = EmpFor.Employee_ID
        LEFT JOIN Department DeptFor ON RP.Department_ID_For = DeptFor.Department_ID
        LEFT JOIN Zone Z ON RP.Zone_ID_For = Z.Zone_ID
        LEFT JOIN Project P ON RP.Project_Code_For = P.Project_Code

        WHERE RP.Requisition_ID = @ReqID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ReqID", requisitionId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Basic Info
                            lblReqNo.Text = requisitionId.ToString();
                            lblCreatedDate.Text = Convert.ToDateTime(reader["Created_Date"]).ToString("dd MMM yyyy");
                            lblReqType.Text = reader["Requisition_For"].ToString();
                            lblRequestedBy.Text = reader["Requested_By"].ToString();

                            // Purpose (if applicable)
                            string reqFor = reader["Requisition_For"].ToString();
                            lblPurpose.Text = reqFor == "Project" ? reader["Requisition_Purpose"]?.ToString() : "-";

                            // Recipient Info
                            switch (reqFor)
                            {
                                case "Employee":
                                    lblRecipientInfo.Text = reader["Employee_For_Name"]?.ToString();
                                    break;
                                case "Department":
                                    lblRecipientInfo.Text = reader["Department_For_Name"]?.ToString();
                                    break;
                                case "Zone":
                                    lblRecipientInfo.Text = reader["Zone_For_Name"]?.ToString();
                                    break;
                                case "Project":
                                    lblRecipientInfo.Text = reader["Project_For_Name"]?.ToString();
                                    break;
                                default:
                                    lblRecipientInfo.Text = "-";
                                    break;
                            }

                            // Department Approval
                            lblDeptStatus.Text = reader["Dept_Status"]?.ToString();
                            lblDeptApprovedBy.Text = reader["Dept_Approved_By"]?.ToString();
                            lblDeptApprovalDate.Text = reader["Dept_Approval_Date"] == DBNull.Value ? "-" :
                                Convert.ToDateTime(reader["Dept_Approval_Date"]).ToString("dd MMM yyyy");
                            lblDeptRemarks.Text = reader["Dept_Approval_Remarks"]?.ToString();

                            // Store Approval
                            lblStoreStatus.Text = reader["Store_Status"]?.ToString();
                            lblStoreApprovedBy.Text = reader["Store_Approved_By"]?.ToString();
                            lblStoreApprovalDate.Text = reader["Store_Approval_Date"] == DBNull.Value ? "-" :
                                Convert.ToDateTime(reader["Store_Approval_Date"]).ToString("dd MMM yyyy");
                            lblStoreRemarks.Text = reader["Store_Approval_Remarks"]?.ToString();

                            pnlRequisitionDeliveryInfo.Visible = true;
                        }
                        else
                        {
                            pnlRequisitionDeliveryInfo.Visible = false;
                        }
                    }
                }
            }
        }
        private void LoadRequisitionMaterials(int requisitionId)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT 
                        ric.Material_ID,
                        m.Materials_Name,
                        ric.Quantity,
                        m.Requires_Serial_Number 
                    FROM Requisition_Item_Child ric
                    JOIN Material m ON m.Material_ID = ric.Material_ID
                    WHERE ric.Requisition_ID = @Requisition_ID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Requisition_ID", requisitionId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            DeliveryMaterials = dt;

            // Rebind
            gvMaterials.DataSource = dt;
            gvMaterials.DataBind();
        }

        // Create DataTable for Items List
        private DataTable DeliveryMaterials
        {
            get
            {
                if (ViewState["DeliveryMaterials"] == null)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Material_ID");
                    dt.Columns.Add("Materials_Name");
                    dt.Columns.Add("Quantity");
                    dt.Columns.Add("Requires_Serial_Number", typeof(bool));
                    ViewState["DeliveryMaterials"] = dt;
                }
                return (DataTable)ViewState["DeliveryMaterials"];
            }
            set
            {
                ViewState["DeliveryMaterials"] = value;
            }
        }

        // Add row on '+' click
        protected void gvMaterials_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddRow")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                DataTable dt = DeliveryMaterials;

                if (dt != null && rowIndex >= 0 && rowIndex < dt.Rows.Count)
                {
                    DataRow sourceRow = dt.Rows[rowIndex];

                    // Create new empty row
                    DataRow newRow = dt.NewRow();
                    newRow["Material_ID"] = sourceRow["Material_ID"];
                    newRow["Materials_Name"] = sourceRow["Materials_Name"];
                    newRow["Quantity"] = 0;
                    newRow["Requires_Serial_Number"] = sourceRow["Requires_Serial_Number"];

                    // Insert new row after the current row
                    dt.Rows.InsertAt(newRow, rowIndex + 1);

                    // update ViewState
                    DeliveryMaterials = dt;

                    // Rebind
                    gvMaterials.DataSource = dt;
                    gvMaterials.DataBind();
                }
            }
        }

        protected void gvMaterials_RowCommand_00(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddRow")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                string materialId = e.CommandArgument.ToString();

                DataTable dt = DeliveryMaterials;

                // Find original row
                DataRow[] originalRows = dt.Select($"Material_ID = '{materialId}'");
                if (originalRows.Length > 0)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["Material_ID"] = originalRows[0]["Material_ID"];
                    newRow["Materials_Name"] = originalRows[0]["Materials_Name"];
                    newRow["Quantity"] = 0;
                    newRow["Requires_Serial_Number"] = originalRows[0]["Requires_Serial_Number"];
                    //dt.Rows.Add(newRow);
                    dt.Rows.InsertAt(newRow, rowIndex + 1);
                }

                DeliveryMaterials = dt;
                gvMaterials.DataSource = dt;
                gvMaterials.DataBind();
            }
        }

        protected void gvMaterials_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = (DataRowView)e.Row.DataItem;
                bool requiresSerial = dataItem["Requires_Serial_Number"].ToString() == "Yes";
                //bool requiresSerial = dataItem["Requires_Serial_Number"] != DBNull.Value && dataItem["Requires_Serial_Number"].ToString() == "Yes";
                int materialId = dataItem["Material_ID"] != DBNull.Value ? Convert.ToInt32(dataItem["Material_ID"]) : 0;

                // Get controls from the GridView row
                Panel pnlSerialInput = (Panel)e.Row.FindControl("pnlSerialInput");
                Panel pnlQtyInput = (Panel)e.Row.FindControl("pnlQtyInput");
                ListBox txtSerialNumbers = (ListBox)e.Row.FindControl("txtSerialNumbers");
                DropDownList ddlLocation = (DropDownList)e.Row.FindControl("ddlLocation");

                if (requiresSerial)
                {
                    pnlSerialInput.Visible = true;

                    // Fetch from DB or ViewState
                    txtSerialNumbers.DataSource = GetAvailableSerials(materialId);
                    txtSerialNumbers.DataTextField = "Serial_Number";
                    txtSerialNumbers.DataValueField = "Serial_Number";
                    txtSerialNumbers.DataBind();
                }
                else
                {
                    pnlQtyInput.Visible = true;

                    // Bind locations
                    ddlLocation.DataSource = GetAvailableLocations(materialId);
                    ddlLocation.DataTextField = "Location_Name";
                    ddlLocation.DataValueField = "Location_ID";
                    ddlLocation.DataBind();
                    ddlLocation.Items.Insert(0, new ListItem("-- Select Location --", ""));
                }
            }
        }

        private DataTable GetAvailableLocations(int materialId)
        {
            DataTable dtLocations = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT DISTINCT Stock_ID AS Location_ID, CONCAT(Rack_Number, ' ', Shelf_Number) AS Location_Name 
                    FROM Stock 
                    WHERE Material_ID = @MaterialID 
                        AND Availability = 'AVAILABLE'
                        AND Status = 'ACTIVE'
                        AND Stock_ID IS NOT NULL";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaterialID", materialId);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtLocations);
                    }
                }
            }

            return dtLocations;
        }
        private DataTable GetAvailableSerials(int materialId)
        {
            DataTable dtSerials = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT Serial_Number 
                    FROM Stock 
                    WHERE Material_ID = @MaterialID 
                        AND Availability = 'AVAILABLE'
                        AND Status = 'ACTIVE'
                        AND Serial_Number IS NOT NULL";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaterialID", materialId);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtSerials);
                    }
                }
            }

            return dtSerials;
        }

        protected void btnDeliver_Click(object sender, EventArgs e)
        {

        }

        // Clear ViewState and Grid
        private void Clear_ViewStateAndGrid()
        {
            ViewState["MaterialsTable"] = null;
            gvMaterials.DataSource = null;
            gvMaterials.DataBind();

        }

        private void ShowMessage(string message, bool isSuccess)
        {
            string messageType = isSuccess ? "success" : "error";
            string escapedMessage = message.Replace("'", "\\'"); // Escape single quotes

            string js = $@"
                            setTimeout(function() {{
                                if (typeof showToast === 'function') {{
                                    showToast('{escapedMessage}', '{messageType}');
                                }}
                            }}, 100);
                        ";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowToastMessage", js, true);
        }
    }
}