<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddNewMaterial.aspx.cs" Inherits="STORE_FINAL.Forms.AddNewMaterial" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <h2>Add New Material</h2>
        
        <label>Material Name:</label>
        <asp:TextBox ID="txtMaterialName" runat="server"></asp:TextBox>
        <br /><br />

        <label>Description:</label>
        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
        <br /><br />

        <label>Category:</label>
        <asp:TextBox ID="txtCategory" runat="server"></asp:TextBox>
        <br /><br />

        <label>Unit Price:</label>
        <asp:TextBox ID="txtUnitPrice" runat="server"></asp:TextBox>
        <br /><br />

        <label>Stock Quantity:</label>
        <asp:TextBox ID="txtStockQuantity" runat="server"></asp:TextBox>
        <br /><br />

        <label>Rack Number:</label>
        <asp:TextBox ID="txtRackNumber" runat="server"></asp:TextBox>
        <br /><br />

        <label>Shelf Number:</label>
        <asp:TextBox ID="txtShelfNumber" runat="server"></asp:TextBox>
        <br /><br />

        <asp:Button ID="btnAddMaterial" runat="server" Text="Add Material" OnClick="btnAddMaterial_Click" />
        <br /><br />

        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
</asp:Content>
