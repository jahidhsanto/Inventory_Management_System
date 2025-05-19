<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="Report_Requisition.aspx.cs" Inherits="STORE_FINAL.Test.Report_Requisition" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- ============================================= Requisition Summary ============================================= --%>
    <div class="container mt-4">
        <div class="card shadow">
            <div class="card-header bg-primary text-white">
                <h5 class="mb-0"><i class="bi bi-clipboard-data"></i>Requisition Summary</h5>
            </div>
            <div class="card-body">
                <asp:Repeater ID="rptCategories" runat="server" OnItemDataBound="rptCategories_ItemDataBound">
                    <HeaderTemplate>
                        <div class="accordion" id="accordionExample">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="accordion-item">
                            <h2 class="accordion-header" id="heading<%# Container.ItemIndex %>">
                                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse"
                                    data-bs-target="#collapse<%# Container.ItemIndex %>" aria-expanded="false"
                                    aria-controls="collapse<%# Container.ItemIndex %>">
                                    <i class="bi bi-folder-fill me-2"></i>
                                    <%# Eval("Requisition_For") %> (<%# Eval("TotalRequests") %> requests)
                                </button>
                            </h2>
                            <div id="collapse<%# Container.ItemIndex %>" class="accordion-collapse collapse"
                                aria-labelledby="heading<%# Container.ItemIndex %>" data-bs-parent="#accordionExample">
                                <div class="accordion-body">
                                    <asp:GridView ID="gvDetails" runat="server" CssClass="table table-bordered table-hover requisitionTable"
                                        AutoGenerateColumns="true" GridLines="None" />
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>

    <script>
        // Enhance all tables after accordion expand
        document.addEventListener('shown.bs.collapse', function () {
            $('.requisitionTable').DataTable();
        });
    </script>
    <%-- ============================================= Requisition Summary ============================================= --%>
    <hr />
    <%-- ============================================= Project wise requisition ============================================= --%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h3>Requisition Report Dashboard</h3>

            <asp:DropDownList ID="ddlProject" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged" CssClass="form-control">
            </asp:DropDownList>

            <asp:GridView ID="gvRequisitionType" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered" EmptyDataText="No data found.">
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- ============================================= Project wise requisition ============================================= --%>
    <hr />
    <%-- ============================================= Total Requisitions by Status ============================================= --%>
    <h3>Total Requisitions by Status</h3>
    <asp:GridView ID="gvRequisitionStatus" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered" />
    <%-- ============================================= Total Requisitions by Status ============================================= --%>
    <hr />
    <%-- ============================================= Requisitions by Employee (Requester) ============================================= --%>
    <h3>Requisitions by Employee (Requester)</h3>
    <asp:GridView ID="gvRequisitionsByEmployee" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered" />
    <%-- ============================================= Requisitions by Employee (Requester) ============================================= --%>
    <hr />
    <%-- ============================================= Requisitions by Recipient Type (Requisition_For) ============================================= --%>
    <h3>Requisitions by Recipient Type (Requisition_For)</h3>
    <asp:GridView ID="gvRequisitionsByRecipientType" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered" />
    <%-- ============================================= Requisitions by Recipient Type (Requisition_For) ============================================= --%>

    <hr />
    <%-- ============================================= Monthly Summary (Pivot Example by Month) ============================================= --%>
    <h3>Monthly Summary (Pivot Example by Month)</h3>
    <asp:GridView ID="gvMonthlySummary" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered" />

    <%-- ============================================= Monthly Summary (Pivot Example by Month) ============================================= --%>
    <hr />

    <%-- ============================================= Material Consumption Summary ============================================= --%>
    <h3>Material Consumption Summary</h3>
    <asp:GridView ID="gvMaterialConsumption" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered" />

    <%-- ============================================= Material Consumption Summary ============================================= --%>
    <hr />

    <%-- ============================================= Requisitions by Project Code ============================================= --%>
    <h3>Requisitions by Project Code</h3>
    <asp:GridView ID="gvRequisitionsByProject" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered" />
    <%-- ============================================= Requisitions by Project Code ============================================= --%>
    <hr />

    <%-- ============================================= Pending vs Approved Count with Pivot ============================================= --%>
    <h3>Pending vs Approved Count with Pivot</h3>

    <asp:GridView ID="gvStatusPivot" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered" />
    <%-- ============================================= Pending vs Approved Count with Pivot ============================================= --%>
    <hr />

    <%-- ============================================= Requisitions by Zone ============================================= --%>
    <h3>Requisitions by Zone</h3>
    <asp:GridView ID="gvRequisitionsByZone" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered" />

    <%-- ============================================= Requisitions by Zone ============================================= --%>
    <hr />

    <%-- ============================================= Store Delivery Status Breakdown ============================================= --%>
    <h3>Store Delivery Status Breakdown</h3>
    <asp:GridView ID="gvDeliveryStatus" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered" />
    <%-- ============================================= Store Delivery Status Breakdown ============================================= --%>

</asp:Content>
