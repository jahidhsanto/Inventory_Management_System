<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="STORE_FINAL.Dashboard" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Welcome to the Dashboard!</h2>
    <div class="row">
        <div class="col-12">
            <h4>Your Username: <asp:Label ID="lblUsername" runat="server" Text=""></asp:Label></h4>
            <h4>Your Role: <asp:Label ID="lblRole" runat="server" Text=""></asp:Label></h4>
            <h4>Your EmployeeID: <asp:Label ID="lblID" runat="server" Text=""></asp:Label></h4>
            <h4>Your EmployeeName: <asp:Label ID="lblName" runat="server" Text=""></asp:Label></h4>
            <h4>Your EmployeeDepartment: <asp:Label ID="lblDepartment" runat="server" Text=""></asp:Label></h4>
            <h4>Your EmployeeDesignation: <asp:Label ID="lblDesignation" runat="server" Text=""></asp:Label></h4>
        </div>
    </div>

    <!-- Other content specific to the dashboard -->
    <div class="container">
        <div class="row">
            <!-- Card 1 -->
            <div class="col-lg-3 col-md-4 col-sm-6 mb-3">
                <div class="card">
<%--                    <div class="card-header">
                        Employee
                    </div>--%>
                    <div class="card-body">
                        <h5 class="card-title">Employee</h5>
                        <a href="/Forms/AddNewEmployee" class="btn btn-primary">+</a>
                        <a href="/Pages/Employee" class="btn btn-primary">Employee List</a>
                    </div>
                </div>
            </div>

            <!-- Card 2 -->
            <div class="col-lg-3 col-md-4 col-sm-6 mb-3">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Department</h5>
                        <a href="#" class="btn btn-primary">+</a>
                        <a href="/Pages/Department" class="btn btn-primary">Department List</a>
                    </div>
                </div>
            </div>

            <!-- Card 3 -->
            <div class="col-lg-3 col-md-4 col-sm-6 mb-3">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Material</h5>
                        <a href="/Forms/AddNewMaterial" class="btn btn-primary">+</a>
                        <a href="/Pages/Material" class="btn btn-primary">Material List</a>
                        <a href="/Forms/ReceiveStock" class="btn btn-primary">Receive Stock</a>
                    </div>
                </div>
            </div>

            <!-- Card 4 -->
            <div class="col-lg-3 col-md-4 col-sm-6 mb-3">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Requisition</h5>
                        <a href="/Forms/RequisitionForm" class="btn btn-primary">+</a>
                        <a href="/Pages/Requisition" class="btn btn-primary">Requisition List</a>
                        <a href="/Forms/ApproveRequisition" class="btn btn-primary">ApproveRequisition</a>
                    </div>
                </div>
            </div>

            <!-- Card 5 -->
            <div class="col-lg-3 col-md-4 col-sm-6 mb-3">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Stock</h5>
                        <a href="#" class="btn btn-primary">+</a>
                        <a href="/Pages/Stock" class="btn btn-primary">Stock List</a>
                    </div>
                </div>
            </div>

            <!-- Card 6 -->
            <div class="col-lg-3 col-md-4 col-sm-6 mb-3">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Challan</h5>
                        <a href="#" class="btn btn-primary">+</a>
                        <a href="/Pages/Challan" class="btn btn-primary">Challan List</a>
                    </div>
                </div>
            </div>

            <!-- Card 7 -->
            <div class="col-lg-3 col-md-4 col-sm-6 mb-3">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Purchase_Request</h5>
                        <a href="#" class="btn btn-primary">+</a>
                        <a href="/Pages/Purchase_Request" class="btn btn-primary">Purchase_Request List</a>
                    </div>
                </div>
            </div>

            <!-- Card 8 -->
            <div class="col-lg-3 col-md-4 col-sm-6 mb-3">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Vendor</h5>
                        <a href="#" class="btn btn-primary">+</a>
                        <a href="/Pages/Vendor" class="btn btn-primary">Vendor List</a>
                    </div>
                </div>
            </div>

            <!-- Card 9 -->
            <div class="col-lg-3 col-md-4 col-sm-6 mb-3">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Procurement</h5>
                        <a href="#" class="btn btn-primary">+</a>
                        <a href="/Pages/Procurement" class="btn btn-primary">Procurement List</a>
                    </div>
                </div>
            </div>

            <!-- Card 10 -->
            <div class="col-lg-3 col-md-4 col-sm-6 mb-3">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Material_Receipt</h5>
                        <a href="/Forms/AddNewMaterial" class="btn btn-primary">+</a>
                        <a href="/Pages/Material_Receipt" class="btn btn-primary">Material_Receipt List</a>
                    </div>
                </div>
            </div>

            <!-- Card 11 -->
            <div class="col-lg-3 col-md-4 col-sm-6 mb-3">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Return_Table</h5>
                        <a href="#" class="btn btn-primary">+</a>
                        <a href="/Pages/Return_Table" class="btn btn-primary">Return_Table List</a>
                    </div>
                </div>
            </div>

            <!-- Card 12 -->
            <div class="col-lg-3 col-md-4 col-sm-6 mb-3">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Warranty</h5>
                        <a href="#" class="btn btn-primary">+</a>
                        <a href="/Pages/Warranty" class="btn btn-primary">Warranty List</a>
                    </div>
                </div>
            </div>

            <!-- Card 13 -->
            <div class="col-lg-3 col-md-4 col-sm-6 mb-3">
                <div class="card">
                    <div class="card-header">
                        Card Title
                    </div>
                    <div class="card-body">
                        <h5 class="card-title">Card Heading</h5>
                        <a href="#" class="btn btn-primary">+</a>
                        <a href="#" class="btn btn-primary">List</a>
                    </div>
                </div>
            </div>

            <!-- Card 14 -->
            <div class="col-lg-3 col-md-4 col-sm-6 mb-3">
                <div class="card">
                    <div class="card-header">
                        Card Title
                    </div>
                    <div class="card-body">
                        <h5 class="card-title">Card Heading</h5>
                        <a href="#" class="btn btn-primary">+</a>
                        <a href="#" class="btn btn-primary">List</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
