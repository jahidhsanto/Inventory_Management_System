<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="MaterialDelivery_Test.aspx.cs" Inherits="STORE_FINAL.Test_02.MaterialDelivery_Test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<asp:DropDownList ID="ddlRequisition" runat="server" AutoPostBack="true" 
    OnSelectedIndexChanged="ddlRequisition_SelectedIndexChanged" 
    CssClass="form-control">
</asp:DropDownList>

<br /><br />

<asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="False" 
    OnRowDataBound="gvItems_RowDataBound" CssClass="table table-bordered">
    <Columns>
        <asp:BoundField DataField="Material_ID" HeaderText="Material ID" />
        <asp:BoundField DataField="Materials_Name" HeaderText="Material Name" />
        <asp:BoundField DataField="Requires_Serial_Number" HeaderText="Requires Serial" />
        <asp:TemplateField HeaderText="Input/Select">
            <ItemTemplate>
                <asp:HiddenField ID="hfMaterialID" runat="server" Value='<%# Eval("Material_ID") %>' />
                <asp:HiddenField ID="hfRequiresSerial" runat="server" Value='<%# Eval("Requires_Serial_Number") %>' />
                
                <asp:TextBox ID="txtQuantity" runat="server" Visible="false" CssClass="form-control" TextMode="Number"></asp:TextBox>
                
                <asp:ListBox ID="lstSerials" runat="server" Visible="false" SelectionMode="Multiple" CssClass="form-control serial-select"></asp:ListBox>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
</asp:Content>
