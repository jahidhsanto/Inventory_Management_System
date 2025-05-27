<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="MaterialDelivery_Test.aspx.cs" Inherits="STORE_FINAL.Test_02.MaterialDelivery_Test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .serial-multiselect {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:DropDownList ID="ddlRequisition" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRequisition_SelectedIndexChanged" CssClass="form-control">
        <asp:ListItem Text="-- Select Requisition --" Value="0"></asp:ListItem>
    </asp:DropDownList>

    <br />

    <asp:GridView ID="gvMaterials" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" OnRowDataBound="gvMaterials_RowDataBound">
        <Columns>
            <asp:BoundField DataField="Material_ID" HeaderText="Material ID" />
            <asp:BoundField DataField="Materials_Name" HeaderText="Material Name" />
            <asp:BoundField DataField="Requires_Serial_Number" HeaderText="Requires Serial Number" />

            <asp:TemplateField HeaderText="Quantity">
                <ItemTemplate>
                    <asp:TextBox ID="txtQty" runat="server" TextMode="Number" onblur="validateQty(this)" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Serial Numbers">
                <ItemTemplate>
                    <asp:ListBox ID="lstSerialNumbers" runat="server" SelectionMode="Multiple" CssClass="serial-multiselect js-example-basic-hide-search-multi" Visible="false" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

<script>
    function validateQty(input) {
        if (parseInt(input.value) <= 0) {
            alert("Quantity must be greater than 0.");
            input.value = "";
        }
    }
</script>

</asp:Content>
