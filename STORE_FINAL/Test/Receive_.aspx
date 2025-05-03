<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="Receive_.aspx.cs" Inherits="STORE_FINAL.Test.Receive_" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-group {
            margin-bottom: 15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h3>Receive Material</h3>

        <div class="form-group">
            <label>Material</label>
            <asp:DropDownList ID="ddlMaterial" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMaterial_SelectedIndexChanged" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label>Receive Type</label>
            <asp:DropDownList ID="ddlReceiveType" runat="server" CssClass="form-control">
                <asp:ListItem Text="New Material" Value="RECEIVE" />
                <asp:ListItem Text="Return - Active" Value="RETURN_ACTIVE" />
                <asp:ListItem Text="Return - Defective" Value="RETURN_DEFECTIVE" />
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <label>Quantity</label>
            <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" TextMode="Number" />
        </div>

        <div class="form-group">
            <label>Serial Numbers (Comma-separated if multiple)</label>
            <asp:TextBox ID="txtSerialNumbers" runat="server" CssClass="form-control" />
        </div>

        <asp:Button ID="btnSubmit" runat="server" Text="Receive Material" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Green" />
    </div>
</asp:Content>
