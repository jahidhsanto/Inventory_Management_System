<%@ Page Title="" Language="C#" MasterPageFile="~/UserMaster.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="STORE_FINAL.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Welcome to the Dashboard!</h2>
    <div class="row">
        <div class="col-md-12">
            <h3>Your Role:
                <asp:Label ID="lblUserRole" runat="server" Text=""></asp:Label></h3>
            <h4>Welcome,
                <asp:Label ID="lblUsername" runat="server" Text=""></asp:Label>!</h4>
        </div>
    </div>

    <!-- Other content specific to the dashboard -->



    <h2>Welcome to your Dashboard!</h2>

    <div class="container">
        <div class="row">
            <!-- Card 1 -->
            <div class="col-md-4 col-sm-6 mb-4">
                <div class="card" style="width: 18rem;">
                    <div class="card-header">
                        Card Title
                    </div>
                    <div class="card-body">
                        <h5 class="card-title">Card Heading</h5>
                        <p class="card-text">This is some content for the card. You can add text, images, or any other content here.</p>
                        <a href="#" class="btn btn-primary">Go somewhere</a>
                    </div>
                </div>
            </div>

            <!-- Card 2 (Optional) -->
            <div class="col-md-4 col-sm-6 mb-4">
                <div class="card" style="width: 18rem;">
                    <div class="card-header">
                        Card Title
                    </div>
                    <div class="card-body">
                        <h5 class="card-title">Card Heading</h5>
                        <p class="card-text">This is some content for the card. You can add text, images, or any other content here.</p>
                        <a href="#" class="btn btn-primary">Go somewhere</a>
                    </div>
                </div>
            </div>
        </div>
    </div>




</asp:Content>
