<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="MaterialLedger.aspx.cs" Inherits="STORE_FINAL.Role_StoreIncharge.Report.MaterialLedger" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="card shadow">
            <div class="card-header bg-primary text-white">
                <h5 class="mb-0">Material Ledger Report</h5>
            </div>
            <div class="card-body">
                <div class="row mb-3">
                    <div class="col-md-4">
                        <label for="ddlMaterial" class="form-label">Select Material</label>
                        <asp:DropDownList ID="ddlMaterial" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlMaterial_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label for="txtFromDate" class="form-label">From Date</label>
                        <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-3">
                        <label for="txtToDate" class="form-label">To Date</label>
                        <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="col-md-2 d-flex align-items-end">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-success w-100" OnClick="btnSearch_Click" />
                    </div>
                </div>
                <asp:Button ID="btnExportExcel" runat="server" CssClass="btn btn-outline-primary mb-3" Text="Export to Excel" OnClick="btnExportExcel_Click" />

                <div class="row">
                    <asp:Label ID="lblExportError" runat="server" CssClass="text-danger" Visible="false"></asp:Label>

                    <asp:GridView ID="gvMaterialLedger" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="false" EmptyDataText="No data found.">
                        <Columns>
                            <asp:BoundField DataField="Challan_Date" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="Ledger_Type" HeaderText="Type" />
                            <asp:BoundField DataField="In_Quantity" HeaderText="In Qty" />
                            <asp:BoundField DataField="Out_Quantity" HeaderText="Out Qty" />
                            <asp:BoundField DataField="Unit_Price" HeaderText="Unit Price" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="Balance_After_Transaction" HeaderText="Balance" />
                            <asp:BoundField DataField="Valuation_After_Transaction" HeaderText="Valuation" DataFormatString="{0:N2}" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
</asp:Content>
