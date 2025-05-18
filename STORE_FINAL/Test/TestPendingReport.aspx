<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="TestPendingReport.aspx.cs" Inherits="STORE_FINAL.Test.TestPendingReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Test Pending Materials</h2>
        <asp:GridView ID="gvTestPending" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="Stock_ID" HeaderText="Stock ID" />
                <asp:BoundField DataField="Materials_Name" HeaderText="Material" />
                <asp:BoundField DataField="Serial_Number" HeaderText="Serial No." />
                <asp:BoundField DataField="Received_Date" HeaderText="Received On" DataFormatString="{0:yyyy-MM-dd}" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
