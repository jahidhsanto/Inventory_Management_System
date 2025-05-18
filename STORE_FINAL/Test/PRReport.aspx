<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="PRReport.aspx.cs" Inherits="STORE_FINAL.Test.PRReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Monthly Purchase Requisition Summary</h2>
        <asp:GridView ID="gvPRSummary" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="Requisition_Type" HeaderText="Type" />
                <asp:BoundField DataField="TotalPR" HeaderText="Total Requisitions" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
