﻿<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="MaterialDelivery_Test_02.aspx.cs" Inherits="STORE_FINAL.Test_02.MaterialDelivery_Test_02" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .table-scroll {
            overflow-x: auto;
            width: 100%;
        }

        .fixed-table {
            table-layout: fixed;
            width: 100%;
        }

            .fixed-table th, .fixed-table td {
                white-space: nowrap;
                overflow: hidden;
                text-overflow: ellipsis;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="pnlMaterialDelivery" runat="server" CssClass="container mt-4">

        <!-- Requisition Number Dropdown -->
        <div class="form-group row mb-3 align-items-center">
            <label for="ddlRequisition" class="col-sm-2 col-form-label">Requisition No:</label>
            <div class="col-sm-6">
                <asp:DropDownList ID="ddlRequisition" runat="server" CssClass="form-select" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlRequisition_SelectedIndexChanged" ToolTip="Select Requisition Number">
                </asp:DropDownList>
            </div>
        </div>

        <!-- Requisition Info Panel -->
        <asp:UpdatePanel ID="updMaterialDelivery" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlRequisitionDeliveryInfo" runat="server" CssClass="card shadow border-start border-3 border-primary mb-3" Visible="false">
                    <div class="card-header bg-primary text-white">
                        <i class="bi bi-info-circle-fill me-2"></i>Material Delivery Context
                    </div>
                    <div class="card-body">
                        <!-- Requisition Basic Info -->
                        <div class="row g-2 mb-2">
                            <div class="col-md-2 d-flex">
                                <label class="form-label fw-semibold me-2 mb-0 flex-shrink-0">Requisition No:</label>
                                <asp:Label ID="lblReqNo" runat="server" CssClass="badge bg-primary ms-2" />
                            </div>
                            <div class="col-md-3 d-flex">
                                <label class="form-label fw-semibold me-2 mb-0 flex-shrink-0">Created Date:</label>
                                <asp:Label ID="lblCreatedDate" runat="server" CssClass="text-dark" />
                            </div>
                            <div class="col-md-3 d-flex">
                                <label class="form-label fw-semibold me-2 mb-0 flex-shrink-0">Requisition Type:</label>
                                <asp:Label ID="lblReqType" runat="server" CssClass="text-dark" />
                            </div>
                            <div class="col-md-4 d-flex">
                                <label class="form-label fw-semibold me-2 mb-0 flex-shrink-0">Requested By:</label>
                                <asp:Label ID="lblRequestedBy" runat="server" CssClass="text-dark" />
                            </div>
                        </div>

                        <!-- Recipient Info -->
                        <div class="row g-2 mb-2">
                            <div class="col-md-6 d-flex">
                                <label class="form-label fw-semibold me-2 mb-0 flex-shrink-0">Recipient Info:</label>
                                <asp:Label ID="lblRecipientInfo" runat="server" CssClass="text-dark" />
                            </div>
                            <div class="col-md-6 d-flex">
                                <label class="form-label fw-semibold me-2 mb-0 flex-shrink-0">Purpose:</label>
                                <asp:Label ID="lblPurpose" runat="server" CssClass="text-dark" />
                            </div>
                        </div>

                        <!-- Department Approval -->
                        <div class="row g-2 border-top pt-3 mb-2">
                            <div class="col-md-2 d-flex">
                                <label class="form-label fw-semibold me-2 mb-0 flex-shrink-0">Dept Status:</label>
                                <asp:Label ID="lblDeptStatus" runat="server" CssClass="text-dark" />
                            </div>
                            <div class="col-md-3 d-flex">
                                <label class="form-label fw-semibold me-2 mb-0 flex-shrink-0">Approved By:</label>
                                <asp:Label ID="lblDeptApprovedBy" runat="server" CssClass="text-dark" />
                            </div>
                            <div class="col-md-3 d-flex">
                                <label class="form-label fw-semibold me-2 mb-0 flex-shrink-0">Approval Date:</label>
                                <asp:Label ID="lblDeptApprovalDate" runat="server" CssClass="text-dark" />
                            </div>
                            <div class="col-md-4">
                                <label class="form-label fw-semibold mb-1">Remarks:</label>
                                <asp:Label ID="lblDeptRemarks" runat="server" CssClass="text-dark d-block" />
                            </div>
                        </div>

                        <!-- Store Approval -->
                        <div class="row g-2 border-top pt-3">
                            <div class="col-md-2 d-flex">
                                <label class="form-label fw-semibold me-2 mb-0 flex-shrink-0">Store Status:</label>
                                <asp:Label ID="lblStoreStatus" runat="server" CssClass="text-dark" />
                            </div>
                            <div class="col-md-3 d-flex">
                                <label class="form-label fw-semibold me-2 mb-0 flex-shrink-0">Approved By:</label>
                                <asp:Label ID="lblStoreApprovedBy" runat="server" CssClass="text-dark" />
                            </div>
                            <div class="col-md-3 d-flex">
                                <label class="form-label fw-semibold me-2 mb-0 flex-shrink-0">Approval Date:</label>
                                <asp:Label ID="lblStoreApprovalDate" runat="server" CssClass="text-dark" />
                            </div>
                            <div class="col-md-4">
                                <label class="form-label fw-semibold mb-1">Remarks:</label>
                                <asp:Label ID="lblStoreRemarks" runat="server" CssClass="text-dark d-block" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlRequisition" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnDeliver" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

        <!-- GridView for Materials -->
        <div class="table-scroll">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="gvMaterials" runat="server" DataKeyNames="Material_ID" AutoGenerateColumns="False"
                        CssClass="table table-hover table-bordered fixed-table align-middle"
                        OnRowDataBound="gvMaterials_RowDataBound"
                        OnRowCommand="gvMaterials_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="Material_ID" HeaderText="Material ID" ReadOnly="true"
                                HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="Materials_Name" HeaderText="Material Name"
                                HeaderStyle-Width="200px" ItemStyle-Width="200px" />
                            <asp:BoundField DataField="Quantity" HeaderText="Required Qty"
                                HeaderStyle-Width="80px" ItemStyle-Width="80px" />

                            <%--Serial Number or Quantity Entry--%>
                            <asp:TemplateField HeaderText="Quantity / Serial / Location"
                                HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Panel ID="pnlSerialInput" runat="server" Visible="false">
                                        <%--                                        <asp:ListBox ID="txtSerialNumbers" runat="server" CssClass="form-control select2"
                                            SelectionMode="Multiple" data-placeholder="Enter Serial Numbers"
                                            onchange="limitSelection(this, 3)"></asp:ListBox>--%>
                                        <asp:ListBox ID="txtSerialNumbers" runat="server" CssClass="form-control select2"
                                            SelectionMode="Multiple" data-placeholder="Enter Serial Numbers"
                                            onchange=""></asp:ListBox>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlQtyInput" runat="server" Visible="false">
                                        <div style="display: flex; gap: 10px; align-items: center;">
                                            <div>
                                                <asp:Label runat="server" Text="Qty: " AssociatedControlID="txtQuantity" />
                                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control"
                                                    TextMode="Number" placeholder="Enter Quantity" onblur="validateQty(this)"></asp:TextBox>
                                            </div>
                                            <div>
                                                <asp:Label runat="server" Text="Location: " AssociatedControlID="ddlLocation" />
                                                <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-select"></asp:DropDownList>
                                            </div>
                                            <div>
                                                <!-- + icon button -->
                                                <asp:LinkButton ID="btnAddRow" runat="server" CommandName="AddRow" CommandArgument='<%# Container.DataItemIndex %>'
                                                    CssClass="btn btn-sm btn-outline-primary">
                                                    <i class="bi bi-plus-circle"></i>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlRequisition" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="btnDeliver" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <!-- Submit Button -->
        <div class="text-end mt-4">
            <asp:Button ID="btnDeliver" runat="server" Text="Deliver Materials" CssClass="btn btn-success px-4 py-2 shadow-sm"
                OnClick="btnDeliver_Click" />
        </div>
    </asp:Panel>
    <script type="text/javascript">
        // Keep track of previous selection
        var previousSelections = [];

        function limitSelection(listBox, maxSelection) {
            var currentSelections = [];
            for (var i = 0; i < listBox.options.length; i++) {
                if (listBox.options[i].selected) {
                    currentSelections.push(i);
                }
            }

            if (currentSelections.length > maxSelection) {
                // Determine the new selection (last clicked)
                let newlySelectedIndex = currentSelections.find(i => !previousSelections.includes(i));

                if (newlySelectedIndex !== undefined) {
                    listBox.options[newlySelectedIndex].selected = false;
                    alert("You can select up to " + maxSelection + " items only.");
                }
            } else {
                // Update previousSelections only when within limit
                previousSelections = currentSelections;
            }
        }
    </script>

    <script>
        function validateQty(input) {
            if (parseFloat(input.value) <= 0) {
                alert("Quantity must be greater than 0.");
                input.value = "";
                input.focus();
            }
        }
    </script>
</asp:Content>
