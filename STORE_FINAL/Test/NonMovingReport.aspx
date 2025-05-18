<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="NonMovingReport.aspx.cs" Inherits="STORE_FINAL.Test.NonMovingReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .table th, .table td {
            vertical-align: middle !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-3">
        <h4 class="mb-4">Non-Moving Materials - Monthly</h4>

        <div class="container mt-4">
            <asp:GridView ID="gvNonMoving" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="Material_ID" HeaderText="ID" />
                    <asp:BoundField DataField="Materials_Name" HeaderText="Material Name" />
                    <asp:BoundField DataField="Stock_Quantity" HeaderText="Stock Qty" />
                    <asp:BoundField DataField="Unit_Price" HeaderText="Unit Price" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
