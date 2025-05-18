<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="ChallanPendingReport.aspx.cs" Inherits="STORE_FINAL.Test.ChallanPendingReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .table th, .table td {
            vertical-align: middle !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-3">
        <h4 class="mb-4">Pending Challans</h4>
        
        <p>Work In Progress . . .</p>

        <asp:GridView ID="gvChallanPending" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="Challan_ID" HeaderText="Challan ID" />
                <asp:BoundField DataField="Challan_Date" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="Challan_Type" HeaderText="Type" />
                <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
