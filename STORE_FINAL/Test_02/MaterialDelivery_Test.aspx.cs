using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Org.BouncyCastle.Asn1.Cmp;
using System.Configuration;

namespace STORE_FINAL.Test_02
{
    public partial class MaterialDelivery_Test : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        private DataTable CreateItemTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Requisition_ID");
            dt.Columns.Add("Stock_ID");
            dt.Columns.Add("Material_ID");
            dt.Columns.Add("Material_Name");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Requires_Serial_Number");

            return dt;
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            string requisitionID = txtRequisitionID.Text.Trim();
            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string query = @"
								SELECT 
									rp.Requisition_ID,
									rp.Created_Date,
									rp.Requisition_Purpose,

									ric.Material_ID,
									m.Part_Id,
									m.Materials_Name,
									m.Requires_Serial_Number,
									ric.Quantity
								FROM Requisition_Parent rp
								JOIN Requisition_Item_Child ric ON rp.Requisition_ID = ric.Requisition_ID
								JOIN Material m ON ric.Material_ID = m.Material_ID
								WHERE rp.Requisition_ID = 1";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@RequisitionID", requisitionID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // ✅ Store in ViewState
                    ViewState["RequisitionItems"] = dt;

                    // ✅ Bind to Repeater
                    rptMaterials.DataSource = dt;
                    rptMaterials.DataBind();

