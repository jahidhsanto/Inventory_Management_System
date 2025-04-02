using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;


namespace STORE_FINAL.Role_StoreIncharge
{
    public partial class MaterialDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string materialId = Request.QueryString["Material_ID"];
                if (!string.IsNullOrEmpty(materialId))
                {
                    LoadMaterialDetails(materialId);
                    if (gvMaterialTracking != null) // Ensure GridView exists before binding
                    {
                        LoadMaterialTracking(materialId);
                    }
                }
            }
        }

        private void LoadMaterialDetails(string materialId)
        {
            string connStr = WebConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"
                                    SELECT M.Material_ID, M.Part_Id, M.Materials_Name, M.Unit_Price, M.Stock_Quantity, M.MSQ, 
                                           UoM.UoM AS UnitOfMeasure, 
                                           C.Com_Non_Com AS Com_Non_Com, 
                                           A.Asset_Status AS Asset_Status, 
                                           ATG.Asset_Type_Grouped AS Asset_Type, 
                                           Cat.Category AS Category, 
                                           SC.Sub_Category AS Sub_Category, 
                                           Mod.Model AS Model, 
                                           Cntrl.Control AS Control, 
                                           M.Material_Image_Path
                                    FROM Material M
                                    JOIN UoM ON M.UoM_ID = UoM.UoM_ID
                                    JOIN Com_Non_Com C ON M.Com_Non_Com_ID = C.Com_Non_Com_ID
                                    JOIN Asset_Status A ON M.Asset_Status_ID = A.Asset_Status_ID
                                    JOIN Asset_Type_Grouped ATG ON M.Asset_Type_Grouped_ID = ATG.Asset_Type_Grouped_ID
                                    JOIN Category Cat ON M.Category_ID = Cat.Category_ID
                                    JOIN Sub_Category SC ON M.Sub_Category_ID = SC.Sub_Category_ID
                                    JOIN Model Mod ON M.Model_ID = Mod.Model_ID
                                    JOIN Control Cntrl ON M.Control_ID = Cntrl.Control_ID
                                    WHERE M.Material_ID = @MaterialID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaterialID", materialId);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblMaterialName.Text = reader["Materials_Name"].ToString();
                                lblPartId.Text = reader["Part_Id"].ToString();
                                lblUnitPrice.Text = Convert.ToDecimal(reader["Unit_Price"]).ToString("C");

                                lblStockQuantity.Text = reader["Stock_Quantity"].ToString();
                                lblStockQuantity.Text += " " + reader["UnitOfMeasure"].ToString();

                                lblMSQ.Text = reader["MSQ"].ToString();
                                lblMSQ.Text += " " + reader["UnitOfMeasure"].ToString();

                                lblComNonCom.Text = reader["Com_Non_Com"].ToString();
                                lblAssetStatus.Text = reader["Asset_Status"].ToString();
                                lblAssetType.Text = reader["Asset_Type"].ToString();
                                lblCategory.Text = reader["Category"].ToString();
                                lblSubCategory.Text = reader["Sub_Category"].ToString();
                                lblModel.Text = reader["Model"].ToString();
                                lblControl.Text = reader["Control"].ToString();
                                    
                                // ✅ Handling Material Image Properly
                                string imagePath = reader["Material_Image_Path"]?.ToString()?.Trim();

                                if (string.IsNullOrEmpty(imagePath))
                                {
                                    imgMaterial.ImageUrl = ResolveUrl("~/images/LoGo.png"); // Default image
                                }
                                else
                                {
                                    if (!imagePath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                                    {
                                        // Ensuring correct relative path
                                        string fullPath = Server.MapPath("~/images/Materials/") + Path.GetFileName(imagePath);
                                        if (File.Exists(fullPath))
                                        {
                                            imgMaterial.ImageUrl = ResolveUrl("~/images/Materials/" + Path.GetFileName(imagePath));
                                        }
                                        else
                                        {
                                            imgMaterial.ImageUrl = ResolveUrl("~/images/LoGo.png"); // Default image if file not found
                                        }
                                    }
                                    else
                                    {
                                        imgMaterial.ImageUrl = imagePath; // Use direct URL if stored
                                    }
                                }

                                // Stock Progress Bar
                                decimal stockQty = 0;
                                decimal msq = 1; // Default to 1 to prevent division by zero

                                // Try parsing Stock_Quantity
                                if (!decimal.TryParse(reader["Stock_Quantity"]?.ToString(), out stockQty))
                                {
                                    stockQty = 0; // Default value if parsing fails
                                }

                                // Try parsing MSQ
                                if (!decimal.TryParse(reader["MSQ"]?.ToString(), out msq) || msq <= 0)
                                {
                                    msq = 1; // Prevent division by zero, default to 1
                                }

                                // Calculate Stock Percentage Safely
                                int stockPercentage = (int)((stockQty / msq) * 100);
                                stockPercentage = Math.Min(stockPercentage, 100); // Ensure max value is 100

                                // Set Progress Bar Attributes Safely
                                progressStock.Attributes["style"] = $"width: {stockPercentage}%";
                                progressStock.Attributes["class"] = stockQty > msq ? "progress-bar bg-success" :
                                                                    stockQty > 0 && stockQty <= msq ? "progress-bar bg-warning" :
                                                                    "progress-bar bg-danger";

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMaterialName.Text = "Error loading material details.";
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void LoadMaterialTracking(string materialId)
        {
            string connStr = WebConfigurationManager.ConnectionStrings["StoreDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                                SELECT Stock_ID, Serial_Number, Rack_Number, Shelf_Number, Status, Received_Date, Quantity
                                FROM Stock 
                                WHERE Material_ID = @MaterialID";

                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    da.SelectCommand.Parameters.AddWithValue("@MaterialID", materialId);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        gvMaterialTracking.DataSource = dt;
                        gvMaterialTracking.DataBind();
                    }
                    else
                    {
                        // Show message when no records found
                        dt.Rows.Add(dt.NewRow()); // Add an empty row to avoid GridView errors
                        gvMaterialTracking.DataSource = dt;
                        gvMaterialTracking.DataBind();
                        gvMaterialTracking.Rows[0].Cells.Clear();
                        gvMaterialTracking.Rows[0].Cells.Add(new TableCell { ColumnSpan = 5, Text = "No records found", HorizontalAlign = HorizontalAlign.Center });
                    }
                }
            }
        }

        protected void gvMaterialTracking_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Example: Adding a tooltip to the Status column
                e.Row.Cells[3].ToolTip = "Current status of this item";
            }
        }

        protected void btnDownloadPDF_Click(object sender, EventArgs e)
        {
            GeneratePDF();
        }

        private void GeneratePDF()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Create Document
                Document document = new Document(PageSize.A4, 50, 50, 60, 50);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                document.Open();

                // --- ADD COMPANY LOGO ---
                string imagePath = Server.MapPath("~/images/LoGo.png"); // Adjust the path to your logo

                if (File.Exists(imagePath))
                {
                    try
                    {
                        // Use iTextSharp's Image class
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imagePath);
                        logo.ScaleAbsoluteWidth(100);
                        logo.ScaleAbsoluteHeight(100);
                        logo.Alignment = Element.ALIGN_CENTER;
                        document.Add(logo);
                    }
                    catch (Exception ex)
                    {
                        // Log the error if the image loading fails
                        document.Add(new Paragraph("Error loading logo: " + ex.Message));
                    }
                }
                else
                {
                    // Fallback message if image not found
                    document.Add(new Paragraph("Company logo not found at: " + imagePath));
                }

                // --- ADD TITLE ---
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLUE);
                Paragraph title = new Paragraph("Material Details\n\n", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                document.Add(title);

                // --- ADD TABLE WITH MATERIAL INFORMATION ---
                PdfPTable table = new PdfPTable(2) { WidthPercentage = 100, SpacingBefore = 10 };
                table.SetWidths(new float[] { 30, 70 }); // Column width

                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);
                Font textFont = FontFactory.GetFont(FontFactory.HELVETICA, 11, BaseColor.BLACK);
                BaseColor headerColor = new BaseColor(0, 102, 204);

                // Helper function to add cells
                void AddCell(string text, Font font, BaseColor bgColor, int align = Element.ALIGN_LEFT)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(text, font))
                    {
                        BackgroundColor = bgColor,
                        HorizontalAlignment = align,
                        Padding = 5
                    };
                    table.AddCell(cell);
                }

                // Add Header Cells
                AddCell("Field", headerFont, headerColor, Element.ALIGN_CENTER);
                AddCell("Details", headerFont, headerColor, Element.ALIGN_CENTER);

                // Add Material Details
                AddCell("Part ID", textFont, BaseColor.LIGHT_GRAY);
                AddCell(lblPartId.Text, textFont, BaseColor.WHITE);

                AddCell("Material Name", textFont, BaseColor.LIGHT_GRAY);
                AddCell(lblMaterialName.Text, textFont, BaseColor.WHITE);

                AddCell("Category", textFont, BaseColor.LIGHT_GRAY);
                AddCell(lblCategory.Text, textFont, BaseColor.WHITE);

                AddCell("Sub-Category", textFont, BaseColor.LIGHT_GRAY);
                AddCell(lblSubCategory.Text, textFont, BaseColor.WHITE);

                AddCell("Model", textFont, BaseColor.LIGHT_GRAY);
                AddCell(lblModel.Text, textFont, BaseColor.WHITE);

                AddCell("Control", textFont, BaseColor.LIGHT_GRAY);
                AddCell(lblControl.Text, textFont, BaseColor.WHITE);

                AddCell("Unit Price", textFont, BaseColor.LIGHT_GRAY);
                AddCell(lblUnitPrice.Text, textFont, BaseColor.WHITE);

                AddCell("Commercial", textFont, BaseColor.LIGHT_GRAY);
                AddCell(lblComNonCom.Text, textFont, BaseColor.WHITE);

                AddCell("Asset Status", textFont, BaseColor.LIGHT_GRAY);
                AddCell(lblAssetStatus.Text, textFont, BaseColor.WHITE);

                AddCell("Asset Type", textFont, BaseColor.LIGHT_GRAY);
                AddCell(lblAssetType.Text, textFont, BaseColor.WHITE);

                AddCell("Stock Quantity", textFont, BaseColor.LIGHT_GRAY);
                AddCell(lblStockQuantity.Text, textFont, BaseColor.WHITE);

                AddCell("MSQ", textFont, BaseColor.LIGHT_GRAY);
                AddCell(lblMSQ.Text, textFont, BaseColor.WHITE);

                document.Add(table); // Add Table to PDF

                document.Close();

                // Download PDF
                byte[] fileBytes = ms.ToArray();
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=MaterialDetails.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(fileBytes);
                Response.End();
            }
        }
    }
}
