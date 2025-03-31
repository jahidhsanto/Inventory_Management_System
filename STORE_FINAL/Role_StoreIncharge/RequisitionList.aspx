<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="RequisitionList.aspx.cs" Inherits="STORE_FINAL.Role_StoreIncharge.RequisitionList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="text-center">ApproveRequisition</h2>

    <asp:Panel ID="PanelFilters" runat="server" CssClass="mb-3">
        <div class="row">
            <div class="col-md-6">
                <label>🔍 Search Requisition:</label>
                <input type="text" id="searchRequisitionApproval" class="form-control" placeholder="Type to search requisition..." onkeyup="filterhRequisitionApproval()">
            </div>

            <div class="col-md-6">
                <label for="ddlStoreStatus">Filter by Store Status:</label>
                <asp:DropDownList ID="ddlStoreStatus" runat="server" CssClass="form-control" AutoPostBack="false">
                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                    <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                    <asp:ListItem Text="Processing" Value="Processing"></asp:ListItem>
                    <asp:ListItem Text="Out of Stock" Value="Out of Stock"></asp:ListItem>
                    <asp:ListItem Text="Ordered" Value="Ordered"></asp:ListItem>
                    <asp:ListItem Text="Delivered" Value="Delivered"></asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary mt-2" OnClick="btnFilter_Click" />
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="Panel1" runat="server" CssClass="table-responsive" Style="overflow-x: auto; white-space: nowrap;">
        <contenttemplate>
            <asp:GridView
                ID="RequisitionApprovalGridView"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="table table-bordered table-striped"
                HorizontalAlign="Left"
                DataKeyNames="Requisition_ID"
                OnRowCommand="ApproveRequisitionGridView_RowCommand">
                <columns>
                    <asp:BoundField DataField="Requisition_ID" HeaderText="Req ID" />
                    <asp:BoundField DataField="Materials_Name" HeaderText="Material Name" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="Stock_Quantity" HeaderText="Stock" />
                    <asp:BoundField DataField="Requested_By" HeaderText="Requested By" />
                    <asp:BoundField DataField="Created_Date" HeaderText="Req Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="Dept_Head" HeaderText="Reviewed By" />
                    <asp:BoundField DataField="Store_Status" HeaderText="Store Status" />

                    <asp:TemplateField HeaderText="Actions">
                        <itemtemplate>
                            <!-- Show 'Processing' button when Store_Status is 'Pending' -->
                            <asp:Button ID="btnProcessing" runat="server" Text="Processing" CssClass="btn btn-info"
                                CommandName="Processing" CommandArgument='<%# Eval("Requisition_ID") %>'
                                Visible='<%# Eval("Store_Status").ToString() == "Pending" %>' />

                            <!-- Show 'Out of Stock' button when Store_Status is 'Pending' -->
                            <asp:Button ID="btnOutOfStock" runat="server" Text="Out of Stock" CssClass="btn btn-danger"
                                CommandName="OutOfStock" CommandArgument='<%# Eval("Requisition_ID") %>'
                                Visible='<%# Eval("Store_Status").ToString() == "Pending" %>' />

                            <!-- Show 'Ordered' button when Store_Status is 'Pending' -->
                            <asp:Button ID="btnOrdered" runat="server" Text="Ordered" CssClass="btn btn-warning"
                                CommandName="Ordered" CommandArgument='<%# Eval("Requisition_ID") %>'
                                Visible='<%# Eval("Store_Status").ToString() == "Pending" %>' />

                            <!-- Show 'Delivered' button when Store_Status is 'Processing' -->
                            <asp:Button ID="btnDelivered" runat="server" Text="Delivered" CssClass="btn btn-success"
                                CommandName="Delivered" CommandArgument='<%# Eval("Requisition_ID") %>'
                                Visible='<%# Eval("Store_Status").ToString() == "Processing" %>' />

                            <!-- Show 'Ordered' button when Store_Status is 'Out of Stock' -->
                            <asp:Button ID="Button1" runat="server" Text="Ordered" CssClass="btn btn-warning"
                                CommandName="Ordered" CommandArgument='<%# Eval("Requisition_ID") %>'
                                Visible='<%# Eval("Store_Status").ToString() == "Out of Stock" %>' />

                            <!-- Show 'Pending' button when Store_Status is 'Ordered' -->
                            <asp:Button ID="btnPending" runat="server" Text="Pending" CssClass="btn btn-primary"
                                CommandName="Pending" CommandArgument='<%# Eval("Requisition_ID") %>'
                                Visible='<%# Eval("Store_Status").ToString() == "Ordered" %>' />
                        </itemtemplate>
                    </asp:TemplateField>

                </columns>
            </asp:GridView>
        </contenttemplate>
        <triggers>
            <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
        </triggers>

    </asp:Panel>

    <!-- JavaScript for Live Search -->
    <script>
        function filterhRequisitionApproval() {
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
