<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LogIn.aspx.cs" Inherits="STORE_FINAL.UserAuthentication.LogIn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="container">
    <div class="row justify-content-center">
        <div class="col-md-6 col-lg-4"> <!-- Adjust width -->
            <div class="card card-primary card-outline mb-4">
                <div class="card-header text-center">
                    <div class="card-title">User Login</div>
                </div>
                <form>
                    <div class="card-body">
                        <div class="mb-3">
                            <label for="txtUserName" class="form-label">Email address</label>
                            <asp:TextBox ID="txtUserName" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="txtPassword" class="form-label">Password</label>
                            <asp:TextBox ID="txtPassword" runat="server" class="form-control" TextMode="Password"></asp:TextBox>
                        </div>
                    </div>
                    <div class="card-footer">
                        <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                    </div
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </form>
            </div>
        </div>
    </div>
</div>

</asp:Content>
