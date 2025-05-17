<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="Test_Dashboard.aspx.cs" Inherits="STORE_FINAL.Test.Test_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .info-card {
            background: #f8f9fa;
            border-left: 4px solid #0d6efd;
            padding: 12px 15px;
            border-radius: 6px;
            transition: all 0.3s;
        }

            .info-card:hover {
                background-color: #e9ecef;
                transform: translateY(-2px);
            }

        .info-icon {
            font-size: 1.5rem;
            color: #0d6efd;
            margin-left: 10px;
        }

        .info-card h6 {
            font-size: 0.85rem;
            margin-bottom: 2px;
        }

        .info-card h4 {
            font-size: 1.1rem;
            margin: 0;
        }

        .activity-item {
            border-bottom: 1px solid #dee2e6;
            padding: 10px 0;
        }

        .chart-container {
            height: 220px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">
        <%--        <h2 class="mb-4">Inventory Overview</h2>--%>

        <!-- Summary Info Cards -->
        <div class="row mb-4">
            <div class="col-md-3 mb-3">
                <div class="info-card">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h6>Total Items</h6>
                            <h4>
                                <asp:Label ID="lblTotalItems" runat="server" /></h4>
                        </div>
                        <i class="fas fa-cubes info-icon"></i>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3">
                <div class="info-card border-success">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h6>Stock Quantity</h6>
                            <h4>
                                <asp:Label ID="lblStockQty" runat="server" /></h4>
                        </div>
                        <i class="fas fa-layer-group info-icon text-success"></i>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3">
                <div class="info-card border-dark">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h6>Pending Requisitions</h6>
                            <h4>
                                <asp:Label ID="lblPendingReq" runat="server" /></h4>
                        </div>
                        <i class="fas fa-hourglass-half info-icon text-dark"></i>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3">
                <div class="info-card border-info">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h6>Defective Stock</h6>
                            <h4>
                                <asp:Label ID="lblDefectiveStock" runat="server" /></h4>
                        </div>
                        <i class="fas fa-tools info-icon text-info"></i>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3">
                <div class="info-card border-primary">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h6>Stock Value</h6>
                            <h4>$<asp:Label ID="lblStockValue" runat="server" /></h4>
                        </div>
                        <i class="fas fa-dollar-sign info-icon text-primary"></i>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3">
                <div class="info-card border-warning">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h6>Low Stock</h6>
                            <h4>
                                <asp:Label ID="lblLowStock" runat="server" /></h4>
                        </div>
                        <i class="fas fa-exclamation-circle info-icon text-warning"></i>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3">
                <div class="info-card border-danger">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h6>Out of Stock</h6>
                            <h4>
                                <asp:Label ID="lblOutOfStock" runat="server" /></h4>
                        </div>
                        <i class="fas fa-ban info-icon text-danger"></i>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3">
                <div class="info-card border-info">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h6>Received Today</h6>
                            <h4>
                                <asp:Label ID="lblReceivedToday" runat="server" /></h4>
                        </div>
                        <i class="fas fa-download info-icon text-info"></i>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3">
                <div class="info-card border-secondary">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <h6>Issued Today</h6>
                            <h4>
                                <asp:Label ID="lblIssuedToday" runat="server" /></h4>
                        </div>
                        <i class="fas fa-upload info-icon text-secondary"></i>
                    </div>
                </div>
            </div>
        </div>


        <!-- Chart and Activity -->
        <div class="row">
            <div class="col-md-8">
                <div class="card shadow-sm mb-4">
                    <div class="card-header bg-white fw-bold">Stock Trend (Last 7 Days)</div>
                    <div class="card-body">
                        <div class="chart-container">
                            <canvas id="stockTrendChart"></canvas>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card shadow-sm mb-4">
                    <div class="card-header bg-white fw-bold">Requisition Status</div>
                    <div class="card-body">
<%--                        <div class="chart-container">--%>
                            <div class="chart-container d-flex justify-content-center align-items-center">

                            <canvas id="requisitionStatusChart" style="max-width: 100%; max-height: 100%;"></canvas>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>



    <!-- Serialize chart data to hidden fields -->
    <asp:HiddenField ID="hfStockTrendLabels" runat="server" />
    <asp:HiddenField ID="hfStockTrendData_Receive" runat="server" />
    <asp:HiddenField ID="hfStockTrendData_Delivery" runat="server" />
    <asp:HiddenField ID="hfRequisitionLabels" runat="server" />
    <asp:HiddenField ID="hfRequisitionCounts" runat="server" />

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            const stockTrendLabels = JSON.parse(document.getElementById('<%= hfStockTrendLabels.ClientID %>').value || '[]');
            const stockTrendReceived = JSON.parse(document.getElementById('<%= hfStockTrendData_Receive.ClientID %>').value || '[]');
            const stockTrendDelivered = JSON.parse(document.getElementById('<%= hfStockTrendData_Delivery.ClientID %>').value || '[]');

            const reqLabels = JSON.parse(document.getElementById('<%= hfRequisitionLabels.ClientID %>').value || '[]');
            const reqCounts = JSON.parse(document.getElementById('<%= hfRequisitionCounts.ClientID %>').value || '[]');

            const ctx1 = document.getElementById('stockTrendChart').getContext('2d');
            const gradient1 = ctx1.createLinearGradient(0, 0, 0, 250);
            gradient1.addColorStop(0, '#0d6efd88');
            gradient1.addColorStop(1, '#0d6efd10');

            const gradient2 = ctx1.createLinearGradient(0, 0, 0, 250);
            gradient2.addColorStop(0, '#dc354588');
            gradient2.addColorStop(1, '#dc354510');

            new Chart(ctx1, {
                type: 'line',
                data: {
                    labels: stockTrendLabels,
                    datasets: [
                        {
                            label: 'Received',
                            data: stockTrendReceived,
                            borderColor: '#0d6efd',
                            backgroundColor: gradient1,
                            fill: true,
                            tension: 0.4,
                            pointRadius: 4,
                            pointHoverRadius: 6
                        },
                        {
                            label: 'Delivered',
                            data: stockTrendDelivered,
                            borderColor: '#dc3545',
                            backgroundColor: gradient2,
                            fill: true,
                            tension: 0.4,
                            pointRadius: 4,
                            pointHoverRadius: 6
                        }
                    ]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            position: 'top'
                        },
                        tooltip: {
                            backgroundColor: '#fff',
                            titleColor: '#000',
                            bodyColor: '#000',
                            borderColor: '#ccc',
                            borderWidth: 1
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true,
                            ticks: { stepSize: 20 },
                            grid: { color: '#e9ecef' }
                        },
                        x: { grid: { display: false } }
                    }
                }
            });

            const ctx2 = document.getElementById('requisitionStatusChart').getContext('2d');
            new Chart(ctx2, {
                type: 'doughnut',
                data: {
                    labels: reqLabels,
                    datasets: [{
                        data: reqCounts,
                        backgroundColor: ['#ffc107', '#198754', '#dc3545', '#0dcaf0'],
                        hoverOffset: 10
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: { position: 'bottom' },
                        tooltip: {
                            backgroundColor: '#fff',
                            titleColor: '#000',
                            bodyColor: '#000',
                            borderColor: '#ccc',
                            borderWidth: 1
                        }
                    }
                }
            });
        });
    </script>




</asp:Content>
