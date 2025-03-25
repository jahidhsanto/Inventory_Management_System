<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/Site1.Master" AutoEventWireup="true" CodeBehind="testweb.aspx.cs" Inherits="STORE_FINAL.UserDashboard.testweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


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
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card dashboard-card bg-success text-white">
                        <h4>📑 Pending Requisitions</h4>
                        <p>12 Requests</p>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card dashboard-card bg-warning text-dark">
                        <h4>🚚 Deliveries in Progress</h4>
                        <p>5 Deliveries</p>
                    </div>
                </div>
            </div>

        </div>

        <div id="employeeDashboard" runat="server" visible="true" class="card bg-light p-3">
            <h4>📝 Employee Actions</h4>
            <ul>
                <li><a href="Requisition.aspx">Place Requisition</a></li>
                <li><a href="MyRequisitions.aspx">View My Requisitions</a></li>
                <li><a href="RequisitionStatus.aspx">Check Status</a></li>
            </ul>
        </div>

        <div id="departmentHeadDashboard" runat="server" visible="true" class="card bg-light p-3">
            <h4>🏢 Department Head Actions</h4>
            <ul>
                <li><a href="ApproveRequisitions.aspx">Approve Requisitions</a></li>
                <li><a href="DepartmentStock.aspx">Department Stock</a></li>
                <li><a href="Reports.aspx">Generate Reports</a></li>
            </ul>
        </div>

    </div>
</asp:Content>
