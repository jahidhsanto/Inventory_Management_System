<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="Requisition.aspx.cs" Inherits="STORE_FINAL.Role_Employee.Requisition" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="mt-3 text-center">Requisition Management</h2>

        <asp:Label ID="lblMessage" runat="server" CssClass="alert d-none"></asp:Label>

        <div class="accordion mt-4" id="requisitionManagementAccordion">

            <div class="accordion-item">
                <h2 class="accordion-header" id="headingCreate">
                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseCreate" aria-expanded="false" aria-controls="collapseCreate">
                        ➕ Add New Requisition
                    </button>
                </h2>
                <div id="collapseCreate" class="accordion-collapse collapse" aria-labelledby="headingCreate" data-bs-parent="#requisitionManagementAccordion">
                    <div class="accordion-body">
                        <div class="row">
                            <div class="col-md-6">
                                <label>Select Project:</label>
                                <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control select2" Width="100%"></asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label>Select Material:</label>
                                <asp:DropDownList ID="ddlMaterials" runat="server" CssClass="form-control select2" Width="100%"></asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label>Quantity:</label>
                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="text-center mt-3">
                            <asp:Button ID="btnSubmitRequisition" runat="server" Text="Submit Request" CssClass="btn btn-primary" OnClick="btnSubmitRequisition_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Search Bar -->
        <div class="mt-5">
            <label>🔍 Search Requisition:</label>
            <input type="text" id="searchRequisition" class="form-control" placeholder="Type to search requisition..." onkeyup="filterRequisition()">
        </div>

        <h3 class="mt-3 text-center">📋 My All Requisition</h3>
        <asp:GridView ID="RequisitionGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered mt-3">
            <Columns>
                <asp:BoundField DataField="Requisition_ID" HeaderText="Requisition ID" />
                <asp:BoundField DataField="Materials_Name" HeaderText="Material Name" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                <asp:BoundField DataField="Created_Date" HeaderText="Created Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="Dept_Status" HeaderText="Department Status" />
                <asp:BoundField DataField="Dept_Head" HeaderText="Department Head" />
                <asp:BoundField DataField="Store_Status" HeaderText="Store Status" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- JavaScript for Live Search -->
    <script>
        function filterRequisition() {
            var input, filter, table, tr, td, i, j, txtValue, found;
            input = document.getElementById("searchRequisition");
            filter = input.value.toUpperCase();
            table = document.getElementById("<%= RequisitionGridView.ClientID %>");
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

