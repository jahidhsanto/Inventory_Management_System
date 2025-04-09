<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="RequisitionApproval.aspx.cs" Inherits="STORE_FINAL.Role_DepartmentHead.RequisitionApproval" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="text-center">ApproveRequisition</h2>

    <asp:Panel ID="PanelFilters" runat="server" CssClass="mb-3">
        <div class="row">
            <div class="col-md-3">
                <label>🔍 Search Requisition:</label>
                <input type="text" id="searchRequisitionApproval" class="form-control" placeholder="Type to search requisition..." onkeyup="filterRequisition()">
            </div>

            <div class="col-md-3">
                <label for="ddlProject">Filter by Project:</label>
                <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control select2">
                    <%--AutoPostBack="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">--%>
                </asp:DropDownList>
            </div>

            <div class="col-md-3">
                <label for="ddlEmployee">Filter by Employee:</label>
                <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control select2">
                    <%--AutoPostBack="true" OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged">--%>
                </asp:DropDownList>
            </div>

            <div class="col-md-3">
                <label for="ddlStatus">Filter by Status:</label>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="false">
                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                    <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                    <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                    <asp:ListItem Text="Rejected" Value="Rejected"></asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary mt-2" OnClick="btnFilter_Click" />
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="Panel1" runat="server" CssClass="table-responsive" Style="overflow-x: auto; white-space: normal;">
        <contenttemplate>
            <asp:GridView
                ID="RequisitionApprovalGridView"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="table table-bordered table-striped"
                HorizontalAlign="Left"
                DataKeyNames="Requisition_ID"
                OnRowCommand="ApproveRequisitionGridView_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="S.No">
                        <itemtemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </itemtemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Requisition_ID" HeaderText="Requisition ID" ReadOnly="True" />
                    <asp:BoundField DataField="Materials_Name" HeaderText="Material Name" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="Project_Name" HeaderText="Project Name" />
                    <asp:BoundField DataField="Requested_By" HeaderText="Requested By" />
                    <asp:BoundField DataField="Created_Date" HeaderText="Requested Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="Dept_Status" HeaderText="Dept Status" />
                    <asp:BoundField DataField="Store_Status" HeaderText="Store Status" />

                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="btn btn-success"
                                CommandName="Approve" CommandArgument='<%# Eval("Requisition_ID") %>'
                                Visible='<%# Eval("Dept_Status").ToString() == "Pending" %>' />
                            <asp:Button ID="btnPending" runat="server" Text="Pending" CssClass="btn btn-warning"
                                CommandName="Pending" CommandArgument='<%# Eval("Requisition_ID") %>'
                                Visible='<%# (Eval("Dept_Status").ToString() == "Approved" || 
                                              Eval("Dept_Status").ToString() == "Rejected") &&
                                             (Eval("Store_Status").ToString() == "Pending" ||
                                              Eval("Store_Status") == DBNull.Value) %>' />
                            <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn btn-danger"
                                CommandName="Reject" CommandArgument='<%# Eval("Requisition_ID") %>'
                                Visible='<%# Eval("Dept_Status").ToString() == "Pending" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </asp:Panel>
    
    <!-- JavaScript for Live Search -->
    <script>
        function filterRequisition() {
            var input, filter, table, tr, td, i, j, txtValue, found;
            input = document.getElementById("searchRequisitionApproval");
            filter = input.value.toUpperCase();
            table = document.getElementById("<%= RequisitionApprovalGridView.ClientID %>");
            tr = table.getElementsByTagName("tr");

            for (i = 1; i < tr.length; i++) { // Start from 1 to skip the header row
                td = tr[i].getElementsByTagName("td");
                found = false; // Reset found flag for each row

                for (j = 0; j < td.length; j++) { // Loop through all columns
                    if (td[j]) {
                        txtValue = td[j].textContent || td[j].innerText;
                        if (txtValue.toUpperCase().indexOf(filter) > -1) {
                            found = true; // Match found in any column
                            break; // Stop checking further columns in this row
                        }
                    }
                }

                // Show or hide row based on search result
                tr[i].style.display = found ? "" : "none";
            }
        }
    </script>

</asp:Content>
