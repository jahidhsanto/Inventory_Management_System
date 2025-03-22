<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LocationDetails.aspx.cs" Inherits="STORE_FINAL.Pages.LocationDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>Location Details</h2>
    
    <asp:Label ID="lblMaterialName" runat="server" CssClass="h4"></asp:Label>
    
    <h4>Item List</h4>
    <asp:GridView ID="gvMaterialTracking" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
        <Columns>
            <asp:BoundField DataField="Shelf_number" HeaderText="Shelf Number" />
            <asp:BoundField DataField="Material_ID" HeaderText="Material ID" />
            <asp:BoundField DataField="Part_Id" HeaderText="Part ID" />
            <asp:BoundField DataField="Materials_Name" HeaderText="Materials Name" />
            <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
        </Columns>
    </asp:GridView>

    <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="LocationReport.aspx" CssClass="btn btn-secondary">Back to List</asp:HyperLink>

</asp:Content>
