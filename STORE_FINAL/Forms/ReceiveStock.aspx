<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReceiveStock.aspx.cs" Inherits="STORE_FINAL.Forms.ReceiveStock" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Receive New Stock</h2>

    <!-- Material Name Dropdown -->
    <div class="form-group">
        <label>Material Name:</label>
        <asp:DropDownList ID="ddlMaterial" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlMaterial_SelectedIndexChanged"></asp:DropDownList>
    </div>

    <!-- Part ID Dropdown -->
    <div class="form-group">
        <label>Part ID:</label>
        <asp:DropDownList ID="ddlPartID" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlPartID_SelectedIndexChanged"></asp:DropDownList>
    </div>

    <div class="form-group">
        <label>Serial Number:</label>
        <asp:TextBox ID="txtSerialNumber" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="form-group">
        <label>Rack Number:</label>
        <asp:TextBox ID="txtRackNumber" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="form-group">
        <label>Shelf Number:</label>
        <asp:TextBox ID="txtShelfNumber" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="form-group">
        <label>Status:</label>
        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
            <asp:ListItem Text="Available" Value="Available"></asp:ListItem>
            <asp:ListItem Text="Reserved" Value="Reserved"></asp:ListItem>
            <asp:ListItem Text="Delivered" Value="Delivered"></asp:ListItem>
            <asp:ListItem Text="Warranty" Value="Warranty"></asp:ListItem>
        </asp:DropDownList>
    </div>

    <asp:Button ID="btnAddStock" runat="server" Text="Add Stock" CssClass="btn btn-primary" OnClick="btnAddStock_Click" />

    <asp:Label ID="lblMessage" runat="server" CssClass="alert d-none" Visible="false"></asp:Label>

</asp:Content>
