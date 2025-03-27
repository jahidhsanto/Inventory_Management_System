<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="Departments.aspx.cs" Inherits="STORE_FINAL.Role_Admin.Departments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="text-center">Department Table</h2>

    <asp:Panel ID="Panel1" runat="server" CssClass="table-responsive" style="overflow-x: auto; white-space: nowrap;">
        <asp:GridView 
            ID="DepartmentGridView" 
            runat="server" 
            AutoGenerateColumns="false" 
            CssClass="table table-bordered table-striped"
            HorizontalAlign="Left">
            <Columns>
                <asp:BoundField DataField="DepartmentName" HeaderText="Department" />
                <asp:BoundField DataField="Department_Head_ID" HeaderText="Head Employee ID" />
                <asp:BoundField DataField="Name" HeaderText="Head Name" />
                <asp:BoundField DataField="Designation" HeaderText="Designation" />
            </Columns>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
