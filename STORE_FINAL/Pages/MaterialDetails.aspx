<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MaterialDetails.aspx.cs" Inherits="STORE_FINAL.Pages.MaterialDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>Material Details</h2>
    
    <asp:Label ID="lblMaterialName" runat="server" CssClass="h4"></asp:Label>
    
    <h4>Item Locations</h4>
    <asp:GridView ID="gvMaterialTracking" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
        <Columns>
            <asp:BoundField DataField="Serial_Number" HeaderText="Serial Number" />
            <asp:BoundField DataField="Rack_Number" HeaderText="Rack Number" />
            <asp:BoundField DataField="Shelf_Number" HeaderText="Shelf Number" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:BoundField DataField="Received_Date" HeaderText="Received Date" />
        </Columns>
    </asp:GridView>

    <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="Material.aspx" CssClass="btn btn-secondary">Back to List</asp:HyperLink>

</asp:Content>
