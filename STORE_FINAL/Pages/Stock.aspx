<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Stock.aspx.cs" Inherits="STORE_FINAL.Pages.Stock" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="text-center">Stock List</h2>

    <asp:Panel ID="Panel1" runat="server" CssClass="table-responsive" style="overflow-x: auto; white-space: nowrap;">
        <asp:GridView 
            ID="StockGridView" 
            runat="server" 
            AutoGenerateColumns="TRUE" 
            CssClass="table table-bordered table-striped"
            HorizontalAlign="Left">
            <Columns>
            </Columns>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
