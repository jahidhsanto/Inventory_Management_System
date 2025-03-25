<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="UserDashboard.aspx.cs" Inherits="STORE_FINAL.UserDashboard.UserDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        /* Dashboard Cards */
        .dashboard-card {
            border-radius: 10px;
            padding: 20px;
            transition: transform 0.3s;
        }

            .dashboard-card:hover {
                transform: scale(1.05);
            }
    </style>

    <div class="container mt-4">
        <h2 class="mt-3">🌟 Welcome to Your Dashboard</h2>
        <p class="text-muted">Manage your tasks efficiently based on your role</p>
        <hr>

        <!-- Dashboard Content -->
        <div id="storePersonDashboard" runat="server" visible="true" class="card bg-light p-3">
            <h4>📦 Store Management</h4>
            <div class="row">
                <div class="col-md-4">
                    <div class="card dashboard-card bg-primary text-white">
                        <h4>📦 Total Stock</h4>
                        <p>500 Items</p>
                        <asp:Button ID="Button1" runat="server" Text="" CssClass="btn btn-light" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card dashboard-card bg-success text-white">
                        <h4>📑 Pending Requisitions</h4>
                        <p>12 Requests</p>
                        <asp:Button ID="Button2" runat="server" Text="" CssClass="btn btn-light" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card dashboard-card bg-warning text-dark">
                        <h4>🚚 Deliveries in Progress</h4>
                        <p>5 Deliveries</p>
                        <!-- Add a button for managing deliveries -->
                        <asp:Button ID="Button3" runat="server" Text="" CssClass="btn btn-light" />
                    </div>
                </div>
            </div>
        </div>

        <%--        📊 Manage Stock 
        🚚 Deliver Materials
        📑 Pending Requisitions
        🏢 View Stock
        📦 Total Stock
        📑 Pending Requisitions
        🚚 Deliveries in Progress--%>


        <div id="employeeDashboard" runat="server" visible="true" class="card bg-light p-3">
            <h4>📝 Employee Actions</h4>
            <div class="row">
                <div class="col-md-4">
                    <div class="card dashboard-card bg-primary text-white">
                        <h4>H4</h4>
                        <p>p</p>
                        <asp:Button ID="Button4" runat="server" Text="📝 Place Requisition" CssClass="btn btn-light" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card dashboard-card bg-primary text-white">
                        <h4>H4</h4>
                        <p>p</p>
                        <asp:Button ID="Button5" runat="server" Text="📋 View My Requisitions" CssClass="btn btn-light" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card dashboard-card bg-primary text-white">
                        <h4>H4</h4>
                        <p>p</p>
                        <asp:Button ID="Button6" runat="server" Text="🔍 Check Status" CssClass="btn btn-light" />
                    </div>
                </div>
            </div>
        </div>

        <div id="departmentHeadDashboard" runat="server" visible="true" class="card bg-light p-3">
            <h4>🏢 Department Head Actions</h4>
            <div class="row">
                <div class="col-md-4">
                    <div class="card dashboard-card bg-primary text-white">
                        <h4>H4</h4>
                        <p>p</p>
                        <asp:Button ID="Button7" runat="server" Text="✅ Approve Requisitions" CssClass="btn btn-light" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card dashboard-card bg-primary text-white">
                        <h4>H4</h4>
                        <p>p</p>
                        <asp:Button ID="Button8" runat="server" Text="🏢 Department Stock" CssClass="btn btn-light" />
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card dashboard-card bg-primary text-white">
                        <h4>H4</h4>
                        <p>p</p>
                        <asp:Button ID="Button9" runat="server" Text="📈 Generate Reports" CssClass="btn btn-light" />
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
