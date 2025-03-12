<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddNewEmployee.aspx.cs" Inherits="STORE_FINAL.Forms.AddNewEmployee" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <h2>Add New Employee</h2>

        <label>Employee ID:</label>
        <asp:TextBox ID="txtEmployeeID" runat="server"></asp:TextBox>
        <br /><br />

        <label>Name:</label>
        <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
        <br /><br />

        <label>Department:</label>
        <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="select2">
            <asp:ListItem Text="Select Department" Value="" />
            <asp:ListItem Text="HR&D" Value="HR" />
            <asp:ListItem Text="IT" Value="IT" />
            <asp:ListItem Text="Accounts" Value="Accounts" />
            <asp:ListItem Text="Store" Value="Store" />
            <asp:ListItem Text="Procurement" Value="Procurement" />
            <asp:ListItem Text="P&S" Value="Planning & Strategy" />
            <asp:ListItem Text="Existing Installation (EI) - Technical" Value="Existing Installation (EI) - Technical" />
            <asp:ListItem Text="Existing Installation (EI) - Sales" Value="Existing Installation (EI) - Sales" />
        </asp:DropDownList>
        <br /><br />


        <label>Role:</label>
        <asp:TextBox ID="txtRole" runat="server"></asp:TextBox>
        <br /><br />

        <asp:Button ID="btnAddEmployee" runat="server" Text="Add Employee" OnClick="btnAddEmployee_Click" />
        <br /><br />

        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

</asp:Content>
