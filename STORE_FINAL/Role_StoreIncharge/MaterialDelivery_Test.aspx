<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="MaterialDelivery_Test.aspx.cs" Inherits="STORE_FINAL.Role_StoreIncharge.MaterialDelivery_Test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Required Script Manager for AJAX -->
    <asp:ScriptManager runat="server" />

    <div class="container mt-4">
        <h2 class="text-center mb-4">📦 Material Delivery</h2>
        <asp:Label ID="lblMessage" runat="server" CssClass="alert mt-3"></asp:Label>
        <!-- Requisition Selection & Details -->
        <div class="row">
            <!-- Left Side: Requisition Details -->
            <div class="col-md-6">
                <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <label>Requisition:</label>
                        <asp:DropDownList ID="ddlRequisition" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlRequisition_SelectedIndexChanged"></asp:DropDownList>

                        <label><strong>Received Employee:</strong></label>
                        <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control "></asp:DropDownList>

                        <div class="card mt-3 p-3 bg-light">
                            <h5>📋 Requisition Details</h5>
                            <p><strong>Material Name:</strong>
                                <asp:Label ID="lblMaterialName" runat="server" Text="-"></asp:Label></p>
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
            <div class="col-md-6 d-flex align-items-center justify-content-center">
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
                        <asp:DropDownList ID="ddlSerialNumber" runat="server" CssClass="form-control select2"></asp:DropDownList>
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
                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control select2"></asp:DropDownList>
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
            <asp:GridView ID="gvDeliveryItems" runat="server" CssClass="table table-bordered text-center" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="Material_ID" HeaderText="Material ID" />
                    <asp:BoundField DataField="Serial_Number" HeaderText="Serial Number" />
                    <asp:BoundField DataField="Delivered_Quantity" HeaderText="Quantity" />
                    <asp:CommandField ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>
        </div>

        <!-- Deliver Button -->
        <div class="text-center mt-3">
            <asp:Button ID="btnDeliver" runat="server" Text="🚚 Deliver Items" CssClass="btn btn-success btn-lg" OnClick="btnDeliver_Click" />
        </div>
    </div>

    <asp:HiddenField ID="hfDeliverySessionID" runat="server" />

</asp:Content>
