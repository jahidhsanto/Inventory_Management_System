<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddNewMaterial.aspx.cs" Inherits="STORE_FINAL.Forms.AddNewMaterial" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <h2>Add New Material</h2>
    
        <label>Com/Non-Com:</label>
        <asp:DropDownList ID="ddlCom_NonCom" runat="server"></asp:DropDownList>
        <br /><br />

        <label>Asset Status:</label>
        <asp:DropDownList ID="ddlAssetStatus" runat="server"></asp:DropDownList>
        <br /><br />

        <label>Asset Type:</label>
        <asp:DropDownList ID="ddlAssetType" runat="server"></asp:DropDownList>
        <br /><br />

        <label>Category:</label>
        <asp:DropDownList ID="ddlCategory" runat="server"></asp:DropDownList>
        <br /><br />

        <label>Sub Category:</label>
        <asp:DropDownList ID="ddlSubCategory" runat="server"></asp:DropDownList>
        <br /><br />

        <label>Model:</label>
        <asp:DropDownList ID="ddlModel" runat="server"></asp:DropDownList>
        <br /><br />

        <label>Control:</label>
        <asp:DropDownList ID="ddlControl" runat="server"></asp:DropDownList>
        <br /><br />
        
        <label>Material Name:</label>
        <asp:TextBox ID="txtMaterialName" runat="server"></asp:TextBox>
        <br /><br />

        <label>Part Id:</label>
        <asp:TextBox ID="txtPart_Id" runat="server"></asp:TextBox>
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