                    lblStatus.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error loading data: " + ex.Message;
            }

        }

        protected void btnLoad_Click00(object sender, EventArgs e)
        {
            string requisitionID = txtRequisitionID.Text.Trim();
            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string query = @"
								WITH Required_Items AS (
									SELECT 
										rp.Requisition_ID,
										rp.Created_Date,
										rp.Requisition_For,
										rp.Employee_ID_For,
										rp.Department_ID_For,
										rp.Zone_ID_For,
										rp.Project_Code_For,
										rp.Requisition_Purpose,

										ric.Material_ID,
										m.Part_Id,
										m.Materials_Name,
										m.Requires_Serial_Number,
										ric.Quantity,
										m.Unit_Price
									FROM Requisition_Parent rp
									JOIN Requisition_Item_Child ric ON rp.Requisition_ID = ric.Requisition_ID
									JOIN Material m ON ric.Material_ID = m.Material_ID
								),

								Available_Serials AS (
									SELECT 
										s.Material_ID,
										s.Serial_Number,
										s.Rack_Number,
										s.Shelf_Number,
										s.Status,
										s.Availability,
										ROW_NUMBER() OVER (PARTITION BY s.Material_ID ORDER BY s.Received_Date) AS rn
									FROM Stock s
									JOIN Material m ON s.Material_ID = m.Material_ID
									WHERE m.Requires_Serial_Number = 'Yes'
									  AND s.Availability = 'AVAILABLE'
									  AND s.Status = 'ACTIVE'
								)

								-- Final output: Expand by serials or just show one row per item
								SELECT 
									ri.Material_ID,
									ri.Materials_Name,
									ri.Requires_Serial_Number,
									ri.Quantity,

									ri.Requisition_ID,
									ri.Created_Date,
									ri.Requisition_For,
									ri.Employee_ID_For,
									ri.Department_ID_For,
									ri.Zone_ID_For,
									ri.Project_Code_For,
									ri.Requisition_Purpose,

									ri.Material_ID,
									ri.Part_Id,
									ri.Materials_Name,
									ri.Requires_Serial_Number,
									ri.Quantity,
									ri.Unit_Price,

									-- Serial info (only if applicable)
									asn.Serial_Number,
									asn.Rack_Number,
									asn.Shelf_Number

								FROM Required_Items ri
								LEFT JOIN Available_Serials asn
									ON ri.Material_ID = asn.Material_ID
									AND asn.rn <= ri.Quantity
								WHERE ri.Requisition_ID = @RequisitionID
								ORDER BY ri.Requisition_ID, ri.Material_ID, asn.rn;";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@RequisitionID", requisitionID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptMaterials.DataSource = dt;
                    rptMaterials.DataBind();

                    lblStatus.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error loading data: " + ex.Message;
            }
        }

        protected void rptMaterials_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var hfMaterialID = (HiddenField)e.Item.FindControl("hfMaterialID");
                var ddlSerialOptions = (DropDownList)e.Item.FindControl("ddlSerialOptions");
                var lblRequiresSerial = (Label)e.Item.FindControl("lblRequiresSerial");

                if (lblRequiresSerial.Text.Trim() == "Yes")
                {
                    string materialId = hfMaterialID.Value;

                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        string query = @"SELECT Serial_Number FROM Stock WHERE Material_ID = @MaterialID AND Availability = 'AVAILABLE' AND Status = 'ACTIVE'";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@MaterialID", materialId);

                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        ddlSerialOptions.Items.Insert(0, new ListItem("-- Select Serial --", ""));

                        while (reader.Read())
                        {
                            ddlSerialOptions.Items.Add(new ListItem(reader["Serial_Number"].ToString()));
                        }

                        con.Close();
                    }

                    // Add onchange handler
                    ddlSerialOptions.Attributes["onchange"] = $"addSerial('{materialId}', this);";
                }
                else
                {
                    ddlSerialOptions.Visible = false;
                }
            }
        }


        protected void rptMaterials_ItemDataBound00(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var txtQuantity = (TextBox)e.Item.FindControl("txtQuantity");
                var txtSerialNumber = (TextBox)e.Item.FindControl("txtSerialNumber");
                var lblRequiresSerial = (Label)e.Item.FindControl("lblRequiresSerial");

                if (lblRequiresSerial.Text.Trim() == "Yes")
                {
                    txtSerialNumber.Enabled = true;
                    txtQuantity.Enabled = false;
                }
                else
                {
                    txtSerialNumber.Enabled = false;
                    txtQuantity.Enabled = true;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string requisitionID = txtRequisitionID.Text.Trim();

            if (string.IsNullOrEmpty(requisitionID))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMissingID", "Swal.fire('Error', 'Please enter a Requisition ID', 'error');", true);
                return;
            }

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

foreach (RepeaterItem item in rptMaterials.Items)
{
    string materialID = ((HiddenField)item.FindControl("hfMaterialID")).Value;
    string requiresSerial = ((Label)item.FindControl("lblRequiresSerial")).Text;
    string selectedSerials = ((HiddenField)item.FindControl("hfSelectedSerials")).Value;
    string quantity = ((TextBox)item.FindControl("txtQuantity")).Text.Trim();

    if (requiresSerial == "Yes" && !string.IsNullOrEmpty(selectedSerials))
    {
        foreach (string serial in selectedSerials.Split(','))
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO Requisition_Item_Child (Requisition_ID, Material_ID, Quantity, Serial_Number) VALUES (@RID, @MID, 1, @Serial)", con);
            cmd.Parameters.AddWithValue("@RID", requisitionID);
            cmd.Parameters.AddWithValue("@MID", materialID);
            cmd.Parameters.AddWithValue("@Serial", serial);
            cmd.ExecuteNonQuery();
        }
    }
    else if (requiresSerial != "Yes" && !string.IsNullOrEmpty(quantity))
    {
        SqlCommand cmd = new SqlCommand("INSERT INTO Requisition_Item_Child (Requisition_ID, Material_ID, Quantity, Serial_Number) VALUES (@RID, @MID, @Qty, NULL)", con);
        cmd.Parameters.AddWithValue("@RID", requisitionID);
        cmd.Parameters.AddWithValue("@MID", materialID);
        cmd.Parameters.AddWithValue("@Qty", Convert.ToInt32(quantity));
        cmd.ExecuteNonQuery();
    }
}

                con.Close();
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "alertSuccess", "Swal.fire('Saved!', 'Requisition items saved successfully.', 'success');", true);
        }
        protected void btnSave_Click00(object sender, EventArgs e)
        {
            string requisitionID = txtRequisitionID.Text.Trim();

            if (string.IsNullOrEmpty(requisitionID))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alertMissingID", "Swal.fire('Error', 'Please enter a Requisition ID', 'error');", true);
                return;
            }

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                foreach (RepeaterItem item in rptMaterials.Items)
                {
                    string materialID = ((HiddenField)item.FindControl("hfMaterialID")).Value;
                    string requiresSerial = ((Label)item.FindControl("lblRequiresSerial")).Text;
                    var chkSerials = (CheckBoxList)item.FindControl("chkSerialNumbers");
                    var txtQuantity = (TextBox)item.FindControl("txtQuantity");

                    if (requiresSerial == "Yes")
                    {
                        foreach (ListItem serial in chkSerials.Items)
                        {
                            if (serial.Selected)
                            {
                                SqlCommand cmd = new SqlCommand("INSERT INTO Requisition_Item_Child (Requisition_ID, Material_ID, Quantity, Serial_Number) VALUES (@RID, @MID, 1, @Serial)", con);
                                cmd.Parameters.AddWithValue("@RID", requisitionID);
                                cmd.Parameters.AddWithValue("@MID", materialID);
                                cmd.Parameters.AddWithValue("@Serial", serial.Value);
                                cmd.Parameters.AddWithValue("@Qty", 1);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        string qty = txtQuantity.Text.Trim();
                        if (!string.IsNullOrEmpty(qty))
                        {
                            SqlCommand cmd = new SqlCommand("INSERT INTO Requisition_Item_Child (Requisition_ID, Material_ID, Quantity, Serial_Number) VALUES (@RID, @MID, @Qty, NULL)", con);
                            cmd.Parameters.AddWithValue("@RID", requisitionID);
                            cmd.Parameters.AddWithValue("@MID", materialID);
                            cmd.Parameters.AddWithValue("@Qty", Convert.ToInt32(qty));
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                con.Close();
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "alertSuccess", "Swal.fire('Saved!', 'Requisition items saved successfully.', 'success');", true);
        }

    }
}