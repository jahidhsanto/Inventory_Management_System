<%@ Page Title="" Language="C#" MasterPageFile="~/TEST/MasterPage.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="STORE_FINAL.TEST.Dashboard" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Welcome to the Dashboard</h2>
    <p>Hello, <asp:Label ID="lblUsername" runat="server"></asp:Label></p>

    <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" CssClass="logout-button" />
</asp:Content>
