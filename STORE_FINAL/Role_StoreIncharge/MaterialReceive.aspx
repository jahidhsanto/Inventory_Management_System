<%@ Page Title="Receive New Stock" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="MaterialReceive.aspx.cs" Inherits="STORE_FINAL.Role_StoreIncharge.MaterialReceive" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="mb-4"><i class="fas fa-box"></i>Receive New Stock</h2>

    <div class="card shadow p-4">
        <div class="row">
            <div class="col-md-6">

                <div class="form-group">
                    <label><i class="fas fa-cubes"></i>Material Name:</label>
                    <asp:DropDownList ID="ddlMaterial" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlMaterial_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <div class="form-group">
                    <label><i class="fas fa-barcode"></i>Part ID:</label>
                    <asp:DropDownList ID="ddlPartID" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlPartID_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <div class="form-group">
                    <label><i class="fas fa-hashtag"></i>Serial Number:</label>
                    <asp:TextBox ID="txtSerialNumber" runat="server" CssClass="form-control" placeholder="Enter Serial Number"></asp:TextBox>
                    <small class="text-danger d-none" id="serialError">Serial Number is required!</small>
                </div>

            </div>
            <div class="col-md-6">

                <div class="form-group">
                    <label><i class="fas fa-th-large"></i>Rack Number:</label>
                    <asp:TextBox ID="txtRackNumber" runat="server" CssClass="form-control" placeholder="Enter Rack Number"></asp:TextBox>
                    <small class="text-danger d-none" id="rackError">Rack Number is required!</small>
                </div>

                <div class="form-group">
                    <label><i class="fas fa-align-left"></i>Shelf Number:</label>
                    <asp:TextBox ID="txtShelfNumber" runat="server" CssClass="form-control" placeholder="Enter Shelf Number"></asp:TextBox>
                    <small class="text-danger d-none" id="shelfError">Shelf Number is required!</small>
                </div>

                <div class="form-group">
                    <label><i class="fas fa-info-circle"></i>Status:</label>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control select2">
                        <asp:ListItem Text="Available" Value="Available"></asp:ListItem>
                        <asp:ListItem Text="Reserved" Value="Reserved"></asp:ListItem>
                        <asp:ListItem Text="Delivered" Value="Delivered"></asp:ListItem>
                        <asp:ListItem Text="Warranty" Value="Warranty"></asp:ListItem>
                    </asp:DropDownList>
                </div>

            </div>
        </div>

        <!-- Submit Button -->
        <div class="mt-4">
            <asp:Button ID="btnAddStock" runat="server" Text="Add Stock" CssClass="btn btn-success btn-lg" OnClick="btnAddStock_Click" />
            <span id="loading" class="spinner-border spinner-border-sm text-success d-none"></span>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="alert mt-3 d-none"></asp:Label>
        <%--<asp:Label ID="lblMessage" runat="server" CssClass="alert d-none" Visible="false"></asp:Label>--%>

    </div>
</asp:Content>
