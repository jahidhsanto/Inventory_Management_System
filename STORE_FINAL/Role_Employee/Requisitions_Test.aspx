<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="Requisitions_Test.aspx.cs" Inherits="STORE_FINAL.Test.Requisitions_Test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        body {
            background: #f4f6f9;
            font-family: 'Segoe UI', sans-serif;
        }

        .card {
            background: #fff;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
            padding: 2rem;
            margin-bottom: 2rem;
        }

        .form-title {
            font-size: 1.2rem;
            font-weight: 600;
            color: #444;
            border-bottom: 1px solid #ddd;
            margin-bottom: 1rem;
            padding-bottom: 0.5rem;
        }

        .form-group label {
            font-weight: 600;
            display: block;
        }

        .form-control:focus {
            border-color: #4c84ff;
            box-shadow: 0 0 0 0.2rem rgba(76, 132, 255, 0.25);
        }

        #rblRequisitionFor label {
            padding: 10px 20px;
            border-radius: 25px;
            border: 2px solid #4c84ff;
            margin: 0 10px 10px 0;
            background-color: white;
            color: #4c84ff;
            font-weight: 500;
            cursor: pointer;
            transition: 0.3s;
        }

        #rblRequisitionFor input[type="radio"] {
            display: none;
        }

            #rblRequisitionFor input[type="radio"]:checked + label {
                background-color: #4c84ff;
                color: white;
            }

        .btn {
            border-radius: 8px;
            font-weight: 600;
        }

        .btn-primary {
            background-color: #4c84ff;
            border: none;
        }

            .btn-primary:hover {
                background-color: #356ae6;
            }

        .btn-success {
            background-color: #38c172;
            border: none;
        }

            .btn-success:hover {
                background-color: #2e9d5e;
            }

        .grid-header {
            background-color: #4c84ff;
            color: #fff;
            font-weight: 600;
        }

        .delete-icon i {
            font-size: 1.2rem;
            color: #dc3545;
        }

        @media (max-width: 768px) {
            .btn {
                width: 100%;
                margin-bottom: 1rem;
            }
        }

        @media (max-width: 768px) {
            .table-responsive {
                overflow-x: auto;
            }
        }

        .table-hover tbody tr:hover {
            background-color: #f1f1f1;
        }

        .btn-outline-danger:hover {
            background-color: #dc3545;
            color: white;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="mb-4">➕ Add New Requisition</h2>

    <div class="card shadow p-4">
        <!-- Radio Button Group -->
        <div class="form-title">Select Requisition For</div>
        <div class="row mb-3">
            <div class="col-6">
                <asp:RadioButtonList ID="rblRequisitionFor" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="d-flex flex-wrap" OnSelectedIndexChanged="rblRequisitionFor_SelectedIndexChanged" ClientIDMode="Static">
                    <asp:ListItem Text="Project" Value="Project" Selected="True" />
                    <asp:ListItem Text="Employee" Value="Employee" />
                    <asp:ListItem Text="Department" Value="Department" />
                    <asp:ListItem Text="Zone" Value="Zone" />
                </asp:RadioButtonList>
            </div>
            <div class="col-6">
                <div class="row" id="dropdownProjectFor" runat="server" visible="true">
                    <div class="col-md-6 dropdown-container">
                        <label class="form-label">Select Project:</label>
                        <asp:DropDownList ID="ddlProjectFor" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="resetSelection_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="Static" />
                    </div>
                    <div class="col-md-6 dropdown-container">
                        <label class="form-label">Requisition Purpose:</label>
                        <asp:DropDownList ID="ddlRequisitionPurpose" runat="server" CssClass="form-control" OnSelectedIndexChanged="resetSelection_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="Static">
                            <asp:ListItem Text="-- Select Purpose --" Value="0" />
                            <asp:ListItem Text="Warranty Replacement" Value="Warranty Replacement" />
                            <asp:ListItem Text="Free of cost" Value="Free of cost" />
                            <asp:ListItem Text="Spare Sale" Value="Spare Sale" />
                            <asp:ListItem Text="Test Purpose" Value="Test Purpose" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 dropdown-container" id="dropdownEmployeeFor" runat="server" visible="false">
                    <label class="form-label">Select Employee:</label>
                    <asp:DropDownList ID="ddlEmployeeFor" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="resetSelection_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="Static" />
                </div>
                <div class="col-md-6 dropdown-container" id="dropdownDepartmentFor" runat="server" visible="false">
                    <label class="form-label">Select Department:</label>
                    <asp:DropDownList ID="ddlDepartmentFor" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="resetSelection_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="Static" />
                </div>
                <div class="col-md-6 dropdown-container" id="dropdownZoneFor" runat="server" visible="false">
                    <label class="form-label">Select Zone:</label>
                    <asp:DropDownList ID="ddlZoneFor" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="resetSelection_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="Static" />
                </div>
            </div>
        </div>

        <!-- Material Info -->
        <div class="row">
            <div class="col-md-3">
                <div class="form-group">
                    <label><i class="fas fa-cubes"></i>Material Name:</label>
                    <asp:DropDownList ID="ddlMaterial" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlMaterial_SelectedIndexChanged" />
                </div>
            </div>

            <div class="col-md-3">
                <div class="form-group">
                    <label><i class="fas fa-cogs"></i>Quantity:</label>
                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" placeholder="Enter Quantity" />
                </div>
            </div>

            <!-- Material Image -->
            <div class="col-md-3 text-center">
                <!-- Material Image (Thumbnail - Click to View) -->
                <asp:Image ID="imgMaterial" runat="server" CssClass="img-thumbnail shadow border p-1 bg-white"
                    Width="180px" Height="180px" AlternateText="Material Image"
                    onerror="this.onerror=null; this.src='~/images/LoGo.png';"
                    Style="cursor: pointer; transition: transform 0.3s ease;"
                    onclick="showLargeImage(this.src)" Visible="false" />
            </div>

        </div>

        <div class="text-end">
            <asp:LinkButton ID="btnAddItem" runat="server" CssClass="btn btn-primary" OnClick="btnAddItem_Click">
                <i class="bi bi-cart"></i> Add Serial
            </asp:LinkButton>
        </div>

    </div>

    <asp:GridView ID="gvItems" runat="server" CssClass="table table-bordered table-hover table-striped align-middle"
        AutoGenerateColumns="False" OnRowDeleting="gvItems_RowDeleting" DataKeyNames="Material_ID">
        <HeaderStyle CssClass="table-primary text-center" />
        <RowStyle CssClass="text-center" />

        <Columns>
            <asp:TemplateField HeaderText="S.No">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>

            <%--            <asp:BoundField DataField="Material_ID" HeaderText="Material ID" />--%>
            <asp:BoundField DataField="Material_Name" HeaderText="Material Name" />
            <asp:BoundField DataField="Quantity" HeaderText="Quantity" />

            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:LinkButton
                        ID="btnDelete"
                        runat="server"
                        CommandName="Delete"
                        Text="🗑 Delete"
                        CssClass="btn btn-danger btn-sm"
                        OnClientClick="return confirm('Are you sure you want to delete this item?');">
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>



    <!-- Temporary GridView -->
    <div class="table-responsive">

        <%--<asp:GridView ID="gvReceivingItems" runat="server" CssClass="table table-bordered table-hover table-striped align-middle" AutoGenerateColumns="False" DataKeyNames="Temp_ID" OnRowCommand="gvReceivingItems_RowCommand">--%>
        <asp:GridView ID="gvReceivingItems" runat="server" CssClass="table table-bordered table-hover table-striped align-middle"
            AutoGenerateColumns="False" DataKeyNames="Temp_ID">
            <HeaderStyle CssClass="table-primary text-center" />
            <RowStyle CssClass="text-center" />
            <Columns>
                <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Materials_Name" HeaderText="Material Name" />
                <asp:BoundField DataField="Serial_Number" HeaderText="Serial Number" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
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
    </div>

    <!-- Final Receive Button -->
    <asp:Button ID="btnSubmit" runat="server" Text="✅ Submit" CssClass="btn btn-success" OnClick="btnSubmit_Click" />

</asp:Content>
