<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="Report_01.aspx.cs" Inherits="STORE_FINAL.Role_StoreIncharge.Report.Report_01" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .filter-section { margin-bottom: 20px; }
        .report-grid { margin-top: 20px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="filter-section">
        <asp:Label ID="lblReq" runat="server" Text="Requisition No:"></asp:Label>
        <asp:TextBox ID="txtReq" runat="server"></asp:TextBox>

        <asp:Label ID="lblEmployee" runat="server" Text="Employee:"></asp:Label>
        <asp:DropDownList ID="ddlEmployee" runat="server"></asp:DropDownList>

        <asp:Label ID="lblProject" runat="server" Text="Project:"></asp:Label>
        <asp:DropDownList ID="ddlProject" runat="server"></asp:DropDownList>

        <asp:Label ID="lblZone" runat="server" Text="Zone:"></asp:Label>
        <asp:DropDownList ID="ddlZone" runat="server"></asp:DropDownList>

        <%--<asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" />--%>
        <asp:Button ID="btnFilter" runat="server" Text="Filter" />
    </div>

    <asp:GridView ID="gvMaterialWise" runat="server" CssClass="report-grid" AutoGenerateColumns="true" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"></asp:GridView>
    <asp:GridView ID="gvZoneWise" runat="server" CssClass="report-grid" AutoGenerateColumns="true" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"></asp:GridView>
    <asp:GridView ID="gvProjectWise" runat="server" CssClass="report-grid" AutoGenerateColumns="true" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"></asp:GridView>
    <asp:GridView ID="gvEmployeeWise" runat="server" CssClass="report-grid" AutoGenerateColumns="true" HeaderStyle-BackColor="#3AC0F2" HeaderStyle-ForeColor="White"></asp:GridView>
</asp:Content>
