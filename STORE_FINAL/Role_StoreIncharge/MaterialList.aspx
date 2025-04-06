<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="MaterialList.aspx.cs" Inherits="STORE_FINAL.Role_StoreIncharge.MaterialList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Material Management</h2>

    <asp:Label ID="lblMessage" runat="server" CssClass="alert d-none"></asp:Label>

    <div class="accordion mt-4" id="requisitionManagementAccordion">
        <%--Add Material--%>
        <div class="accordion-item">
            <h2 class="accordion-header" id="headingCreate">
                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseCreate" aria-expanded="false" aria-controls="collapseCreate">
                    ➕ Add New Material
                </button>
            </h2>
            <div id="collapseCreate" class="accordion-collapse collapse" style="background-color: #e5e5e5" aria-labelledby="headingCreate" data-bs-parent="#requisitionManagementAccordion">
                <div class="accordion-body">
                    <div class="row mb-3">
                        <div class="col-md-3">
                            <label for="ddlCom_NonCom">Com/Non-Com:</label>
                            <asp:DropDownList ID="ddlCom_NonCom" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <label for="ddlAssetStatus">Asset Status:</label>
                            <asp:DropDownList ID="ddlAssetStatus" runat="server" CssClass="form-control select2"></asp:DropDownList>

                        </div>
                        <div class="col-md-3">
                            <label for="ddlAssetType">Asset Type:</label>
                            <asp:DropDownList ID="ddlAssetType" runat="server" CssClass="form-control select2"></asp:DropDownList>

                        </div>
                        <div class="col-md-3">
                            <label for="ddlCategory">Category:</label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control select2"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <label for="ddlSubCategory">Sub Category:</label>
                            <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="form-control select2"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <label for="ddlModel">Model:</label>
                            <asp:DropDownList ID="ddlModel" runat="server" CssClass="form-control select2"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <label for="ddlControl">Control:</label>
                            <asp:DropDownList ID="ddlControl" runat="server" CssClass="form-control select2"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <label for="txtMaterialName">Material Name:</label>
                            <asp:TextBox ID="txtMaterialName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="txtPart_Id">Part Id:</label>
                            <asp:TextBox ID="txtPart_Id" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="txtUnitPrice">Unit Price:</label>
                            <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="txtMSQ">MSQ:</label>
                            <asp:TextBox ID="txtMSQ" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label for="ddlUoM">UoM:</label>
                            <asp:DropDownList ID="ddlUoM" runat="server" CssClass="form-control select2"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <label for="rblRequiredSerial">Required Serial Number?:</label>
                            <asp:RadioButtonList ID="rblRequiredSerial" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>

                        <div class="col-md-6">
                            <label for="fuImage">Upload Material Image:</label>
                            <div class="card p-2 shadow-sm" style="border-radius: 10px; text-align: center; max-width: 250px;">
                                <%-- Image Preview Area --%>
                                <div id="previewArea" class="d-none">
                                    <img id="imgPreview" src="~/images/placeholder.png"
                                        class="img-thumbnail mt-2"
                                        style="max-width: 100%; height: auto; display: block; cursor: pointer;"
                                        onclick="showLargeImage(this.src);" />
                                </div>

                                <%-- Hidden File Input --%>
                                <asp:FileUpload ID="fuImage" runat="server" CssClass="form-control d-none"
                                    onchange="previewImage(this, 'imgPreview', 'previewArea');" />

                                <%-- Choose Image Button --%>
                                <button type="button" class="btn btn-primary btn-sm mt-2"
                                    onclick="document.getElementById('<%= fuImage.ClientID %>').click();">
                                    Choose Image
                                </button>
                            </div>
                        </div>

                    </div>

                    <div class="text-center mt-3">
                        <asp:Button ID="btnAddMaterial" runat="server" Text="Add Material" OnClick="btnAddMaterial_Click" CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>


        <%--Search Material--%>
        <div class="accordion-item">
            <h2 class="accordion-header" id="headingUpdate">
                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseUpdate" aria-expanded="false" aria-controls="collapseUpdate">
                    ✏️ Search Materials
                </button>
            </h2>
            <div id="collapseUpdate" class="accordion-collapse collapse" style="background-color: #e5e5e5" aria-labelledby="headingUpdate" data-bs-parent="#requisitionManagementAccordion">
                <div class="accordion-body">
                    <div class="row mb-3">
                        <div class="col-md-3">
                            <asp:TextBox ID="txtSearchPartID" runat="server" CssClass="form-control" Placeholder="Search Part ID"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtSearchMaterialName" runat="server" CssClass="form-control" Placeholder="Search Material Name"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddlCom_Non_Com" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddlAsset_Status" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddlAsset_Type_Grouped" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddl_Category" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddlSub_Category" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddl_Model" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <asp:DropDownList ID="ddl_Control" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>

                        <div class="col-md-3">
                            <asp:DropDownList ID="ddlStockFilter" runat="server" CssClass="form-control">
                                <asp:ListItem Value="">All Stock</asp:ListItem>
                                <asp:ListItem Value="Available">Available Stock</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="text-center mt-3">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                    </div>
                </div>
            </div>
        </div>

    </div>

    <!-- Material List Panel -->
    <asp:Panel ID="Panel1" runat="server" CssClass="table-responsive" Style="overflow-x: auto;">
        <asp:GridView ID="gvMaterials" runat="server" AutoGenerateColumns="False"
            CssClass="table table-bordered"
            HorizontalAlign="Left"
            DataKeyNames="Material_ID"
            OnRowCommand="gvMaterials_RowCommand"
            GridLines="None"
            CellPadding="8"
            Style="overflow-x: auto;" 
            AllowPaging="True" OnPageIndexChanging="gvMaterials_PageIndexChanging">

            <%--Table Header Styling--%>
            <HeaderStyle CssClass="table-dark text-white" />
            <RowStyle CssClass="table-light" />
            <AlternatingRowStyle CssClass="table-secondary" />

            <Columns>
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:Button ID="btnView" runat="server" CommandName="ViewDetails"
                            CommandArgument='<%# Eval("Material_ID") %>' Text="🔍 View"
                            CssClass="btn btn-info btn-sm" />
                    </ItemTemplate>
                    <ItemStyle Width="90px" HorizontalAlign="Center" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="Material_ID" HeaderText="ID">
                    <ItemStyle Width="50px" HorizontalAlign="Center" />
                </asp:BoundField>

                <asp:BoundField DataField="Part_Id" HeaderText="Part ID">
                    <ItemStyle Width="80px" />
                </asp:BoundField>

                <asp:BoundField DataField="Materials_Name" HeaderText="Material Name">
                    <ItemStyle Width="200px" />
                </asp:BoundField>

                <asp:TemplateField HeaderText="Stock QTY">
                    <ItemTemplate>
                        <span class='<%# 
                            (Eval("Stock_Quantity") == DBNull.Value || Eval("Stock_Quantity") == null) ? 
                                "badge rounded-pill bg-danger text-center" : 
                            (Convert.ToInt32(Eval("Stock_Quantity")) > Convert.ToInt32(Eval("MSQ"))) ? 
                                "badge rounded-pill bg-success text-center" : 
                            (Convert.ToInt32(Eval("Stock_Quantity")) > 0 && Convert.ToInt32(Eval("Stock_Quantity")) <= Convert.ToInt32(Eval("MSQ"))) ? 
                                "badge rounded-pill bg-warning text-dark text-center" : 
                                "badge rounded-pill bg-danger text-center"
                        %>'
                            style="display: inline-block; width: 100%; text-align: center;">
                            <%# Eval("Stock_Quantity") == DBNull.Value || Eval("Stock_Quantity") == null ? "0" : Eval("Stock_Quantity") %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:BoundField DataField="UoM" HeaderText="Unit of Measure">
                    <ItemStyle Width="80px" />
                </asp:BoundField>

                <asp:BoundField DataField="Com_Non_Com" HeaderText="Com / Non-Com">
                    <ItemStyle Width="100px" HorizontalAlign="Center" />
                </asp:BoundField>

                <asp:BoundField DataField="Asset_Status" HeaderText="Asset Status">
                    <ItemStyle Width="120px" />
                </asp:BoundField>

                <asp:BoundField DataField="Asset_Type_Grouped" HeaderText="Asset Type">
                    <ItemStyle Width="120px" />
                </asp:BoundField>

                <asp:BoundField DataField="Category" HeaderText="Category">
                    <ItemStyle Width="120px" />
                </asp:BoundField>

                <asp:BoundField DataField="Sub_Category" HeaderText="Sub Category">
                    <ItemStyle Width="150px" />
                </asp:BoundField>

                <asp:BoundField DataField="Model" HeaderText="Model">
                    <ItemStyle Width="120px" />
                </asp:BoundField>

                <asp:BoundField DataField="Control" HeaderText="Control">
                    <ItemStyle Width="100px" />
                </asp:BoundField>

                <asp:TemplateField HeaderText="Unit Price">
                    <ItemTemplate>
                        <span style="text-align: right; display: block;">
                            <%# Eval("Unit_Price") == DBNull.Value || Eval("Unit_Price") == null ? "৳0.00" : "৳" + Convert.ToDecimal(Eval("Unit_Price")).ToString("0.00") %>  
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>


            </Columns>
        </asp:GridView>
    </asp:Panel>

</asp:Content>
