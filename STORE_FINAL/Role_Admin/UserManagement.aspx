<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="STORE_FINAL.UserAuthentication.UserManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="mt-3 text-center">User Management</h2>

        <asp:Label ID="lblMessage" runat="server" CssClass="alert d-none"></asp:Label>

        <div class="accordion mt-4" id="userManagementAccordion">

            <!-- Create User Section -->
            <div class="accordion-item">
                <h2 class="accordion-header" id="headingCreate">
                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseCreate" aria-expanded="false" aria-controls="collapseCreate">
                        ➕ Create New User
                    </button>
                </h2>
                <div id="collapseCreate" class="accordion-collapse collapse" aria-labelledby="headingCreate" data-bs-parent="#userManagementAccordion">
                    <div class="accordion-body">
                        <div class="row">
                            <div class="col-md-6">
                                <label>Employee ID:</label>
                                <asp:TextBox ID="txtEmployeeID" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label>Username:</label>
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label>Role:</label>
                                <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label>Password:</label>
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="text-center mt-3">
                            <asp:Button ID="btnCreateUser" runat="server" Text="Create User" CssClass="btn btn-primary" OnClick="btnCreateUser_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- Update Role Section -->
            <div class="accordion-item">
                <h2 class="accordion-header" id="headingUpdate">
                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseUpdate" aria-expanded="false" aria-controls="collapseUpdate">
                        ✏️ Update User Role
                    </button>
                </h2>
                <div id="collapseUpdate" class="accordion-collapse collapse" aria-labelledby="headingUpdate" data-bs-parent="#userManagementAccordion">
                    <div class="accordion-body">
                        <div class="row">
                            <div class="col-md-6">
                                <label>Username:</label>
                                <asp:TextBox ID="txtUpdateUsername" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label>New Role:</label>
                                <asp:DropDownList ID="ddlNewRole" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="text-center mt-3">
                            <asp:Button ID="btnUpdateRole" runat="server" Text="Update Role" CssClass="btn btn-warning" OnClick="btnUpdateRole_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- Delete User Section -->
            <div class="accordion-item">
                <h2 class="accordion-header" id="headingDelete">
                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseDelete" aria-expanded="false" aria-controls="collapseDelete">
                        ❌ Delete User
                    </button>
                </h2>
                <div id="collapseDelete" class="accordion-collapse collapse" aria-labelledby="headingDelete" data-bs-parent="#userManagementAccordion">
                    <div class="accordion-body">
                        <div class="row">
                            <div class="col-md-12">
                                <label>Username:</label>
                                <asp:TextBox ID="txtDeleteUsername" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="text-center mt-3">
                            <asp:Button ID="btnDeleteUser" runat="server" Text="Delete User" CssClass="btn btn-danger" OnClick="btnDeleteUser_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- Reset Password Section -->
            <div class="accordion-item">
                <h2 class="accordion-header" id="headingResetPassword">
                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseResetPassword" aria-expanded="false" aria-controls="collapseResetPassword">
                        🔑 Reset Password
                    </button>
                </h2>
                <div id="collapseResetPassword" class="accordion-collapse collapse" aria-labelledby="headingResetPassword" data-bs-parent="#userManagementAccordion">
                    <div class="accordion-body">
                        <div class="row">
                            <div class="col-md-6">
                                <label>Username:</label>
                                <asp:TextBox ID="txtResetUsername" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label>New Password:</label>
                                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="text-center mt-3">
                            <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password" CssClass="btn btn-info" OnClick="btnResetPassword_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Search Bar -->
        <div class="mt-5">
            <label>🔍 Search User:</label>
            <input type="text" id="searchUser" class="form-control" placeholder="Type to search users..." onkeyup="filterUsers()">
        </div>

        <!-- User List Table -->
        <h3 class="mt-3 text-center">📋 All Users</h3>
        <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered mt-3">
            <Columns>
                <asp:BoundField DataField="Username" HeaderText="Username" />
                <asp:BoundField DataField="Role" HeaderText="Role" />
                <asp:BoundField DataField="Employee_ID" HeaderText="Employee ID" />
                <asp:BoundField DataField="Name" HeaderText="Employee Name" />
                <asp:BoundField DataField="DepartmentName" HeaderText="Department" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- JavaScript for Live Search -->
    <script>
        function filterUsers() {
            var input, filter, table, tr, td, i, txtValue;
            input = document.getElementById("searchUser");
            filter = input.value.toUpperCase();
            table = document.getElementById("<%= gvUsers.ClientID %>");
            tr = table.getElementsByTagName("tr");

            for (i = 1; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td")[0]; // Username column
                if (td) {
                    txtValue = td.textContent || td.innerText;
                    if (txtValue.toUpperCase().indexOf(filter) > -1) {
                        tr[i].style.display = "";
                    } else {
                        tr[i].style.display = "none";
                    }
                }
            }
        }
    </script>

</asp:Content>
