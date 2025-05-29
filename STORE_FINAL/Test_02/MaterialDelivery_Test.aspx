<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="MaterialDelivery_Test.aspx.cs" Inherits="STORE_FINAL.Test_02.MaterialDelivery_Test" %>

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
        <div class="form-group row mb-3">
            <label for="ddlRequisition" class="col-sm-2 col-form-label fw-bold">Requisition No:</label>
            <div class="col-sm-6">
                <asp:DropDownList ID="ddlRequisition" runat="server" CssClass="form-control" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlRequisition_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
<!-- Requisition Info Panel -->
<asp:Panel ID="pnlRequisitionInfo" runat="server" CssClass="card shadow-sm border-info mb-4" Visible="false">
    <div class="card-header bg-info text-white fw-bold">
        Requisition Information
    </div>
    <div class="card-body row">
        <div class="col-md-4">
            <label class="form-label">Requisition Date:</label>
            <asp:Label ID="lblRequisitionDate" runat="server" CssClass="form-control-plaintext text-dark"></asp:Label>
        </div>
        <div class="col-md-4">
            <label class="form-label">Requested By:</label>
            <asp:Label ID="lblRequestedBy" runat="server" CssClass="form-control-plaintext text-dark"></asp:Label>
        </div>
        <div class="col-md-4">
            <label class="form-label">Department:</label>
            <asp:Label ID="lblDepartment" runat="server" CssClass="form-control-plaintext text-dark"></asp:Label>
        </div>
        <div class="col-md-12 mt-3">
            <label class="form-label">Purpose:</label>
            <asp:Label ID="lblPurpose" runat="server" CssClass="form-control-plaintext text-dark"></asp:Label>
        </div>
    </div>
</asp:Panel>
        <!-- GridView for Materials -->
        <div class="table-scroll">
            <asp:GridView ID="gvMaterials" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered fixed-table"
                OnRowDataBound="gvMaterials_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="Material_ID" HeaderText="Material ID" ReadOnly="true"
                        HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                    <asp:BoundField DataField="Materials_Name" HeaderText="Material Name"
                        HeaderStyle-Width="200px" ItemStyle-Width="200px" />
                    <asp:BoundField DataField="Quantity" HeaderText="Required Qty"
                        HeaderStyle-Width="120px" ItemStyle-Width="120px" />

                    <%--Serial Number or Quantity Entry--%>
                    <asp:TemplateField HeaderText="Delivery Input"
                        HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Panel ID="pnlSerialInput" runat="server" Visible="false">
                                <asp:ListBox ID="txtSerialNumbers" runat="server" CssClass="form-control select2"
                                    SelectionMode="Multiple" data-placeholder="Enter Serial Numbers"></asp:ListBox>
                            </asp:Panel>
                            <asp:Panel ID="pnlQtyInput" runat="server" Visible="false">
                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control"
                                    TextMode="Number" placeholder="Enter Quantity" onblur="validateQty(this)"></asp:TextBox>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <!-- Submit Button -->
        <div class="text-end mt-4">
            <asp:Button ID="btnDeliver" runat="server" Text="Deliver Materials" CssClass="btn btn-primary"
                OnClick="btnDeliver_Click" />
        </div>
    </asp:Panel>

    <script>
        function validateQty(input) {
            if (parseFloat(input.value) <= 0) {
                alert("Quantity must be greater than 0.");
                input.value = "";
            }
        }
    </script>
</asp:Content>
