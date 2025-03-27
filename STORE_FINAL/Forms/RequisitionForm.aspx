<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RequisitionForm.aspx.cs" Inherits="STORE_FINAL.Forms.RequisitionForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Material Requisition Form</h2>

    <label>Select Material:</label>
    <asp:DropDownList ID="ddlMaterials" runat="server"></asp:DropDownList>
    <br /><br />

    <label>Quantity:</label>
    <asp:TextBox ID="txtQuantity" runat="server"></asp:TextBox>
    <br /><br />

    <asp:Button ID="btnSubmitRequisition" runat="server" Text="Submit Request" OnClick="btnSubmitRequisition_Click" />
    <br /><br />

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

</asp:Content>
