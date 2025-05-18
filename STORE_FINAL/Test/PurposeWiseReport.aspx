<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="PurposeWiseReport.aspx.cs" Inherits="STORE_FINAL.Test.PurposeWiseReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .table th, .table td {
            vertical-align: middle !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-3">
        <h4 class="mb-4">Purpose Wise In/Out Summary</h4>

        <div class="row mb-3">
            <div class="col-md-3">
                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="col-md-3">
                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <div class="col-md-2">
                <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnFilter_Click" />
            </div>
        </div>

        <asp:GridView ID="gvPurposeWise" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="Requisition_For" HeaderText="Purpose" />
                <asp:BoundField DataField="InCount" HeaderText="In Count" />
                <asp:BoundField DataField="OutCount" HeaderText="Out Count" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
