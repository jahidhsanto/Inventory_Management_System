<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="Employee.aspx.cs" Inherits="STORE_FINAL.Role_Admin.AddEmployee" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="mt-3 text-center">Employee Management</h2>

        <div class="accordion mt-4" id="employeeManagementAccordion">

            <div class="accordion-item">
                <h2 class="accordion-header" id="headingCreate">
                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseCreate" aria-expanded="false" aria-controls="collapseCreate">
                        ➕ Add New Employee
                    </button>
                </h2>
                <div id="collapseCreate" class="accordion-collapse collapse" aria-labelledby="headingCreate" data-bs-parent="#employeeManagementAccordion">
                    <div class="accordion-body">
                        <div class="row">
                            <div class="col-md-6">
                                <label>Employee ID:</label>
                                <asp:TextBox ID="txtEmployeeID" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label>Name:</label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label>Select Department:</label>
                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label>Designation:</label>
                                <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="text-center mt-3">
                            <asp:Button ID="btnAddEmployee" runat="server" Text="Add Employee" CssClass="btn btn-primary" OnClick="btnAddEmployee_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Search Bar -->
        <div class="mt-5">
            <label>🔍 Search Employee:</label>
            <input type="text" id="searchUser" class="form-control" placeholder="Type to search employee..." onkeyup="filterEmployee()">
        </div>

        <!-- Employee List Table -->
        <h3 class="mt-3 text-center">📋 All Employee</h3>
        <asp:GridView ID="EmployeeGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered mt-3">
            <Columns>
                <asp:BoundField DataField="Employee_ID" HeaderText="Employee ID" />
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:BoundField DataField="Designation" HeaderText="Designation" />
                <asp:BoundField DataField="DepartmentName" HeaderText="Department" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- JavaScript for Live Search -->
    <script>
        function filterEmployee() {
            var input, filter, table, tr, td, i, j, txtValue, found;
            input = document.getElementById("searchUser");
            filter = input.value.toUpperCase();
            table = document.getElementById("<%= EmployeeGridView.ClientID %>");
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
