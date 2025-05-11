<%@ Page Title="Receive New Stock" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="MaterialReceive.aspx.cs" Inherits="STORE_FINAL.Role_StoreIncharge.MaterialReceive" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
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

        #rblReceiveType label {
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

        #rblReceiveType input[type="radio"] {
            display: none;
        }

            #rblReceiveType input[type="radio"]:checked + label {
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

    <h2 class="mb-4">📦 Receive New Stock</h2>
    <asp:Label ID="lblSession_2" runat="server" CssClass="alert"></asp:Label>


    <div class="card shadow p-4">
        <!-- Radio Button Group -->
        <div class="form-title">Select Receive Type</div>
        <div class="row mb-3">
            <div class="col-12">
                <asp:RadioButtonList ID="rblReceiveType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="d-flex flex-wrap" OnSelectedIndexChanged="rblReceiveType_SelectedIndexChanged" ClientIDMode="Static">
                    <asp:ListItem Text="New Receive" Value="NewReceive" Selected="True" />
                    <asp:ListItem Text="Return Active" Value="ReturnActiveReceive" />
                    <asp:ListItem Text="Return Defective" Value="ReturnDefectiveReceive" />
                </asp:RadioButtonList>
            </div>
        </div>

        <!-- Challan Fields -->
        <div class="row" id="ReturnAgainst" runat="server" visible="false">
            <div class="form-title">Return Against Challan</div>
            <div class="col-md-6 form-group">
                <label><i class="fas fa-file-invoice"></i>Select Challan:</label>
                <asp:DropDownList ID="ddlChallanID" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlChallanID_SelectedIndexChanged" />
            </div>
            <div class="col-md-6 form-group">
                <label><i class="fas fa-list-ul"></i>Select Challan Item:</label>
                <asp:DropDownList ID="ddlChallanItemsID" runat="server" CssClass="form-control select2" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlChallanItemsID_SelectedIndexChanged" />
            </div>
        </div>

        <!-- Material Details -->
        <div class="form-title">Material Details</div>
        <div class="row">
            <div class="col-md-6">

                <div class="form-group">
                    <label><i class="fas fa-cubes"></i>Material Name:</label>
                    <asp:DropDownList ID="ddlMaterial" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlMaterial_SelectedIndexChanged" />
                </div>

                <div class="form-group">
                    <label><i class="fas fa-hashtag"></i>Serial Number:</label>
                    <asp:TextBox ID="txtSerialNumber" runat="server" CssClass="form-control" placeholder="Enter Serial Number" Enabled="false" />
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
                    <label><i class="fas fa-barcode"></i>Part ID:</label>
                    <asp:DropDownList ID="ddlPartID" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlPartID_SelectedIndexChanged" />
                </div>


                <div class="form-group">
                    <label><i class="fas fa-th-large"></i>Rack Number:</label>
                    <asp:TextBox ID="txtRackNumber" runat="server" CssClass="form-control" placeholder="Enter Rack Number" />
                    <small class="text-danger d-none" id="rackError">Rack Number is required!</small>
                </div>

                <div class="form-group">
                    <label><i class="fas fa-align-left"></i>Shelf Number:</label>
                    <asp:TextBox ID="txtShelfNumber" runat="server" CssClass="form-control" placeholder="Enter Shelf Number" />
                    <small class="text-danger d-none" id="shelfError">Shelf Number is required!</small>
                </div>
            </div>
        </div>

        <!-- Submit Button -->
        <%--        <div class="mt-4">
            <asp:Button ID="btnAddStock" runat="server" Text="Add Stock" CssClass="btn btn-success btn-lg" OnClick="btnAddStock_Click" />
            <span id="loading" class="spinner-border spinner-border-sm text-success d-none"></span>
        </div>--%>

        <div class="text-end">
            <asp:LinkButton ID="btnAddToReceiving" runat="server" CssClass="btn btn-primary" OnClick="btnAddToReceiving_Click">
        <i class="bi bi-cart"></i> Add Serial
            </asp:LinkButton>
        </div>

    </div>

    <!-- Temporary GridView -->
    <div class="table-responsive">

        <asp:GridView ID="gvReceivingItems" runat="server" CssClass="table table-bordered table-hover table-striped align-middle" AutoGenerateColumns="False" DataKeyNames="Temp_ID" OnRowCommand="gvReceivingItems_RowCommand">
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
    <asp:Button ID="btnReceive" runat="server" Text="✅ Final Receive" CssClass="btn btn-success" OnClick="btnReceive_Click" />
    <asp:Label ID="Label1" runat="server" ForeColor="Green" />

    <asp:HiddenField ID="hfReceiveSessionID" runat="server" />
</asp:Content>
