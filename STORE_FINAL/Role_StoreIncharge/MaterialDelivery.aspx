<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="MaterialDelivery.aspx.cs" Inherits="STORE_FINAL.Role_StoreIncharge.MaterialDelivery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <div class="container mt-4">
            <h2>Material Delivery</h2>

            <!-- Select Requisition & Employee -->
            <div class="row">
                <div class="col-md-4">
                    <label>Requisition:</label>
                    <asp:DropDownList ID="ddlRequisition" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="col-md-4">
                    <label>Received Employee:</label>
                    <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control select2"></asp:DropDownList>
                </div>
            </div>

            <!-- Select Stock Item -->
            <div class="row mt-3">
                <div class="col-md-3">
                    <label>Stock Item:</label>
                    <asp:DropDownList ID="ddlStock" runat="server" CssClass="form-control select2"></asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label>Material:</label>
                    <asp:DropDownList ID="ddlMaterial" runat="server" CssClass="form-control select2"></asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label>Serial Number (Optional):</label>
                    <asp:TextBox ID="txtSerialNumber" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <label>Quantity:</label>
                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>

            <!-- Add to Delivery Button -->
            <div class="mt-3">
                <asp:Button ID="btnAddToDelivery" runat="server" Text="Add to Delivery" CssClass="btn btn-primary" OnClick="btnAddToDelivery_Click" />
            </div>

            <!-- Display Selected Items -->
            <h3 class="mt-4">Selected Items for Delivery</h3>
            <asp:GridView ID="gvDeliveryItems" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="Temp_ID" HeaderText="Temp ID" />
                    <asp:BoundField DataField="Material_ID" HeaderText="Material ID" />
                    <asp:BoundField DataField="Stock_ID" HeaderText="Stock ID" />
                    <asp:BoundField DataField="Serial_Number" HeaderText="Serial Number" />
                    <asp:BoundField DataField="Delivered_Quantity" HeaderText="Quantity" />
                    <asp:CommandField ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>

            <!-- Deliver Button -->
            <div class="mt-3">
                <asp:Button ID="btnDeliver" runat="server" Text="Deliver Items" CssClass="btn btn-success" OnClick="btnDeliver_Click" />
            </div>
        </div>

</asp:Content>
