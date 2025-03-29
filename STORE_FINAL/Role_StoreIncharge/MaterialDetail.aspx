<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="MaterialDetail.aspx.cs" Inherits="STORE_FINAL.Role_StoreIncharge.MaterialDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-4">
        <h2 class="text-primary">
            <i class="fas fa-box"></i>Material Details
        </h2>

        <!-- Material Information Card -->
        <div class="card shadow-sm">
            <div class="card-body">
                <div class="row">
                    <!-- Material Image -->
                    <div class="col-md-3 text-center">
                        <%--<asp:Image ID="imgMaterial" runat="server" CssClass="img-fluid rounded shadow" Width="180px" Height="180px" />--%>
                        <asp:Image ID="imgMaterial" ImageUrl="~/images/jahidhsanto.jpg" runat="server" CssClass="img-fluid rounded shadow" Width="180px" Height="180px" />
                    </div>

                    <!-- Material Details -->
                    <div class="col-md-9">
                        <h4 class="card-title text-secondary">
                            <asp:Label ID="lblMaterialName" runat="server"></asp:Label>
                        </h4>

                        <div class="row">
                            <div class="col-md-6">
                                <p><strong>Part ID:</strong><asp:Label ID="lblPartId" runat="server"></asp:Label></p>
                                <p><strong>Category:</strong><asp:Label ID="lblCategory" runat="server"></asp:Label></p>
                                <p><strong>Sub-Category:</strong><asp:Label ID="lblSubCategory" runat="server"></asp:Label></p>
                                <p><strong>Model:</strong><asp:Label ID="lblModel" runat="server"></asp:Label></p>
                                <p><strong>Control:</strong><asp:Label ID="lblControl" runat="server"></asp:Label></p>
                            </div>

                            <div class="col-md-6">
                                <p><strong>Unit Price:</strong><asp:Label ID="lblUnitPrice" runat="server"></asp:Label></p>
                                <p><strong>Commercial?:</strong><asp:Label ID="lblComNonCom" runat="server"></asp:Label></p>
                                <p><strong>Asset Status:</strong><asp:Label ID="lblAssetStatus" runat="server"></asp:Label></p>
                                <p><strong>Asset Type:</strong><asp:Label ID="lblAssetType" runat="server"></asp:Label></p>
                                <p><strong>Stock Quantity:</strong><asp:Label ID="lblStockQuantity" runat="server"></asp:Label></p>

                                <!-- Stock Progress Bar -->
                                <div class="progress">
                                    <div id="progressStock" runat="server" class="progress-bar" role="progressbar"
                                        style="width: 0%;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                                    </div>
                                </div>

                                <p>
                                    <strong>MSQ:</strong>
                                    <asp:Label ID="lblMSQ" runat="server"></asp:Label>
                                </p>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Action Buttons -->
                <div class="mt-4">
                    <asp:Button ID="btnEdit" runat="server" Text="✏️ Edit" CssClass="btn btn-warning" />
                    <asp:Button ID="btnPrint" runat="server" Text="🖨️ Print" CssClass="btn btn-secondary" OnClientClick="window.print(); return false;" />
                    <asp:Button ID="btnDownloadPDF" runat="server" Text="📄 Download PDF" CssClass="btn btn-success" />
                    <asp:HyperLink ID="lnkBack" runat="server" NavigateUrl="MaterialList.aspx" CssClass="btn btn-outline-primary">
                        <i class="fas fa-arrow-left"></i> Back to List
                    </asp:HyperLink>
                </div>
            </div>
        </div>
    </div>

    <h4>📍 Material Locations</h4>

    <asp:GridView ID="gvMaterialTracking" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
        <Columns>
            <asp:BoundField DataField="Serial_Number" HeaderText="Serial Number" />
            <asp:BoundField DataField="Rack_Number" HeaderText="Rack Number" />
            <asp:BoundField DataField="Shelf_Number" HeaderText="Shelf Number" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:BoundField DataField="Received_Date" HeaderText="Received Date" DataFormatString="{0:dd-MMM-yyyy}" />
        </Columns>
    </asp:GridView>


</asp:Content>
