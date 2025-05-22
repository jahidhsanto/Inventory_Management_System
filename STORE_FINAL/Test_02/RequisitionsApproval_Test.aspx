<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="RequisitionsApproval_Test.aspx.cs" Inherits="STORE_FINAL.Test_02.RequisitionsApproval_Test" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h3 class="mb-4">Requisition Approvals</h3>

        <asp:GridView ID="gvRequisitions" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped"
            OnRowCommand="gvRequisitions_RowCommand" DataKeyNames="Requisition_ID">
            <Columns>
                <asp:BoundField DataField="Requisition_ID" HeaderText="Requisition No" />
                <asp:BoundField DataField="Created_Date" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="CreatedByEmployee_ID" HeaderText="Requested By" />
                <asp:BoundField DataField="Dept_Status" HeaderText="Status" />
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button runat="server" CssClass="btn btn-sm btn-primary me-2" CommandName="View" CommandArgument='<%# Eval("Requisition_ID") %>' Text="View" />
                        <asp:Button runat="server" CssClass="btn btn-sm btn-success me-2" CommandName="Approve" CommandArgument='<%# Eval("Requisition_ID") %>' Text="Approve" />
                        <asp:Button runat="server" CssClass="btn btn-sm btn-danger" CommandName="Reject" CommandArgument='<%# Eval("Requisition_ID") %>' Text="Reject" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="detailModal" tabindex="-1" aria-labelledby="detailModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Requisition Details</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblModalContent" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript">
        function showModal() {
            var modal = new bootstrap.Modal(document.getElementById('detailModal'));
            modal.show();
        }
    </script>

</asp:Content>
