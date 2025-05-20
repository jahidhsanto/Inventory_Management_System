<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="Requisition.aspx.cs" Inherits="STORE_FINAL.Role_Employee.Requisition" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <style>
        #rblRequisitionFor label {
            margin-right: 10px;
            padding: 8px 16px;
            border: 1px solid #ccc;
            border-radius: 4px;
            cursor: pointer;
            user-select: none;
            transition: all 0.2s ease-in-out;
        }
        #rblRequisitionFor input[type="radio"] {
            display: none;
        }
        #rblRequisitionFor input[type="radio"]:checked + label {
            background-color: #0d6efd;
            color: white;
            border-color: #0d6efd;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="mt-3 text-center">Requisition Management</h2>

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
                            <div class="col-md-12 mt-4">
                                <asp:RadioButtonList ID="rblRequisitionFor" runat="server" RepeatDirection="Horizontal" CssClass="btn-group w-100" ClientIDMode="Static">
                                    <asp:ListItem Text="Project" Value="Project" Selected="True" />
                                    <asp:ListItem Text="Employee" Value="Employee" />
                                    <asp:ListItem Text="Department" Value="Department" />
                                    <asp:ListItem Text="Zone" Value="Zone" />
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 dropdown-container" id="dropdownProjectFor">
                                <label class="form-label">Select Project:</label>
                                <asp:DropDownList ID="ddlProjectFor" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                <asp:DropDownList ID="ddlRequisitionType" runat="server" CssClass="form-control select2">
                                    <asp:ListItem Text="Warranty Replacement" Value="Warranty Replacement" />
                                    <asp:ListItem Text="Free of cost" Value="Free of cost" />
                                    <asp:ListItem Text="Spare Sale" Value="Spare Sale" />
                                    <asp:ListItem Text="Test Purpose" Value="Test Purpose" />
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6 dropdown-container d-none" id="dropdownEmployeeFor">
                                <label class="form-label">Select Employee:</label>
                                <asp:DropDownList ID="ddlEmployeeFor" runat="server" CssClass="form-control select2"></asp:DropDownList>
                            </div>
                            <div class="col-md-6 dropdown-container d-none" id="dropdownDepartmentFor">
                                <label class="form-label">Select Department:</label>
                                <asp:DropDownList ID="ddlDepartmentFor" runat="server" CssClass="form-control select2"></asp:DropDownList>
                            </div>
                            <div class="col-md-6 dropdown-container d-none" id="dropdownZoneFor">
                                <label class="form-label">Select Zone:</label>
                                <asp:DropDownList ID="ddlZoneFor" runat="server" CssClass="form-control select2"></asp:DropDownList>
                            </div>                            
                            
                            <div class="col-md-6 dropdown-container">
                                <label class="form-label">Select Material:</label>
                                <asp:DropDownList ID="ddlMaterials" runat="server" CssClass="form-control select2" Width="100%"></asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">Quantity:</label>
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

    <%-- JavaScript for requisition type selection --%>
    <script>
        function showDropdown() {
            var selected = document.querySelector('#rblRequisitionFor input[type="radio"]:checked');
            if (!selected) return;

            var selectedValue = selected.value;

            // List of all dropdown IDs
            const dropdowns = {
                Project: document.getElementById('dropdownProjectFor'),
                Employee: document.getElementById('dropdownEmployeeFor'),
                Department: document.getElementById('dropdownDepartmentFor'),
                Zone: document.getElementById('dropdownZoneFor')
            };

            // Loop through each dropdown
            for (const type in dropdowns) {
                const container = dropdowns[type];
                const ddl = container.querySelector('select');

                if (type === selectedValue) {
                    container.classList.remove('d-none'); // Show
                } else {
                    container.classList.add('d-none');    // Hide
                    if (ddl) ddl.selectedIndex = 0;       // Clear selection
                }
            }
        }

        document.addEventListener("DOMContentLoaded", function () {
            showDropdown();

            // Attach change event to all radio buttons
            document.querySelectorAll('#rblRequisitionFor input[type="radio"]').forEach(function (rb) {
                rb.addEventListener("change", showDropdown);
            });
        });
    </script>

</asp:Content>

