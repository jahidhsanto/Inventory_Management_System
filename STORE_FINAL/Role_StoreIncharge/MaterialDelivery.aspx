<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="MaterialDelivery.aspx.cs" Inherits="STORE_FINAL.Role_StoreIncharge.MaterialDelivery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-4">
        <h2 class="text-center mb-4">📦 Material Delivery</h2>
        <asp:Label ID="lblMessage" runat="server" CssClass="alert mt-3"></asp:Label>
        <!-- Requisition Selection & Details -->
        <div class="row">
            <!-- Left Side: Requisition Details -->
            <div class="col-md-8">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row mt-4">
                            <div class="col-md-6">
                                <label>Requisition By:</label>
                                <asp:DropDownList ID="ddlRequisitionBy" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlRequisitionBy_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label>Select Requisition:</label>
                                <asp:DropDownList ID="ddlRequisition" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlRequisition_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="card mt-3 p-3 bg-light">
                            <h5>📋 Requisition Details</h5>
                            <p><strong>Material Name:</strong>
                                <asp:Label ID="lblMaterialName" runat="server" Text="-"></asp:Label></p>
                            <p><strong>Requested Quantity:</strong>
                                <asp:Label ID="lblRequestedQuantity" runat="server" Text="-"></asp:Label></p>
                            <p><strong>Requested By:</strong>
                                <asp:Label ID="lblRequestedBy" runat="server" Text="-"></asp:Label></p>
                            <p><strong>Requested Date:</strong>
                                <asp:Label ID="lblRequestedDate" runat="server" Text="-"></asp:Label></p>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlRequisition" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

            <!-- Right Side: Requisition Image -->
            <div class="col-md-4 d-flex align-items-center justify-content-center">
                <div class="card text-center p-3">
                    <!-- 🔄 UpdatePanel for Requisition Image -->
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <h5>📷 Requisition Image</h5>
                            <asp:Image ID="imgRequisition" runat="server" CssClass="img-fluid rounded" Height="200px" Width="100%" ImageUrl="~/images/LoGo.jpg" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlRequisition" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <!-- Select Stock Item -->
        <div class="row mt-4">
            <div class="col-md-3">
                <!-- 🔄 UpdatePanel for Requisition Image -->
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <label ID="lblSerialNumber" runat="server"><strong>Serial Number:</strong></label>
                        <asp:DropDownList ID="ddlSerialNumber" runat="server" CssClass="form-control select2" Enabled="false">
                            <asp:ListItem Text="Select a Requisition" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlRequisition" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="col-md-3">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <label ID="lblLocation" runat="server"><strong>Stock Location:</strong></label>
                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control select2" Enabled="false">
                            <asp:ListItem Text="Select Requisition" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlRequisition" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="col-md-3">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <label><strong>Quantity:</strong></label>
                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlRequisition" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

        <!-- Add to Delivery Button -->
        <div class="text-center mt-4">
            <asp:Button ID="btnAddToDelivery" runat="server" Text="➕ Add to Delivery" CssClass="btn btn-primary btn-lg" OnClick="btnAddToDelivery_Click" />
        </div>
        <!-- Selected Items for Delivery -->
        <h3 class="mt-4 text-center">📦 Selected Items for Delivery</h3>
        <div class="table-responsive">
            <asp:GridView ID="gvDeliveryItems" runat="server" CssClass="table table-bordered table-striped text-center" AutoGenerateColumns="False" DataKeyNames="Temp_ID" OnRowCommand="gvDeliveryItems_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="S.No">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Material_ID" HeaderText="Material ID" />
                    <asp:BoundField DataField="Part_Id" HeaderText="Part ID" />
                    <asp:BoundField DataField="Materials_Name" HeaderText="Material Name" />
                    <asp:BoundField DataField="Serial_Number" HeaderText="Serial Number" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="Location" HeaderText="Location" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnRemove" runat="server" CommandName="Remove" 
                                CommandArgument='<%# Eval("Temp_ID") %>' CssClass="btn delete-icon">
                                <i class="fa fa-remove" style="color:red"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>

        <div class="text-center mt-3">
            <label><strong>Received Employee:</strong></label>
            <asp:DropDownList ID="ddlReceivedByEmployee" runat="server" CssClass="form-control "></asp:DropDownList>
            
            <!-- Deliver Button -->
            <asp:Button ID="btnDeliver" runat="server" Text="🚚 Deliver Items" CssClass="btn btn-success btn-lg" OnClick="btnDeliver_Click" />
        </div>
    </div>

    <asp:HiddenField ID="hfDeliverySessionID" runat="server" />

</asp:Content>
