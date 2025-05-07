<%@ Page Title="Receive New Stock" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="MaterialReceive.aspx.cs" Inherits="STORE_FINAL.Role_StoreIncharge.MaterialReceive" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <style>
        #rblReceiveType label {
            margin-right: 10px;
            padding: 8px 16px;
            border: 1px solid #ccc;
            border-radius: 4px;
            cursor: pointer;
            user-select: none;
            transition: all 0.2s ease-in-out;
        }

        #rblReceiveType input[type="radio"] {
            display: none;
        }

            #rblReceiveType input[type="radio"]:checked + label {
                background-color: #0d6efd;
                color: white;
                border-color: #0d6efd;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2 class="mb-4">📦 Receive New Stock</h2>
    <asp:Label ID="lblSession_2" runat="server" CssClass="alert"></asp:Label>
    <div class="card shadow p-4">
        <div class="row">
            <div class="col-md-12 mt-4">
                <asp:RadioButtonList ID="rblReceiveType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="btn-group w-100" OnSelectedIndexChanged="rblReceiveType_SelectedIndexChanged" ClientIDMode="Static">
                    <%-- All materials are new and active --%>
                    <asp:ListItem Text="New Receive" Value="NewReceive" Selected="True" />
                    <%-- Which materials are already dispatched and active --%>
                    <asp:ListItem Text="Return Active Receive" Value="ReturnActiveReceive" />
                    <%-- Which materials are already dispatched and defective --%>
                    <asp:ListItem Text="Return Defective Receive" Value="ReturnDefectiveReceive" />
                </asp:RadioButtonList>
            </div>
        </div>
        <div class="row d-none" id="ReturnAgainst" runat="server">

            <div class="col-md-6 form-group">
                <label><i class="fas fa-file-invoice"></i>Select Challan:</label>
                <asp:DropDownList ID="ddlChallanID" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlChallanID_SelectedIndexChanged" />
            </div>

            <div class="col-md-6 form-group">
                <label><i class="fas fa-list-ul"></i>Select Challan Item:</label>
                <asp:DropDownList ID="ddlChallanItemsID" runat="server" CssClass="form-control select2" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlChallanItemsID_SelectedIndexChanged"></asp:DropDownList>
            </div>

        </div>
        <div class="row">
            <div class="col-md-6">

                <div class="form-group">
                    <label><i class="fas fa-cubes"></i>Material Name:</label>
                    <asp:DropDownList ID="ddlMaterial" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlMaterial_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <div class="form-group">
                    <label><i class="fas fa-barcode"></i>Part ID:</label>
                    <asp:DropDownList ID="ddlPartID" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlPartID_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <div class="form-group">
                    <label><i class="fas fa-hashtag"></i>Serial Number:</label>
                    <asp:TextBox ID="txtSerialNumber" runat="server" CssClass="form-control" placeholder="Enter Serial Number" Enabled="false"></asp:TextBox>
                    <small class="text-danger d-none" id="serialError">Serial Number is required!</small>
                </div>

                <div class="form-group">
                    <label><i class="fas fa-cogs"></i>Quantity:</label>
                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" placeholder="Enter Quantity" Enabled="false" />
                    <small class="text-danger d-none" id="quantityError">Quantity is required!</small>
                </div>

            </div>
            <div class="col-md-6">

                <div class="form-group">
                    <label><i class="fas fa-th-large"></i>Rack Number:</label>
                    <asp:TextBox ID="txtRackNumber" runat="server" CssClass="form-control" placeholder="Enter Rack Number"></asp:TextBox>
                    <small class="text-danger d-none" id="rackError">Rack Number is required!</small>
                </div>

                <div class="form-group">
                    <label><i class="fas fa-align-left"></i>Shelf Number:</label>
                    <asp:TextBox ID="txtShelfNumber" runat="server" CssClass="form-control" placeholder="Enter Shelf Number"></asp:TextBox>
                    <small class="text-danger d-none" id="shelfError">Shelf Number is required!</small>
                </div>

                <div class="form-group">
                    <label><i class="fas fa-info-circle"></i>Status:</label>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Active" Value="ACTIVE"></asp:ListItem>
                        <asp:ListItem Text="Defective" Value="DEFECTIVE"></asp:ListItem>
                    </asp:DropDownList>
                </div>

            </div>
        </div>

        <!-- Submit Button -->
        <div class="mt-4">
            <asp:Button ID="btnAddStock" runat="server" Text="Add Stock" CssClass="btn btn-success btn-lg" OnClick="btnAddStock_Click" />
            <span id="loading" class="spinner-border spinner-border-sm text-success d-none"></span>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="alert d-none" Visible="false"></asp:Label>
    </div>






    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:Button ID="btnAddToReceiving" runat="server" Text="➕ Add Serial" CssClass="btn btn-primary" OnClick="btnAddToReceiving_Click" />

    <br />

    <!-- Temporary GridView -->
    <asp:GridView ID="gvReceivingItems" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" DataKeyNames="Temp_ID" OnRowCommand="gvReceivingItems_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="S.No">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Serial_Number" HeaderText="Serial Number" />
            <asp:BoundField DataField="Rack_Number" HeaderText="Rack" />
            <asp:BoundField DataField="Shelf_Number" HeaderText="Shelf" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="btnRemove" runat="server" CommandName="Remove"
                        CommandArgument='<%# Eval("Temp_ID") %>' CssClass="btn delete-icon">
                                <i class="fa fa-remove" style="color:red"></i>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>

    <br />

    <!-- Final Receive Button -->
    <%--<asp:Button ID="btnReceive" runat="server" Text="✅ Final Receive" CssClass="btn btn-success" OnClick="btnReceive_Click" />--%>
    <asp:Button ID="btnReceive" runat="server" Text="✅ Final Receive" CssClass="btn btn-success" />
    <br />
    <br />
    <asp:Label ID="Label1" runat="server" ForeColor="Green" />
    <asp:HiddenField ID="hfReceivingSessionID" runat="server" />































    <asp:HiddenField ID="hfReceiveSessionID" runat="server" />

    <%-- JavaScript for receive type selection --%>
    <%--    <script>
        function showDropdown() {
            var selected = document.querySelector('#rblReceiveType input[type="radio"]:checked');
            if (!selected) return;

            var selectedValue = selected.value;

            var returnAgainstContainer = document.getElementById('ReturnAgainst');

            if (selectedValue === 'ReturnActiveReceive' || selectedValue === 'ReturnDefectiveReceive') {
                returnAgainstContainer.classList.remove('d-none'); // Show
            } else {
                returnAgainstContainer.classList.add('d-none');    // Hide
                // Optionally clear selections:
                var dropdowns = returnAgainstContainer.querySelectorAll('select');
                dropdowns.forEach(function (ddl) {
                    ddl.selectedIndex = 0;
                });
            }
        }

        document.addEventListener("DOMContentLoaded", function () {
            showDropdown();

            // Attach change event to all radio buttons
            document.querySelectorAll('#rblReceiveType input[type="radio"]').forEach(function (rb) {
                rb.addEventListener("change", showDropdown);
            });
        });
</script>--%>
</asp:Content>
