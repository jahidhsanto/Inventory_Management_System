<%@ Page Title="" Language="C#" MasterPageFile="~/UserDashboard/MasterDashboard.Master" AutoEventWireup="true" CodeBehind="RequisitionApproval.aspx.cs" Inherits="STORE_FINAL.Role_DepartmentHead.RequisitionApproval" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="mt-3 text-center">Requisition Management</h2>

        <asp:Label ID="lblMessage" runat="server" CssClass="alert d-none"></asp:Label>

        <!-- Search Bar -->
        <div class="row">
            <div class="col-md-6">
                <label>🔍 Search Requisition:</label>
                <input type="text" id="searchRequisition" class="form-control" placeholder="Type to search requisition..." onkeyup="filterRequisition()">
            </div>
            <div class="col-md-6">
                <label>🔍 Search Requisition:</label>
                <input type="text" id="searchRequisition" class="form-control" placeholder="Type to search requisition..." onkeyup="filterRequisition()">
            </div>
        </div>


    </div>
</asp:Content>
