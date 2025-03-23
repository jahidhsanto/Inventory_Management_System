<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Material.aspx.cs" Inherits="STORE_FINAL.Pages.Material" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>Material List</h2>
    
    <div class="row mb-3">
        <div class="col-md-3">
            <asp:TextBox ID="txtSearchPartID" runat="server" CssClass="form-control" Placeholder="Search Part ID"></asp:TextBox>
        </div>
        <div class="col-md-3">
            <asp:TextBox ID="txtSearchMaterialName" runat="server" CssClass="form-control" Placeholder="Search Material Name"></asp:TextBox>
        </div>



        <div class="col-md-3">
            <asp:DropDownList ID="ddlCom_Non_Com" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>

        <div class="col-md-3">
            <asp:DropDownList ID="ddlAsset_Status" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="ddlAsset_Type_Grouped" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="ddlSub_Category" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="ddlModel" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="ddlControl" runat="server" CssClass="form-control"></asp:DropDownList>
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

    <asp:Panel ID="Panel1" runat="server" CssClass="table-responsive" style="overflow-x: auto; white-space: nowrap;">
    <asp:GridView ID="gvMaterials" runat="server" AutoGenerateColumns="False" 
        CssClass="table table-striped" HorizontalAlign="Left"
        OnRowCommand="gvMaterials_RowCommand">
<%--    CssClass="table table-bordered table-striped"--%>
        <Columns>
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:Button ID="btnView" runat="server" CommandName="ViewDetails" 
                        CommandArgument='<%# Eval("Material_ID") %>' Text="View Details" CssClass="btn btn-primary"/>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField DataField="Material_ID" HeaderText="ID" />
            <asp:BoundField DataField="Part_Id" HeaderText="Part ID" />
            <asp:BoundField DataField="Materials_Name" HeaderText="Material Name" />
            <asp:BoundField DataField="Stock_Quantity" HeaderText="Stock Quantity" />
            <asp:BoundField DataField="UoM" HeaderText="UoM Unit of Measure" />
            <asp:BoundField DataField="Com_Non_Com" HeaderText="Com_Non_Com" />
            <asp:BoundField DataField="Asset_Status" HeaderText="Asset_Status" />
            <asp:BoundField DataField="Asset_Type_Grouped" HeaderText="Asset_Type_Grouped" />
            <asp:BoundField DataField="Category" HeaderText="Category" />
            <asp:BoundField DataField="Sub_Category" HeaderText="Sub_Category" />
            <asp:BoundField DataField="Model" HeaderText="Model" />
            <asp:BoundField DataField="Control" HeaderText="Control" />
            <asp:BoundField DataField="Unit_Price" HeaderText="Unit_Price" />
            
        </Columns>
    </asp:GridView>
    </asp:Panel>



</asp:Content>
