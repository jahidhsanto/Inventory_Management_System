<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddNewMaterial.aspx.cs" Inherits="STORE_FINAL.Forms.AddNewMaterial" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Add New Material</h2>

    <div class="container">
        <div class="row">
            <div class="col-12 col-md-3 mb-3">
                <label for="ddlCom_NonCom">Com/Non-Com:</label>
                <asp:DropDownList ID="ddlCom_NonCom" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <div class="col-12 col-md-3 mb-3">
                <label for="ddlAssetStatus">Asset Status:</label>
                <asp:DropDownList ID="ddlAssetStatus" runat="server" CssClass="form-control select2"></asp:DropDownList>
            </div>

            <div class="col-12 col-md-3 mb-3">
                <label for="ddlAssetType">Asset Type:</label>
                <asp:DropDownList ID="ddlAssetType" runat="server" CssClass="form-control select2"></asp:DropDownList>
            </div>

            <div class="col-12 col-md-3 mb-3">
                <label for="ddlCategory">Category:</label>
                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control select2"></asp:DropDownList>
            </div>

            <div class="col-12 col-md-3 mb-3">
                <label for="ddlSubCategory">Sub Category:</label>
                <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="form-control select2"></asp:DropDownList>
            </div>

            <div class="col-12 col-md-3 mb-3">
                <label for="ddlModel">Model:</label>
                <asp:DropDownList ID="ddlModel" runat="server" CssClass="form-control select2"></asp:DropDownList>
            </div>

            <div class="col-12 col-md-3 mb-3">
                <label for="ddlControl">Control:</label>
                <asp:DropDownList ID="ddlControl" runat="server" CssClass="form-control select2"></asp:DropDownList>
            </div>

            <div class="col-12 col-md-3 mb-3">
                <label for="txtMaterialName">Material Name:</label>
                <asp:TextBox ID="txtMaterialName" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="col-12 col-md-3 mb-3">
                <label for="txtPart_Id">Part Id:</label>
                <asp:TextBox ID="txtPart_Id" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="col-12 col-md-3 mb-3">
                <label for="txtUnitPrice">Unit Price:</label>
                <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="col-12 col-md-3 mb-3
                <label for="ddlUoM">UoM:</label>
                <asp:DropDownList ID="ddlUoM" runat="server" CssClass="form-control select2"></asp:DropDownList>
            </div>

            <div class="col-12 mt-3">
                <asp:Button ID="btnAddMaterial" runat="server" Text="Add Material" OnClick="btnAddMaterial_Click" CssClass="btn btn-primary" />
            </div>
        </div>
    </div>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
</asp:Content>
