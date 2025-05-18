<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="STORE_FINAL.Test.Reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .report-card {
            background: #f8f9fa;
            border-left: 4px solid #0d6efd;
            padding: 16px 20px;
            border-radius: 6px;
            transition: all 0.3s ease;
            text-decoration: none;
            color: inherit;
        }

        .report-card:hover {
            background-color: #e9ecef;
            transform: translateY(-3px);
            text-decoration: none;
        }

        .report-icon {
            font-size: 1.5rem;
            color: #0d6efd;
            margin-left: 10px;
        }

        .report-card h6 {
            font-size: 0.9rem;
            margin-bottom: 3px;
            font-weight: 500;
        }

        .report-card h4 {
            font-size: 1.3rem;
            margin: 0;
            font-weight: 600;
        }

        a.report-link {
            text-decoration: none;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <div class="container-fluid mt-4">
        <h4 class="mb-4 fw-bold">Reports Dashboard</h4>

        <div class="row">

            <div class="col-md-4 mb-3">
                <a class="report-link" href="PurposeWiseReport.aspx">
                    <div class="report-card border-primary">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6>Monthly Purpose-wise In/Out</h6>
                                <h4><asp:Label ID="lblPurposeWise" runat="server" Text="0" /></h4>
                            </div>
                            <i class="fas fa-exchange-alt report-icon text-primary"></i>
                        </div>
                    </div>
                </a>
            </div>

            <div class="col-md-4 mb-3">
                <a class="report-link" href="NonMovingReport.aspx">
                    <div class="report-card border-dark">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6>Monthly Non-Moving Report</h6>
                                <h4><asp:Label ID="lblNonMoving" runat="server" Text="0" /></h4>
                            </div>
                            <i class="fas fa-box-open report-icon text-dark"></i>
                        </div>
                    </div>
                </a>
            </div>

            <div class="col-md-4 mb-3">
                <a class="report-link" href="ChallanPendingReport.aspx">
                    <div class="report-card border-warning">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6>Monthly Challan Pending</h6>
                                <h4><asp:Label ID="lblChallanPending" runat="server" Text="0" /></h4>
                            </div>
                            <i class="fas fa-truck-loading report-icon text-warning"></i>
                        </div>
                    </div>
                </a>
            </div>

            <div class="col-md-4 mb-3">
                <a class="report-link" href="TestPendingReport.aspx">
                    <div class="report-card border-info">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6>Monthly Test Pending</h6>
                                <h4><asp:Label ID="lblTestPending" runat="server" Text="0" /></h4>
                            </div>
                            <i class="fas fa-vials report-icon text-info"></i>
                        </div>
                    </div>
                </a>
            </div>

            <div class="col-md-4 mb-3">
                <a class="report-link" href="DefectiveReport.aspx">
                    <div class="report-card border-danger">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6>Monthly Defective Report</h6>
                                <h4><asp:Label ID="lblDefective" runat="server" Text="0" /></h4>
                            </div>
                            <i class="fas fa-tools report-icon text-danger"></i>
                        </div>
                    </div>
                </a>
            </div>

            <div class="col-md-4 mb-3">
                <a class="report-link" href="PRReport.aspx">
                    <div class="report-card border-success">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h6>Monthly PR Report</h6>
                                <h4><asp:Label ID="lblPR" runat="server" Text="0" /></h4>
                            </div>
                            <i class="fas fa-file-alt report-icon text-success"></i>
                        </div>
                    </div>
                </a>
            </div>

        </div>
    </div>

</asp:Content>
