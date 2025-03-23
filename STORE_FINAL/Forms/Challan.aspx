<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Challan.aspx.cs" Inherits="STORE_FINAL.Forms.Challan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-5">
        <div class="card shadow-lg p-4">
            <h2 class="text-center text-primary mb-4"><i class="bi bi-truck"></i> Material Delivery</h2>

            <!-- Dropdown for Requisitions -->
            <div class="mb-3">
                <label for="ddlApprovedRequisitions" class="form-label fw-bold">Select Requisition:</label>
                <asp:DropDownList ID="ddlApprovedRequisitions" CssClass="form-select" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlApprovedRequisitions_SelectedIndexChanged"></asp:DropDownList>
            </div>

            <!-- Stock Details Table -->
            <div class="table-responsive">
                <table class="table table-hover table-bordered">
                    <thead class="table-dark">
                        <tr>
                            <th>Serial Number</th>
                            <th>Status</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:GridView ID="gvStockDetails" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" HeaderStyle-CssClass="table-primary">
                            <Columns>
                                <asp:BoundField DataField="Serial_Number" HeaderText="Serial Number" />
                                <asp:BoundField DataField="Status" HeaderText="Status" />
                            </Columns>
                        </asp:GridView>
                    </tbody>
                </table>
            </div>

            <!-- Deliver Button -->
            <div class="text-center mt-4">
                <asp:Button ID="btnDeliver" runat="server" Text="🚚 Deliver Now" CssClass="btn btn-success btn-lg shadow-sm" OnClick="btnDeliver_Click" />
            </div>
        </div>
    </div>

    <style>
        body {
            background-color: #f8f9fa;
        }
        .card {
            border-radius: 15px;
        }
        .btn-success {
            transition: 0.3s ease-in-out;
        }
        .btn-success:hover {
            background-color: #28a745;
            transform: scale(1.05);
        }
        .table-hover tbody tr:hover {
            background-color: #d4edda !important;
            cursor: pointer;
        }
    </style>

</asp:Content>
