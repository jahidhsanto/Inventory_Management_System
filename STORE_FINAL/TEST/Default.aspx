<%@ Page Title="" Language="C#" MasterPageFile="~/TEST/MasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="STORE_FINAL.TEST.Default" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    Home - My Website
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Welcome to My Website</h2>
    <p>This is a simple ASP.NET Web Application using Master Pages.</p>

    <!-- User Interface: Registration Form -->
    <h3>Register</h3>
    <asp:Label ID="lblName" runat="server" Text="Name:"></asp:Label>
    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
    <br />

    <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
    <br />

    <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label>
    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
    <br />

    <asp:Button ID="btnSubmit" runat="server" Text="Register" OnClick="btnSubmit_Click" />
</asp:Content>
