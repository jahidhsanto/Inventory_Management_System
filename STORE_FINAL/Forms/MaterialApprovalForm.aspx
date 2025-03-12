<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MaterialApprovalForm.aspx.cs" Inherits="STORE_FINAL.Forms.MaterialApprovalForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Approve Material Requisition</h2>

    <label>Select Requisition:</label>
    <asp:DropDownList ID="ddlRequisitions" runat="server"></asp:DropDownList>
    <br /><br />

    <label>Status:</label>
    <asp:DropDownList ID="ddlStatus" runat="server">
        <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
        <asp:ListItem Text="Rejected" Value="Rejected"></asp:ListItem>
    </asp:DropDownList>
    <br /><br />

    <label>Remarks:</label>
    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"></asp:TextBox>
    <br /><br />

    <asp:Button ID="btnApprove" runat="server" Text="Submit" OnClick="btnApprove_Click" />
    <br /><br />

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

</asp:Content>
