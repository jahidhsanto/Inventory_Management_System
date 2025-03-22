<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LocationReport.aspx.cs" Inherits="STORE_FINAL.Pages.LocationReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>Stock Location Report</h2>
    
    <div class="row mb-3">

        <div class="col-md-4">
            <asp:DropDownList ID="ddlRack" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
        
        <div class="col-md-4">
            <asp:DropDownList ID="ddlShelf" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>

        <div class="col-md-3">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
        </div>
    </div>

    <asp:GridView ID="gvLocationReport" runat="server" AutoGenerateColumns="False" CssClass="table table-striped"
        OnRowCommand="gvLocationReport_RowCommand">
        <Columns>
            <asp:BoundField DataField="Rack_Number" HeaderText="Rack Number" />
            <asp:BoundField DataField="Shelf_Number" HeaderText="Shelf Number" />
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:Button ID="btnView" runat="server" CommandName="ViewDetails" 
                        CommandArgument='<%# Eval("Rack_Number") %>' Text="View Details" CssClass="btn btn-primary"/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>
