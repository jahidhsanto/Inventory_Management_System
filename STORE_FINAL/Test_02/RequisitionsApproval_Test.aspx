<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="RequisitionsApproval_Test.aspx.cs" Inherits="STORE_FINAL.Test_02.RequisitionsApproval_Test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .remarks-box {
            width: 100%;
            border-radius: 6px;
            padding: 8px;
        }

        /* Custom header styles */
        .header-title {
            font-size: 1.5rem;
            font-weight: bold;
            color: #007bff;
        }

        .filter-container {
            display: flex;
            justify-content: space-between;
            gap: 1rem;
            align-items: center;
        }

            .filter-container .col-md-4 {
                flex: 1;
            }

        .badge-container {
            display: flex;
            gap: 1rem;
            align-items: center;
        }

            .badge-container span {
                font-size: 1rem;
            }

        .dropdown-container {
            display: flex;
            gap: 0.5rem;
            flex-wrap: wrap;
        }

            .dropdown-container .form-select,
            .dropdown-container .form-control {
                flex: 1;
                max-width: 200px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mb-4">
        <div class="col-12">
            <h3 class="header-title">Department Approval Panel</h3>
        </div>
    </div>
    <asp:UpdatePanel ID="updPanelFilters" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <!-- Filter and Status Section -->
            <div class="filter-container">
                <div class="dropdown-container">
                    <!-- Status Filter Dropdown -->
                    <div class="col-md-4">

                        <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-select"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">
                            <%--                    <asp:ListItem Text="All Status" Value="" />
                    <asp:ListItem Text="Pending" Value="Pending" />
                    <asp:ListItem Text="Approved" Value="Approved" />
                    <asp:ListItem Text="Rejected" Value="Rejected" />--%>
                        </asp:DropDownList>
                    </div>

                    <!-- Employee Filter Dropdown -->
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlEmployeeFilter" runat="server" CssClass="form-select"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged" />
                    </div>

                    <!-- Requisition ID Search -->
                    <div class="col-md-4">
                        <asp:TextBox ID="txtSearchRequisitionID" runat="server" CssClass="form-control"
                            placeholder="Search by Requisition ID" AutoPostBack="true"
                            OnTextChanged="ddlStatusFilter_SelectedIndexChanged" />
                    </div>

                    <!-- Date Filter Section with Date Picker -->
                    <div class="col-md-4">
                        <!-- Start Date TextBox -->
                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control"
                            placeholder="Start Date" AutoPostBack="true"
                            OnTextChanged="ddlStatusFilter_SelectedIndexChanged" TextMode="Date" />
                    </div>
                    <div class="col-md-4">
                        <!-- End Date TextBox -->
                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control"
                            placeholder="End Date" AutoPostBack="true"
                            OnTextChanged="ddlStatusFilter_SelectedIndexChanged" TextMode="Date" />
                    </div>
                </div>

                <!-- Badge Summary Section -->
                <div class="badge-container">
                    <span class="badge bg-warning text-dark">🟡 Pending: 
                <asp:Label ID="lblPendingCount" runat="server" />
                    </span>
                    <span class="badge bg-success">🟢 Approved: 
                <asp:Label ID="lblApprovedCount" runat="server" />
                    </span>
                    <span class="badge bg-danger">🔴 Rejected: 
                <asp:Label ID="lblRejectedCount" runat="server" />
                    </span>
                </div>
            </div>

            <!-- Requisition Summary -->
            <div class="container mt-4">
                <div class="card shadow">
                    <div class="card-header bg-primary text-white">
                        <h5 class="mb-0"><i class="bi bi-clipboard-data"></i>Requisition Summary</h5>
                    </div>

                    <div class="card-body">
                        <asp:Repeater ID="rptRequisitions" runat="server" OnItemDataBound="rptRequisitions_ItemDataBound" OnItemCommand="rptRequisitions_ItemCommand">
                            <HeaderTemplate>
                                <div class="accordion" id="accordionExample">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="accordion-item">
                                    <h2 class="accordion-header" id="heading<%# Container.ItemIndex %>">
                                        <button class="accordion-button collapsed d-flex justify-content-between align-items-center"
                                            type="button" data-bs-toggle="collapse"
                                            data-bs-target="#collapse<%# Container.ItemIndex %>"
                                            aria-expanded="false" aria-controls="collapse<%# Container.ItemIndex %>">
                                            <div class="w-100">
                                                <strong>Req ID:</strong> <%# Eval("Requisition_ID") %> |
                                        <strong>By:</strong> <%# Eval("CreatedBy") %> |
                                        <strong>Date:</strong> <%# Eval("Created_Date", "{0:yyyy-MM-dd}") %> |
                                        <strong>For:</strong> <%# Eval("Requisition_For") %>
                                            </div>

                                            <!-- Show Status Badge on the Right -->
                                            <span class='badge <%# Eval("Dept_Status").ToString() == "Approved" ? "bg-success" : 
                                            Eval("Dept_Status").ToString() == "Rejected" ? "bg-danger" : 
                                            "bg-warning text-dark" %>'>
                                                <%# Eval("Dept_Status") %>
                                            </span>

                                        </button>
                                    </h2>
                                    <div id="collapse<%# Container.ItemIndex %>" class="accordion-collapse collapse"
                                        aria-labelledby="heading<%# Container.ItemIndex %>" data-bs-parent="#accordionExample">
                                        <div class="accordion-body">
                                            <!-- GridView for Requisition Items -->
                                            <asp:GridView ID="gvItems" runat="server" CssClass="table table-bordered table-hover"
                                                AutoGenerateColumns="false" GridLines="None">
                                                <Columns>
                                                    <asp:BoundField DataField="Materials_Name" HeaderText="Material Name" />
                                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                                                </Columns>
                                            </asp:GridView>

                                            <!-- Remarks Input -->
                                            <div class="mt-3">
                                                <label class="form-label"><strong>Remarks:</strong></label>
                                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine"
                                                    CssClass="form-control remarks-box border border-secondary"
                                                    Rows="3" placeholder="Enter remarks here..." />
                                            </div>

                                            <!-- Action Buttons & Existing Status -->
                                            <div class="mt-2 d-flex justify-content-end gap-2">
                                                <asp:Button ID="btnApprove" runat="server" CommandName="Approve" CommandArgument='<%# Eval("Requisition_ID") %>' Text="✓ Approve" CssClass="btn btn-success btn-sm px-3" />
                                                <asp:Button ID="btnReject" runat="server" CommandName="Reject" CommandArgument='<%# Eval("Requisition_ID") %>' Text="✗ Reject" CssClass="btn btn-danger btn-sm px-3" />
                                                <asp:Button ID="btnPending" runat="server" CommandName="Pending" CommandArgument='<%# Eval("Requisition_ID") %>' Text="⏳ Pending" CssClass="btn btn-warning btn-sm px-3" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                            <FooterTemplate>
                                </div>
                        <!-- Close accordion -->
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlStatusFilter" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlEmployeeFilter" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="txtSearchRequisitionID" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="txtStartDate" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="txtEndDate" EventName="TextChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <!-- Accordion Scroll Script -->
    <script>
        document.addEventListener('shown.bs.collapse', function (e) {
            const expanded = e.target.closest('.accordion-item');
            expanded.scrollIntoView({ behavior: 'smooth' });
        });
    </script>
</asp:Content>
