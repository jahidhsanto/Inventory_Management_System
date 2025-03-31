<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="MaterialDelivery_Test.aspx.cs" Inherits="STORE_FINAL.Role_StoreIncharge.MaterialDelivery_Test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2 class="text-center mb-4">📦 Material Delivery</h2>

        <!-- Requisition Selection & Details -->
        <div class="row">
            <!-- Left Side: Requisition Details -->
            <div class="col-md-6">
                <label><strong>Requisition:</strong></label>
                <asp:DropDownList ID="ddlRequisition" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                <%--<asp:DropDownList ID="ddlRequisition" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlRequisition_SelectedIndexChanged"></asp:DropDownList>--%>
                
                <label><strong>Received By:</strong></label>
                <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>

                <div class="card mt-3 p-3 bg-light">
                    <h5>📋 Requisition Details</h5>
                    <p><strong>Material Name:</strong> <asp:Label ID="lblMaterialName" runat="server" Text="-"></asp:Label></p>
                    <p><strong>Requested By:</strong> <asp:Label ID="lblRequestedBy" runat="server" Text="-"></asp:Label></p>
                    <p><strong>Requested Date:</strong> <asp:Label ID="lblRequestedDate" runat="server" Text="-"></asp:Label></p>
                </div>
            </div>

            <!-- Right Side: Requisition Image -->
            <div class="col-md-6 d-flex align-items-center justify-content-center">
                <div class="card text-center p-3">
                    <h5>📷 Requisition Image</h5>
                    <asp:Image ID="imgRequisition" runat="server" CssClass="img-fluid rounded" Height="200px" Width="100%" ImageUrl="~/images/LoGo.jpg" />
                </div>
            </div>
        </div>

        <!-- Select Stock Item -->
        <div class="row mt-4">
            <div class="col-md-3">
                <label><strong>Stock Item:</strong></label>
                <asp:DropDownList ID="ddlStock" runat="server" CssClass="form-control select2"></asp:DropDownList>
            </div>
            <div class="col-md-3">
                <label><strong>Material:</strong></label>
                <asp:DropDownList ID="ddlMaterial" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                <%--<asp:DropDownList ID="ddlMaterial" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlMaterial_SelectedIndexChanged"></asp:DropDownList>--%>
            </div>
            <div class="col-md-3">
                <label><strong>Serial Number (Optional):</strong></label>
                <asp:TextBox ID="txtSerialNumber" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <label><strong>Quantity:</strong></label>
                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <!-- Add to Delivery Button -->
        <div class="text-center mt-4">
            <asp:Button ID="btnAddToDelivery" runat="server" Text="➕ Add to Delivery" CssClass="btn btn-primary btn-lg" />
            <%--<asp:Button ID="btnAddToDelivery" runat="server" Text="➕ Add to Delivery" CssClass="btn btn-primary btn-lg" OnClick="btnAddToDelivery_Click" />--%>
        </div>

        <!-- Selected Items for Delivery -->
        <h3 class="mt-4 text-center">📦 Selected Items for Delivery</h3>
        <div class="table-responsive">
            <asp:GridView ID="gvDeliveryItems" runat="server" CssClass="table table-bordered text-center" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="Temp_ID" HeaderText="Temp ID" />
                    <asp:BoundField DataField="Material_ID" HeaderText="Material ID" />
                    <asp:BoundField DataField="Stock_ID" HeaderText="Stock ID" />
                    <asp:BoundField DataField="Serial_Number" HeaderText="Serial Number" />
                    <asp:BoundField DataField="Delivered_Quantity" HeaderText="Quantity" />
                    <asp:CommandField ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>
        </div>

        <!-- Deliver Button -->
        <div class="text-center mt-3">
            <asp:Button ID="btnDeliver" runat="server" Text="🚚 Deliver Items" CssClass="btn btn-success btn-lg" />
            <%--<asp:Button ID="btnDeliver" runat="server" Text="🚚 Deliver Items" CssClass="btn btn-success btn-lg" OnClick="btnDeliver_Click" />--%>
        </div>
    </div>

    <asp:HiddenField ID="hfDeliverySessionID" runat="server" />

    <!-- JavaScript: Store Session ID -->
    <script>
        if (!sessionStorage.getItem("DeliverySessionID")) {
            sessionStorage.setItem("DeliverySessionID", generateSessionID());
        }
        document.getElementById('<%= hfDeliverySessionID.ClientID %>').value = sessionStorage.getItem("DeliverySessionID");

        function generateSessionID() {
            return 'S' + Math.random().toString(36).substr(2, 9);
        }
    </script>

    <!-- JavaScript: Clear Temp Data on Tab Close -->
    <script>
        window.addEventListener("beforeunload", function () {
            fetch("ClearTempData.aspx?sessionID=" + sessionStorage.getItem("DeliverySessionID"), { method: "GET" });
        });
    </script>
</asp:Content>
