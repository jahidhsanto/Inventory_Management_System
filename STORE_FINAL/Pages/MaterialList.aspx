<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MaterialList.aspx.cs" Inherits="STORE_FINAL.Pages.MaterialList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>Material List</h2>

    <div class="row mb-3">
        <div class="col-md-4">
            <asp:TextBox ID="txtSearchPartID" runat="server" CssClass="form-control" Placeholder="Search Part ID"></asp:TextBox>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="txtSearchMaterialName" runat="server" CssClass="form-control" Placeholder="Search Material Name"></asp:TextBox>
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="ddlStockFilter" runat="server" CssClass="form-control">
                <asp:ListItem Value="">All Stock</asp:ListItem>
                <asp:ListItem Value="Available">Available Stock</asp:ListItem>
                <asp:ListItem Value="Reserved">Reserved Stock</asp:ListItem>
                <asp:ListItem Value="Delivered">Delivered</asp:ListItem>
                <asp:ListItem Value="Warranty">Warranty Stock</asp:ListItem>
                <asp:ListItem Value="Low">Low Stock (&lt; 5)</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-md-3">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
        </div>
    </div>

    <asp:GridView ID="gvMaterials" runat="server" AutoGenerateColumns="False" CssClass="table table-striped"
        OnRowCommand="gvMaterials_RowCommand">
        <Columns>
            <asp:BoundField DataField="Material_ID" HeaderText="ID" />
            <asp:BoundField DataField="Materials_Name" HeaderText="Material Name" />
            <asp:BoundField DataField="Stock_Quantity" HeaderText="Stock Quantity" />
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:Button ID="btnView" runat="server" CommandName="ViewDetails" 
                        CommandArgument='<%# Eval("Material_ID") %>' Text="View Details" CssClass="btn btn-primary"/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>
